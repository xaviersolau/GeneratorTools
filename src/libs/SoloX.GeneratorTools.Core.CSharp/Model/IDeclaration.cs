// ----------------------------------------------------------------------
// <copyright file="IDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Base Declaration interface describing CSharp class, interface, enum...
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public interface IDeclaration<out TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Gets the declaration name space.
        /// </summary>
        string DeclarationNameSpace { get; }

        /// <summary>
        /// Gets the declaration name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the declaration full name.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the declaration syntax node provider.
        /// </summary>
        ISyntaxNodeProvider<TNode> SyntaxNodeProvider { get; }

        /// <summary>
        /// Gets the using directives for the current declaration.
        /// </summary>
        IReadOnlyList<string> UsingDirectives { get; }

        /// <summary>
        /// Gets the used declaration attributes.
        /// </summary>
        IReadOnlyList<IAttributeUse> Attributes { get; }

        /// <summary>
        /// Gets the location on the file system.
        /// </summary>
        string Location { get; }
    }
}
