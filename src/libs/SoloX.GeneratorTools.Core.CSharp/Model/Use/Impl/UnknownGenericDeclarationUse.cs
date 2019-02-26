// ----------------------------------------------------------------------
// <copyright file="UnknownGenericDeclarationUse.cs" company="SoloX Software">
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
    /// Unknown generic declaration use implementation.
    /// </summary>
    public class UnknownGenericDeclarationUse : GenericDeclarationUse, IUnknownGenericDeclarationUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownGenericDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNode">The declaration use syntax node.</param>
        /// <param name="declaration">The declaration in use.</param>
        /// <param name="genericParameters">The generic parameters.</param>
        public UnknownGenericDeclarationUse(
            CSharpSyntaxNode syntaxNode, IDeclaration declaration, IReadOnlyCollection<IDeclarationUse> genericParameters)
            : base(syntaxNode, declaration, genericParameters)
        {
        }
    }
}
