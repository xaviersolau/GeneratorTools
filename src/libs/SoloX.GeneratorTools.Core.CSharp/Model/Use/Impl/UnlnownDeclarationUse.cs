// ----------------------------------------------------------------------
// <copyright file="UnlnownDeclarationUse.cs" company="SoloX Software">
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
    /// Unknown declaration use implementation.
    /// </summary>
    public class UnlnownDeclarationUse : ADeclarationUse, IUnknownDeclarationUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnlnownDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNode">The declaration use syntax node.</param>
        /// <param name="declaration">The declaration in use.</param>
        public UnlnownDeclarationUse(CSharpSyntaxNode syntaxNode, IDeclaration declaration)
            : base(syntaxNode, declaration)
        {
        }
    }
}
