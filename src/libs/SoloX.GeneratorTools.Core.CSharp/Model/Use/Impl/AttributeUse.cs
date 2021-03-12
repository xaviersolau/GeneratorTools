// ----------------------------------------------------------------------
// <copyright file="AttributeUse.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Attribute use model implementation.
    /// </summary>
    public class AttributeUse : IAttributeUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeUse"/> class.
        /// </summary>
        /// <param name="declaration">The attribute declaration.</param>
        /// <param name="syntaxNodeProvider">The attribute syntax node provider.</param>
        public AttributeUse(IDeclaration<SyntaxNode> declaration, ISyntaxNodeProvider<AttributeSyntax> syntaxNodeProvider)
        {
            if (declaration == null)
            {
                throw new ArgumentNullException(nameof(declaration), $"{nameof(declaration)} must not be null.");
            }

            this.Name = declaration.Name;
            this.Declaration = declaration;
            this.SyntaxNodeProvider = syntaxNodeProvider;
        }

        /// <inheritdoc/>
        public ISyntaxNodeProvider<AttributeSyntax> SyntaxNodeProvider { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public IDeclaration<SyntaxNode> Declaration { get; }
    }
}
