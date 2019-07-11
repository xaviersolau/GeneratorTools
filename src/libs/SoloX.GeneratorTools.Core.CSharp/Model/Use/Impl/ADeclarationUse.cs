// ----------------------------------------------------------------------
// <copyright file="ADeclarationUse.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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
        /// <param name="syntaxNode">The declaration use syntax node.</param>
        /// <param name="declaration">The declaration in use.</param>
        protected ADeclarationUse(TNode syntaxNode, IDeclaration<SyntaxNode> declaration)
        {
            this.SyntaxNode = syntaxNode;
            this.Declaration = declaration;
        }

        /// <inheritdoc/>
        public TNode SyntaxNode { get; }

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
