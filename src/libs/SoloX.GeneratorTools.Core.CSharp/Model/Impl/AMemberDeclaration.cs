// ----------------------------------------------------------------------
// <copyright file="AMemberDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Base abstract member declaration implementation.
    /// </summary>
    public class AMemberDeclaration : IMemberDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMemberDeclaration"/> class.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <param name="syntaxNode">The member syntax.</param>
        protected AMemberDeclaration(string name, CSharpSyntaxNode syntaxNode)
        {
            this.SyntaxNode = syntaxNode;
            this.Name = name;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public CSharpSyntaxNode SyntaxNode { get; }
    }
}
