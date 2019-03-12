// ----------------------------------------------------------------------
// <copyright file="ArraySpecification.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Array specification implementation.
    /// </summary>
    public class ArraySpecification : IArraySpecification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArraySpecification"/> class.
        /// </summary>
        /// <param name="syntaxNode">Array specification syntax node.</param>
        public ArraySpecification(SyntaxList<ArrayRankSpecifierSyntax> syntaxNode)
        {
            this.SyntaxNode = syntaxNode;
        }

        /// <inheritdoc/>
        public SyntaxList<ArrayRankSpecifierSyntax> SyntaxNode { get; }
    }
}
