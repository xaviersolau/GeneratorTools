// ----------------------------------------------------------------------
// <copyright file="IAttributeUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Attribute use model.
    /// </summary>
    public interface IAttributeUse
    {
        /// <summary>
        /// Gets the declaration use syntax node provider.
        /// </summary>
        ISyntaxNodeProvider<AttributeSyntax> SyntaxNodeProvider { get; }

        /// <summary>
        /// Gets attribute name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets attribute declaration.
        /// </summary>
        IDeclarationUse<SyntaxNode> DeclarationUse { get; }

        /// <summary>
        /// Attribute named arguments.
        /// </summary>
        public IReadOnlyDictionary<string, object> NamedArguments { get; }

        /// <summary>
        /// Attribute constructor arguments.
        /// </summary>
        public IReadOnlyCollection<object> ConstructorArguments { get; }
    }
}
