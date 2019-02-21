// ----------------------------------------------------------------------
// <copyright file="UnknownDeclarationUse.cs" company="SoloX Software">
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
    public class UnknownDeclarationUse : ADeclarationUse, IUnknownDeclarationUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNode">The declaration use syntax node.</param>
        /// <param name="declaration">The declaration in use.</param>
        public UnknownDeclarationUse(CSharpSyntaxNode syntaxNode, IDeclaration declaration)
            : base(syntaxNode, declaration)
        {
        }
    }
}
