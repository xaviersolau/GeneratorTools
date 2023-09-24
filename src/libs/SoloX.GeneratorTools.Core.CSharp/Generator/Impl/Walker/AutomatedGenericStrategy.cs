// ----------------------------------------------------------------------
// <copyright file="AutomatedGenericStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Utils;
using SoloX.GeneratorTools.Core.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedGenericStrategy : IAutomatedStrategy
    {
        private readonly IGenericDeclaration<SyntaxNode> declaration;
        private readonly IGenericDeclaration<SyntaxNode> pattern;
        private readonly IDeclarationResolver resolver;
        private readonly IEnumerable<string> ignoreUsingList;
        private readonly ISelectorResolver selectorResolver;
        private readonly string targetDeclarationName;
        private readonly string targetPatternName;
        private readonly IEnumerable<IReplacePatternHandler> replacePatternHandlers;

        public AutomatedGenericStrategy(
            IGenericDeclaration<SyntaxNode> pattern,
            IGenericDeclaration<SyntaxNode> declaration,
            IDeclarationResolver resolver,
            IEnumerable<IReplacePatternHandlerFactory> replacePatternHandlerFactories,
            IEnumerable<string> ignoreUsingList,
            ISelectorResolver selectorResolver)
        {
            this.declaration = declaration;
            this.pattern = pattern;
            this.resolver = resolver;
            this.ignoreUsingList = ignoreUsingList;
            this.targetDeclarationName = GeneratorHelper.ComputeClassName(declaration.Name);
            this.targetPatternName = GeneratorHelper.ComputeClassName(pattern.Name);
            this.selectorResolver = selectorResolver;

            this.replacePatternHandlers = replacePatternHandlerFactories.Select(f => f.Setup(pattern, declaration)).ToArray();
        }

        public IReplacePatternHandler CreateReplacePatternHandler()
        {
            return new StrategyReplacePatternHandler(ApplyPatternReplace);
        }

        public string ApplyPatternReplace(string text)
        {
            string result;
            if (this.targetPatternName.Length < this.pattern.Name.Length)
            {
                result = text
                    .Replace(this.pattern.Name, this.declaration.Name)
                    .Replace(this.targetPatternName, this.targetDeclarationName);
            }
            else
            {
                result = text
                    .Replace(this.targetPatternName, this.targetDeclarationName)
                    .Replace(this.pattern.Name, this.declaration.Name);
            }

            foreach (var replacePatternHandler in this.replacePatternHandlers)
            {
                result = replacePatternHandler.ApplyOn(result);
            }

            return result;
        }

        public string GetCurrentNameSpace()
        {
            return this.declaration.DeclarationNameSpace;
        }

        public void RepeatDeclaration(
            AttributeSyntax repeatAttributeSyntax,
            Action<IAutomatedStrategy> callback)
        {
            var patternArgumentExp = repeatAttributeSyntax.ArgumentList?.Arguments
                .FirstOrDefault(a => a.NameEquals.Name.ToString() == nameof(RepeatAttribute.Pattern))?.Expression;
            var patternPrefixExp = repeatAttributeSyntax.ArgumentList?.Arguments
                .FirstOrDefault(a => a.NameEquals.Name.ToString() == nameof(RepeatAttribute.Prefix))?.Expression;
            var patternSuffixExp = repeatAttributeSyntax.ArgumentList?.Arguments
                .FirstOrDefault(a => a.NameEquals.Name.ToString() == nameof(RepeatAttribute.Suffix))?.Expression;

            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>(this.resolver, this.declaration);

            var patternName = patternArgumentExp != null ? constEvaluator.Visit(patternArgumentExp) : null;
            var patternPrefix = patternPrefixExp != null ? constEvaluator.Visit(patternPrefixExp) : null;
            var patternSuffix = patternSuffixExp != null ? constEvaluator.Visit(patternSuffixExp) : null;

            if (!string.IsNullOrEmpty(patternName))
            {
                var resolvedPattern = this.resolver.Resolve(patternName, this.pattern);

                // Check if this is self repeat pattern reference.
                if (object.ReferenceEquals(this.pattern, resolvedPattern))
                {
                    callback(this);
                    return;
                }
            }

            if (this.pattern.Members.Count != 1 && string.IsNullOrEmpty(patternName))
            {
                throw new InvalidOperationException($"The Repeat pattern name must be specified since several members are defined in {this.pattern.Name}");
            }

            // get the member from the current pattern generic definition.
            var repeatPatternMember = string.IsNullOrEmpty(patternName)
                ? this.pattern.Members.Single()
                : this.pattern.Members.First(p => p.Name == patternName);

            ISelector selector;

            // Get the selector if any from the matching property.
            if (repeatPatternMember.Attributes
                .TryMatchAttributeName<PatternAttribute>(out var attribute))
            {
                selector = this.GetSelectorFromPatternAttribute(attribute);
            }
            else
            {
                selector = new AllSelector();
            }

            if (repeatPatternMember is IPropertyDeclaration repeatProperty)
            {
                foreach (var propertyDeclaration in selector.GetProperties(this.declaration))
                {
                    var strategy = new AutomatedPropertyStrategy(repeatProperty, propertyDeclaration, patternPrefix, patternSuffix, this);

                    callback(strategy);
                }
            }
            else if (repeatPatternMember is IMethodDeclaration repeatMethod)
            {
                foreach (var methodDeclaration in selector.GetMethods(this.declaration))
                {
                    var strategy = new AutomatedMethodStrategy(repeatMethod, methodDeclaration, this.resolver, this.declaration, patternPrefix, patternSuffix);

                    callback(strategy);
                }
            }
            else if (repeatPatternMember is IConstantDeclaration repeatConstant)
            {
                foreach (var constantDeclaration in selector.GetConstants(this.declaration))
                {
                    var strategy = new AutomatedConstantStrategy(repeatConstant, constantDeclaration, patternPrefix, patternSuffix);

                    callback(strategy);
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void RepeatNameSpace(Action<string> nsCallback)
        {
            nsCallback(this.declaration.DeclarationNameSpace);
        }

        public bool IgnoreUsingDirective(string ns)
        {
            return this.pattern.DeclarationNameSpace.Equals(ns, StringComparison.Ordinal) || this.ignoreUsingList.Contains(ns);
        }

        public string ComputeTargetName()
        {
            return this.targetDeclarationName;
        }

        private ISelector GetSelectorFromPatternAttribute(IAttributeUse attribute)
        {
            var patternAttributeUse = (IGenericDeclarationUse)attribute.DeclarationUse;

            var selectorTypeUse = patternAttributeUse.GenericParameters.First();

            var selector = this.selectorResolver.GetSelector(selectorTypeUse);
            if (selector != null)
            {
                return selector;
            }

            throw new ArgumentException($"Unknown selector {attribute}");
        }

        public bool TryMatchAndRepeatStatement(
            SyntaxNode? patternNameExpression,
            SyntaxNode? patternPrefixExpression,
            SyntaxNode? patternSuffixExpression,
            Action<IAutomatedStrategy> callback)
        {
            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>(this.resolver, this.declaration);

            var pName = constEvaluator.Visit(patternNameExpression);

            string patternPrefix = null;
            string patternSuffix = null;

            if (string.IsNullOrEmpty(pName))
            {
                return false;
            }

            var repeatPatternMember = string.IsNullOrEmpty(pName)
                ? this.pattern.Members.SingleOrDefault()
                : this.pattern.Members.FirstOrDefault(p => p.Name == pName);

            if (repeatPatternMember == null)
            {
                return false;
            }

            ISelector selector;

            // Get the selector if any from the matching property.
            if (repeatPatternMember.Attributes
                .TryMatchAttributeName<PatternAttribute>(out var attribute))
            {
                selector = this.GetSelectorFromPatternAttribute(attribute);
            }
            else
            {
                selector = new AllSelector();
            }

            if (repeatPatternMember is IPropertyDeclaration repeatProperty)
            {
                foreach (var propertyDeclaration in selector.GetProperties(this.declaration))
                {
                    var strategy = new AutomatedPropertyStrategy(repeatProperty, propertyDeclaration, patternPrefix, patternSuffix, this);

                    callback(strategy);
                }
            }
            else if (repeatPatternMember is IMethodDeclaration repeatMethod)
            {
                foreach (var methodDeclaration in selector.GetMethods(this.declaration))
                {
                    var strategy = new AutomatedMethodStrategy(repeatMethod, methodDeclaration, this.resolver, this.declaration, patternPrefix, patternSuffix);

                    callback(strategy);
                }
            }
            else if (repeatPatternMember is IConstantDeclaration repeatConstant)
            {
                foreach (var constantDeclaration in selector.GetConstants(this.declaration))
                {
                    var strategy = new AutomatedConstantStrategy(repeatConstant, constantDeclaration, patternPrefix, patternSuffix);

                    callback(strategy);
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
