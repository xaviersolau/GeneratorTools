// ----------------------------------------------------------------------
// <copyright file="AutomatedPropertyStrategy.cs" company="Xavier Solau">
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
    internal class AutomatedPropertyStrategy : IAutomatedStrategy
    {
        private readonly IPropertyDeclaration pattern;
        private readonly IPropertyDeclaration declaration;
        private readonly IEnumerable<IReplacePatternHandler> replacePatternHandlers;

        public AutomatedPropertyStrategy(
            IPropertyDeclaration pattern,
            IPropertyDeclaration declaration,
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

            var patternType = this.pattern.PropertyType.SyntaxNodeProvider.SyntaxNode.ToString();

            var declarationType = this.declaration.PropertyType.SyntaxNodeProvider.SyntaxNode.ToString();

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

        internal static bool Match(IPropertyDeclaration repeatProperty, string expression)
        {
            var firstLowerPatternName = char.ToLowerInvariant(repeatProperty.Name[0]) + repeatProperty.Name.Substring(1);
            return expression.Contains(repeatProperty.Name) || expression.Contains(firstLowerPatternName);
        }
    }
}
