// ----------------------------------------------------------------------
// <copyright file="AutomatedConstantStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedConstantStrategy : IAutomatedStrategy
    {
        private readonly IConstantDeclaration pattern;
        private readonly IConstantDeclaration declaration;
        private readonly IEnumerable<IReplacePatternHandler> replacePatternHandlers;
        private readonly TextPatternHelper textReplaceHelper;
        private readonly TextPatternHelper typeReplaceHelper;

        public AutomatedConstantStrategy(
            IConstantDeclaration pattern,
            IConstantDeclaration declaration,
            IEnumerable<IReplacePatternHandler> replacePatternHandlers,
            string patternPrefix, string patternSuffix)
        {
            this.pattern = pattern;
            this.declaration = declaration;
            this.replacePatternHandlers = replacePatternHandlers;

            var patternName = this.pattern.Name;
            var declarationName = this.declaration.Name;

            var patternTypeName = this.pattern.ConstantType.SyntaxNodeProvider.SyntaxNode.ToString();
            var declarationTypeName = this.declaration.ConstantType.SyntaxNodeProvider.SyntaxNode.ToString();

            this.textReplaceHelper = new TextPatternHelper(patternName, declarationName, patternPrefix, patternSuffix);
            this.typeReplaceHelper = new TextPatternHelper(patternTypeName, declarationTypeName, patternPrefix, patternSuffix);
        }

        public bool IsPackStatementEnabled => false;

        public string ApplyPatternReplace(string text)
        {
            var result = this.textReplaceHelper.ReplacePattern(text);
            result = this.typeReplaceHelper.ReplacePattern(result);

            foreach (var replacePatternHandler in this.replacePatternHandlers)
            {
                result = replacePatternHandler.ApplyOn(result);
            }

            return result;
        }

        public string GetCurrentNameSpace()
        {
            throw new NotImplementedException();
        }

        public bool TryMatchRepeatDeclaration(AttributeSyntax repeatAttributeSyntax, string expression)
        {
            return false;
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

        //internal static bool Match(IPropertyDeclaration repeatProperty, string expression)
        //{
        //    var firstLowerPatternName = char.ToLowerInvariant(repeatProperty.Name[0]) + repeatProperty.Name.Substring(1);
        //    return expression.Contains(repeatProperty.Name) || expression.Contains(firstLowerPatternName);
        //}

        public void RepeatStatements(AttributeSyntax repeatStatementsAttributeSyntax, IAutomatedStrategy parentStrategy, Action<IAutomatedStrategy> callback)
        {
            throw new NotImplementedException();
        }
    }
}
