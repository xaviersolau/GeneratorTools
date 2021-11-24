﻿// ----------------------------------------------------------------------
// <copyright file="ADeclarationUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Declaration use base implementation.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public abstract class ADeclarationUse<TNode> : IDeclarationUse<TNode>, IArrayDeclarationUseImpl
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ADeclarationUse{TNode}"/> class.
        /// </summary>
        /// <param name="syntaxNodeProvider">The declaration use syntax node provider.</param>
        /// <param name="declaration">The declaration in use.</param>
        protected ADeclarationUse(ISyntaxNodeProvider<TNode> syntaxNodeProvider, IDeclaration<SyntaxNode> declaration)
        {
            this.SyntaxNodeProvider = syntaxNodeProvider;
            this.Declaration = declaration;
        }

        /// <inheritdoc/>
        public ISyntaxNodeProvider<TNode> SyntaxNodeProvider { get; }

        /// <inheritdoc/>
        public IDeclaration<SyntaxNode> Declaration { get; }

        /// <inheritdoc/>
        public IArraySpecification ArraySpecification { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"U {this.Declaration.Name}";
        }
    }
}
