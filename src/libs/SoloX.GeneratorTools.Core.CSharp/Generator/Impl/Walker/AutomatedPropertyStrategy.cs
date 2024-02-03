// ----------------------------------------------------------------------
// <copyright file="AutomatedPropertyStrategy.cs" company="Xavier Solau">
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
    internal class AutomatedPropertyStrategy : IAutomatedStrategy
    {
        private readonly IPropertyDeclaration pattern;
        private readonly IPropertyDeclaration declaration;
        private readonly IAutomatedStrategy parentStrategy;
        private readonly TextPatternHelper textReplaceHelper;
        private readonly TextPatternHelper typeReplaceHelper;

        public AutomatedPropertyStrategy(
            IPropertyDeclaration pattern,
            IPropertyDeclaration declaration,
            string patternPrefix, string patternSuffix,
            IAutomatedStrategy parentStrategy)
        {
            this.pattern = pattern;
            this.declaration = declaration;
            this.parentStrategy = parentStrategy;
            var patternName = this.pattern.Name;
            var declarationName = this.declaration.Name;

            var patternTypeName = this.pattern.PropertyType.SyntaxNodeProvider.SyntaxNode.ToString();
            var declarationTypeName = this.declaration.PropertyType.SyntaxNodeProvider.SyntaxNode.ToString();

            this.textReplaceHelper = new TextPatternHelper(patternName, declarationName, patternPrefix, patternSuffix);
            this.typeReplaceHelper = new TextPatternHelper(patternTypeName, declarationTypeName, patternPrefix, patternSuffix);
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
            this.parentStrategy.RepeatDeclaration(repeatAttributeSyntax, callback);
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

        internal static bool Match(IPropertyDeclaration repeatProperty, string expression)
        {
            var firstLowerPatternName = char.ToLowerInvariant(repeatProperty.Name[0]) + repeatProperty.Name.Substring(1);
            return expression.Contains(repeatProperty.Name) || expression.Contains(firstLowerPatternName);
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
