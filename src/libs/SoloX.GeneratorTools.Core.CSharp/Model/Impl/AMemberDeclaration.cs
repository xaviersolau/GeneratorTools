﻿// ----------------------------------------------------------------------
// <copyright file="AMemberDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Base abstract member declaration implementation.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public class AMemberDeclaration<TNode> : IMemberDeclaration<TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMemberDeclaration{TNode}"/> class.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <param name="syntaxNodeProvider">The member syntax node provider.</param>
        protected AMemberDeclaration(string name, ISyntaxNodeProvider<TNode> syntaxNodeProvider)
        {
            this.Name = name;
            this.SyntaxNodeProvider = syntaxNodeProvider;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public ISyntaxNodeProvider<TNode> SyntaxNodeProvider { get; }
    }
}
