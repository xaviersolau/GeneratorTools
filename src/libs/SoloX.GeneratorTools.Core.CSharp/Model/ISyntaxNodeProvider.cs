// ----------------------------------------------------------------------
// <copyright file="ISyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Syntax node provider interface.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public interface ISyntaxNodeProvider<out TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Gets the syntax node.
        /// </summary>
        TNode SyntaxNode { get; }
    }
}
