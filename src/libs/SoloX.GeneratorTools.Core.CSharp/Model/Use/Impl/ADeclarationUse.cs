// ----------------------------------------------------------------------
// <copyright file="ADeclarationUse.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Declaration use base implementation.
    /// </summary>
    public abstract class ADeclarationUse : IDeclarationUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ADeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNode">The declaration use syntax node.</param>
        /// <param name="declaration">The declaration in use.</param>
        protected ADeclarationUse(CSharpSyntaxNode syntaxNode, IDeclaration declaration)
        {
            this.SyntaxNode = syntaxNode;
            this.Declaration = declaration;
        }

        /// <inheritdoc/>
        public CSharpSyntaxNode SyntaxNode { get; }

        /// <inheritdoc/>
        public IDeclaration Declaration { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"U {this.Declaration.Name}";
        }
    }
}
