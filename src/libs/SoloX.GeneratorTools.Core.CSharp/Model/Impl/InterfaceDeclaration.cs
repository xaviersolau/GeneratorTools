// ----------------------------------------------------------------------
// <copyright file="InterfaceDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Interface declaration implementation.
    /// </summary>
    public class InterfaceDeclaration : AGenericDeclaration, IInterfaceDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The interface declaration name space.</param>
        /// <param name="syntaxNode">The interface syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this interface.</param>
        public InterfaceDeclaration(string nameSpace, InterfaceDeclarationSyntax syntaxNode, IReadOnlyList<string> usingDirectives)
            : base(nameSpace, syntaxNode.Identifier.ToString(), syntaxNode, syntaxNode.TypeParameterList, usingDirectives)
        {
            this.InterfaceDeclarationSyntax = syntaxNode;
        }

        /// <inheritdoc/>
        public InterfaceDeclarationSyntax InterfaceDeclarationSyntax { get; }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            this.LoadGenericParameters();
            this.LoadExtends(resolver, this.InterfaceDeclarationSyntax.BaseList);
        }
    }
}
