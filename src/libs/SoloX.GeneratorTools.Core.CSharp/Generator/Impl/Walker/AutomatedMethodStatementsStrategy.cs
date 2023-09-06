// ----------------------------------------------------------------------
// <copyright file="AutomatedMethodStatementsStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    /// <summary>
    /// the statements 
    /// </summary>
    internal class AutomatedMethodStatementsStrategy : IAutomatedStrategy
    {
        private readonly IEnumerable<IPropertyDeclaration> propertyDeclarations;
        private readonly IPropertyDeclaration repeatProperty;
        private readonly IAutomatedStrategy parentStrategy;

        public AutomatedMethodStatementsStrategy(IEnumerable<IPropertyDeclaration> propertyDeclarations, IPropertyDeclaration repeatProperty, IAutomatedStrategy parentStrategy, bool isPackStatementEnabled)
        {
            this.propertyDeclarations = propertyDeclarations;
            this.repeatProperty = repeatProperty;
            this.parentStrategy = parentStrategy;

            IsPackStatementEnabled = isPackStatementEnabled;
        }

        public bool IsPackStatementEnabled { get; set; }

        public string ApplyPatternReplace(string text)
        {
            var generated = this.parentStrategy.ApplyPatternReplace(text);

            if (generated.Contains(this.repeatProperty.Name))
            {
                var stringBuilder = new StringBuilder();
                foreach (var item in this.propertyDeclarations)
                {
                    stringBuilder.Append(generated.Replace(this.repeatProperty.Name, item.Name));
                }
                return stringBuilder.ToString();
            }

            return generated;
        }

        public string ComputeTargetName()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentNameSpace()
        {
            throw new NotImplementedException();
        }

        public bool IgnoreUsingDirective(string ns)
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

        public void RepeatStatements(AttributeSyntax repeatStatementsAttributeSyntax, IAutomatedStrategy parentStrategy, Action<IAutomatedStrategy> callback)
        {
            throw new NotImplementedException();
        }

        public bool TryMatchRepeatDeclaration(AttributeSyntax repeatAttributeSyntax, SyntaxNode expression)
        {
            return false;
        }
    }
}
