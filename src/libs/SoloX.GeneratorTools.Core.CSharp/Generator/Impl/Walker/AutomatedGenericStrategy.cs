﻿// ----------------------------------------------------------------------
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
        private readonly string targetDeclarationName;
        private readonly string targetPatternName;
        private readonly IEnumerable<IReplacePatternHandler> replacePatternHandlers;

        public AutomatedGenericStrategy(
            IGenericDeclaration<SyntaxNode> pattern,
            IGenericDeclaration<SyntaxNode> declaration,
            IDeclarationResolver resolver,
            IEnumerable<IReplacePatternHandlerFactory> replacePatternHandlerFactories,
            IEnumerable<string> ignoreUsingList)
        {
            this.declaration = declaration;
            this.pattern = pattern;
            this.resolver = resolver;
            this.ignoreUsingList = ignoreUsingList;
            this.targetDeclarationName = GeneratorHelper.ComputeClassName(declaration.Name);
            this.targetPatternName = GeneratorHelper.ComputeClassName(pattern.Name);

            this.replacePatternHandlers = replacePatternHandlerFactories.Select(f => f.Setup(pattern, declaration)).ToArray();
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

        public bool TryMatchRepeatDeclaration(AttributeSyntax repeatAttributeSyntax, string expression)
        {
            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>();
            var patternName = constEvaluator.Visit(repeatAttributeSyntax.ArgumentList.Arguments.First().Expression);

            // get the property from the current pattern generic definition.
            var repeatProperty = this.pattern.Properties.First(p => p.Name == patternName);

            return AutomatedPropertyStrategy.Match(repeatProperty, expression);
        }

        public void RepeatDeclaration(
            AttributeSyntax repeatAttributeSyntax,
            Action<IAutomatedStrategy> callback)
        {
            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>();
            var patternName = constEvaluator.Visit(repeatAttributeSyntax.ArgumentList.Arguments.First().Expression);

            var resolved = this.resolver.Resolve(patternName, this.pattern);

            // Check if this is self repeat pattern reference.
            if (object.ReferenceEquals(this.pattern, resolved))
            {
                callback(this);
                return;
            }

            // get the member from the current pattern generic definition.
            var repeatMember = this.pattern.Members.First(p => p.Name == patternName);

            if (repeatMember is IPropertyDeclaration repeatProperty)
            {
                ISelector selector;

                // Get the selector if any from the matching property.
                if (repeatProperty.SyntaxNodeProvider.SyntaxNode.AttributeLists
                    .TryMatchAttributeName<PatternAttribute>(out var attributeSyntax))
                {
                    selector = this.GetSelectorFromPatternAttribute(attributeSyntax);
                }
                else
                {
                    selector = new AllPropertySelector();
                }

                foreach (var propertyDeclaration in selector.GetProperties(this.declaration))
                {
                    var strategy = new AutomatedPropertyStrategy(repeatProperty, propertyDeclaration, this.replacePatternHandlers);

                    callback(strategy);
                }
            }
            else if (repeatMember is IMethodDeclaration repeatMethod)
            {
                ISelector selector;

                // Get the selector if any from the matching property.
                if (repeatMethod.SyntaxNodeProvider.SyntaxNode.AttributeLists
                    .TryMatchAttributeName<PatternAttribute>(out var attributeSyntax))
                {
                    selector = this.GetSelectorFromPatternAttribute(attributeSyntax);
                }
                else
                {
                    selector = new AllMethodSelector();
                }

                foreach (var methodDeclaration in selector.GetMethods(this.declaration))
                {
                    var strategy = new AutomatedMethodStrategy(repeatMethod, methodDeclaration, this.replacePatternHandlers);

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

        private ISelector GetSelectorFromPatternAttribute(AttributeSyntax attributeSyntax)
        {
            var selectorExp = attributeSyntax.ArgumentList.Arguments.First();

            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>();
            var selectorTypeName = constEvaluator.Visit(selectorExp.Expression);

            var nsBase = this.pattern.UsingDirectives
                .Concat(NameSpaceHelper.GetParentNameSpaces(this.pattern.DeclarationNameSpace));

            foreach (var usingDirective in nsBase)
            {
                var selectorType = Type.GetType($"{usingDirective}.{selectorTypeName}");
                if (selectorType != null)
                {
                    return (ISelector)Activator.CreateInstance(selectorType);
                }
            }

            throw new ArgumentException($"Unknown selector {selectorTypeName}");
        }
    }
}
