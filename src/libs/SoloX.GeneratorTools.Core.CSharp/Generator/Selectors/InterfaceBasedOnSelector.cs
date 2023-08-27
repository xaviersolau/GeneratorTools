// ----------------------------------------------------------------------
// <copyright file="InterfaceBasedOnSelector.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Selectors
{
    /// <summary>
    /// Pattern target selector by interfaces implementing a base interface.
    /// </summary>
    /// <typeparam name="TInterface">Base interface.</typeparam>
    public class InterfaceBasedOnSelector<TInterface> : ISelector
        where TInterface : class
    {
        /// <inheritdoc/>
        public IEnumerable<IDeclaration<SyntaxNode>> GetDeclarations(IEnumerable<ICSharpFile> files)
        {
            return files
                .SelectMany(file => file.Declarations.Where(d => d is IInterfaceDeclaration).Cast<IInterfaceDeclaration>())
                .Where(d => d.Extends.Any(a => a.Declaration.FullName == typeof(TInterface).FullName));
        }

        /// <inheritdoc/>
        public IEnumerable<IMethodDeclaration> GetMethods(IGenericDeclaration<SyntaxNode> declaration)
        {
            return Array.Empty<IMethodDeclaration>();
        }

        /// <inheritdoc/>
        public IEnumerable<IPropertyDeclaration> GetProperties(IGenericDeclaration<SyntaxNode> declaration)
        {
            return Array.Empty<IPropertyDeclaration>();
        }

        /// <inheritdoc/>
        public IEnumerable<IConstantDeclaration> GetConstants(IGenericDeclaration<SyntaxNode> declaration)
        {
            return Array.Empty<IConstantDeclaration>();
        }
    }
}
