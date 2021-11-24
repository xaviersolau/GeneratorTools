// ----------------------------------------------------------------------
// <copyright file="AutomatedParameterStrategy.cs" company="Xavier Solau">
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
    internal class AutomatedParameterStrategy : IAutomatedStrategy
    {
        private readonly IParameterDeclaration pattern;
        private readonly IParameterDeclaration declaration;
        private readonly IEnumerable<IReplacePatternHandler> replacePatternHandlers;

        public AutomatedParameterStrategy(
            IParameterDeclaration pattern,
            IParameterDeclaration declaration,
            IEnumerable<IReplacePatternHandler> replacePatternHandlers)
        {
            this.pattern = pattern;
            this.declaration = declaration;
            this.replacePatternHandlers = replacePatternHandlers;
        }

        public string ApplyPatternReplace(string text)
        {
            var firstLowerPatternName = char.ToLowerInvariant(this.pattern.Name[0]) + this.pattern.Name.Substring(1);
            var firstLowerDeclarationName = char.ToLowerInvariant(this.declaration.Name[0]) + this.declaration.Name.Substring(1);

            var patternType = this.pattern.ParameterType.SyntaxNodeProvider.SyntaxNode.ToString();

            var declarationType = this.declaration.ParameterType.SyntaxNodeProvider.SyntaxNode.ToString();

            var result = text
                .Replace(patternType, declarationType)
                .Replace(firstLowerPatternName, firstLowerDeclarationName)
                .Replace(this.pattern.Name, this.declaration.Name);

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

        internal static bool Match(IParameterDeclaration repeatParameter, string expression)
        {
            var firstUpperPatternName = char.ToUpperInvariant(repeatParameter.Name[0]) + repeatParameter.Name.Substring(1);
            return expression.Contains(repeatParameter.Name) || expression.Contains(firstUpperPatternName);
        }
    }
}
