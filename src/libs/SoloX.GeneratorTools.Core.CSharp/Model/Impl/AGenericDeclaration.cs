// ----------------------------------------------------------------------
// <copyright file="AGenericDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Base abstract generic declaration implementation.
    /// </summary>
    public class AGenericDeclaration : ADeclaration, IGenericDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AGenericDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The class declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <param name="syntaxNode">The declaration syntax node.</param>
        /// <param name="typeParameterListSyntax">The type parameter list syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this class.</param>
        public AGenericDeclaration(string nameSpace, string name, CSharpSyntaxNode syntaxNode, TypeParameterListSyntax typeParameterListSyntax, IReadOnlyList<string> usingDirectives)
            : base(nameSpace, name, syntaxNode, usingDirectives)
        {
            this.TypeParameterListSyntax = typeParameterListSyntax;
        }

        /// <inheritdoc/>
        public TypeParameterListSyntax TypeParameterListSyntax { get; }
    }
}
