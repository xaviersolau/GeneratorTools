// ----------------------------------------------------------------------
// <copyright file="INamedMemberDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Named member declaration base interface.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public interface INamedMemberDeclaration<out TNode> : IMemberDeclaration<TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Gets the member name.
        /// </summary>
        string Name { get; }
    }
}
