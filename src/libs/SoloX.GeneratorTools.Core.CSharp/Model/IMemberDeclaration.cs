// ----------------------------------------------------------------------
// <copyright file="IMemberDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Member declaration base interface.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public interface IMemberDeclaration<out TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Gets the member name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the used declaration attributes.
        /// </summary>
        IReadOnlyList<IAttributeUse> Attributes { get; }

        /// <summary>
        /// Gets the member declaration syntax node provider.
        /// </summary>
        ISyntaxNodeProvider<TNode> SyntaxNodeProvider { get; }
    }
}
