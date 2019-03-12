// ----------------------------------------------------------------------
// <copyright file="PredefinedDeclarationUse.cs" company="SoloX Software">
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
    /// Predefined declaration use definition.
    /// </summary>
    public class PredefinedDeclarationUse : IPredefinedDeclarationUse, IPredefinedDeclaration, IArrayDeclarationUseImpl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNode">The predefined syntax node.</param>
        /// <param name="name">Predefined type name.</param>
        public PredefinedDeclarationUse(CSharpSyntaxNode syntaxNode, string name)
        {
            this.SyntaxNode = syntaxNode;
            this.Name = name;
        }

        /// <inheritdoc/>
        public CSharpSyntaxNode SyntaxNode { get; }

        /// <inheritdoc/>
        public string DeclarationNameSpace => string.Empty;

        /// <inheritdoc/>
        public IDeclaration Declaration => this;

        /// <inheritdoc/>
        public IArraySpecification ArraySpecification { get; set; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string FullName => this.Name;

        /// <inheritdoc/>
        public IReadOnlyList<string> UsingDirectives => Array.Empty<string>();

        /// <inheritdoc/>
        public string Location { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"U {this.Name}";
        }
    }
}
