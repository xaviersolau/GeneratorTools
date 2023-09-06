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
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedParameterStrategy : IAutomatedStrategy
    {
        private readonly IParameterDeclaration pattern;
        private readonly IParameterDeclaration declaration;
        private readonly IEnumerable<IReplacePatternHandler> replacePatternHandlers;
        private readonly TextPatternHelper textReplaceHelper;
        private readonly TextPatternHelper typeReplaceHelper;

        public AutomatedParameterStrategy(
            IParameterDeclaration pattern,
            IParameterDeclaration declaration,
            IEnumerable<IReplacePatternHandler> replacePatternHandlers,
            string patternPrefix, string patternSuffix)
        {
            this.pattern = pattern;
            this.declaration = declaration;
            this.replacePatternHandlers = replacePatternHandlers;

            var patternName = this.pattern.Name;
            var declarationName = this.declaration.Name;

            var patternTypeName = this.pattern.ParameterType.SyntaxNodeProvider.SyntaxNode.ToString();
            var declarationTypeName = this.declaration.ParameterType.SyntaxNodeProvider.SyntaxNode.ToString();

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

        public bool TryMatchRepeatDeclaration(AttributeSyntax repeatAttributeSyntax, SyntaxNode expression)
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

        internal static bool Match(IParameterDeclaration repeatParameter, string expression)
        {
            var firstUpperPatternName = char.ToUpperInvariant(repeatParameter.Name[0]) + repeatParameter.Name.Substring(1);
            return expression.Contains(repeatParameter.Name) || expression.Contains(firstUpperPatternName);
        }

        public void RepeatStatements(AttributeSyntax repeatStatementsAttributeSyntax, IAutomatedStrategy parentStrategy, Action<IAutomatedStrategy> callback)
        {
            throw new NotImplementedException();
        }
    }
}
