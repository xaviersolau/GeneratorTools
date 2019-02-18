// ----------------------------------------------------------------------
// <copyright file="StructDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Struct declaration implementation.
    /// </summary>
    public class StructDeclaration : AGenericDeclaration, IStructDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The struct declaration name space.</param>
        /// <param name="syntaxNode">The struct syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this struct.</param>
        public StructDeclaration(string nameSpace, StructDeclarationSyntax syntaxNode, IReadOnlyList<string> usingDirectives)
            : base(nameSpace, syntaxNode.Identifier.ToString(), syntaxNode, syntaxNode.TypeParameterList, usingDirectives)
        {
            this.StructDeclarationSyntax = syntaxNode;
        }

        /// <inheritdoc/>
        public StructDeclarationSyntax StructDeclarationSyntax { get; }
    }
}
