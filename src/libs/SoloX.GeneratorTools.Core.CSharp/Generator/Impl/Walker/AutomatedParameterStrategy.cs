// ----------------------------------------------------------------------
// <copyright file="AutomatedParameterStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedParameterStrategy : IAutomatedStrategy
    {
        private readonly IParameterDeclaration pattern;
        private readonly IReadOnlyCollection<IParameterDeclaration> declarations;
        private readonly IDeclarationResolver resolver;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;
        private readonly string patternPrefix;
        private readonly string patternSuffix;
        private readonly string patternName;
        private readonly string patternTypeName;

        private TextPatternHelper textReplaceHelper;
        private TextPatternHelper typeReplaceHelper;

        public AutomatedParameterStrategy(
            IParameterDeclaration pattern,
            IReadOnlyCollection<IParameterDeclaration> declarations,
            IDeclarationResolver resolver,
            IGenericDeclaration<SyntaxNode> genericDeclaration,
            string patternPrefix, string patternSuffix)
        {
            this.pattern = pattern;
            this.declarations = declarations;
            this.resolver = resolver;
            this.genericDeclaration = genericDeclaration;
            this.patternPrefix = patternPrefix;
            this.patternSuffix = patternSuffix;
            this.patternName = this.pattern.Name;
            this.patternTypeName = this.pattern.ParameterType.SyntaxNodeProvider.SyntaxNode.ToString();

        }

        public IReplacePatternHandler CreateReplacePatternHandler()
        {
            return new StrategyReplacePatternHandler(ApplyPatternReplace);
        }

        public IReplacePatternHandler CreateReplacePatternHandler(AttributeSyntax replacePatternAttributeSyntax)
        {
            throw new NotImplementedException();
        }

        private string ApplyPatternReplace(string text)
        {
            var result = this.textReplaceHelper.ReplacePattern(text);
            result = this.typeReplaceHelper.ReplacePattern(result);

            return result;
        }

        public string GetCurrentNameSpace()
        {
            throw new NotImplementedException();
        }

        public void RepeatDeclaration(AttributeSyntax repeatAttributeSyntax, Action<IAutomatedStrategy> callback)
        {
            throw new NotImplementedException();
        }

        public void RepeatNameSpace(Action<string> nsCallback)
        {
            throw new NotImplementedException();
        }

        public bool IgnoreUsingDirective(string ns)
        {
            throw new NotImplementedException();
        }

        public string ComputeTargetName()
        {
            throw new NotImplementedException();
        }

        internal static bool Match(IParameterDeclaration repeatParameter, string expression)
        {
            var firstUpperPatternName = char.ToUpperInvariant(repeatParameter.Name[0]) + repeatParameter.Name.Substring(1);
            return expression.Contains(repeatParameter.Name) || expression.Contains(firstUpperPatternName);
        }

        public bool TryMatchAndRepeatStatement(
            SyntaxNode? patternNameExpression,
            SyntaxNode? patternPrefixExpression,
            SyntaxNode? patternSuffixExpression,
            Action<IAutomatedStrategy> callback)
        {
            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>(this.resolver, this.genericDeclaration);
            var pName = constEvaluator.Visit(patternNameExpression);

            if (pName != null && pName == this.pattern.Name)
            {
                foreach (var declaration in this.declarations)
                {
                    SetCurrent(declaration);
                    callback(this);
                }
                return true;
            }

            return false;
        }

        private void SetCurrent(IParameterDeclaration declaration)
        {
            var declarationName = declaration.Name;
            var declarationTypeName = declaration.ParameterType.SyntaxNodeProvider.SyntaxNode.ToString();

            this.textReplaceHelper = new TextPatternHelper(this.patternName, declarationName, this.patternPrefix, this.patternSuffix);
            this.typeReplaceHelper = new TextPatternHelper(this.patternTypeName, declarationTypeName, this.patternPrefix, this.patternSuffix);
        }
    }
}
