// ----------------------------------------------------------------------
// <copyright file="MethodDeclaration.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Method declaration implementation.
    /// </summary>
    public class MethodDeclaration : AMemberDeclaration<MethodDeclarationSyntax>, IMethodDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDeclaration"/> class.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <param name="returnType">The return type use.</param>
        /// <param name="syntaxNodeProvider">The property syntax provider.</param>
        public MethodDeclaration(
            string name,
            IDeclarationUse<SyntaxNode> returnType,
            ISyntaxNodeProvider<MethodDeclarationSyntax> syntaxNodeProvider)
            : base(name, syntaxNodeProvider)
        {
            this.ReturnType = returnType;
        }

        /// <inheritdoc/>
        public IDeclarationUse<SyntaxNode> ReturnType { get; }
    }
}
