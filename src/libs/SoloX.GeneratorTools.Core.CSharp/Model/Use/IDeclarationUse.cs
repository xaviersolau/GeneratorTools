// ----------------------------------------------------------------------
// <copyright file="IDeclarationUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Declaration use interface.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public interface IDeclarationUse<out TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Gets the declaration use syntax node provider.
        /// </summary>
        ISyntaxNodeProvider<TNode> SyntaxNodeProvider { get; }

        /// <summary>
        /// Gets the declaration in use.
        /// </summary>
        IDeclaration<SyntaxNode> Declaration { get; }

        /// <summary>
        /// Gets array specification.
        /// </summary>
        IArraySpecification ArraySpecification { get; }
    }
}
