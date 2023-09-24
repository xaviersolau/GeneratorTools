// ----------------------------------------------------------------------
// <copyright file="AutomatedConstantStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedConstantStrategy : IAutomatedStrategy
    {
        private readonly IConstantDeclaration pattern;
        private readonly IConstantDeclaration declaration;
        private readonly TextPatternHelper textReplaceHelper;
        private readonly TextPatternHelper typeReplaceHelper;

        public AutomatedConstantStrategy(
            IConstantDeclaration pattern,
            IConstantDeclaration declaration,
            string patternPrefix, string patternSuffix)
        {
            this.pattern = pattern;
            this.declaration = declaration;

            var patternName = this.pattern.Name;
            var declarationName = this.declaration.Name;

            var patternTypeName = this.pattern.ConstantType.SyntaxNodeProvider.SyntaxNode.ToString();
            var declarationTypeName = this.declaration.ConstantType.SyntaxNodeProvider.SyntaxNode.ToString();

            this.textReplaceHelper = new TextPatternHelper(patternName, declarationName, patternPrefix, patternSuffix);
            this.typeReplaceHelper = new TextPatternHelper(patternTypeName, declarationTypeName, patternPrefix, patternSuffix);
        }

        public IReplacePatternHandler CreateReplacePatternHandler()
        {
            return new StrategyReplacePatternHandler(ApplyPatternReplace);
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
            return this.declaration.Name;
        }

        public bool TryMatchAndRepeatStatement(
            SyntaxNode? patternNameExpression,
            SyntaxNode? patternPrefixExpression,
            SyntaxNode? patternSuffixExpression,
            Action<IAutomatedStrategy> callback)
        {
            return false;
        }
    }
}
