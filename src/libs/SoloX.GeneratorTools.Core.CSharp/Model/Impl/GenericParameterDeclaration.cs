// ----------------------------------------------------------------------
// <copyright file="GenericParameterDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Generic parameter declaration.
    /// </summary>
    public class GenericParameterDeclaration : ADeclaration, IGenericParameterDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterDeclaration"/> class.
        /// </summary>
        /// <param name="name">The declaration name.</param>
        /// <param name="syntaxNode">The declaration syntax node.</param>
        public GenericParameterDeclaration(string name, TypeParameterSyntax syntaxNode)
            : base(string.Empty, name, syntaxNode, Array.Empty<string>(), null)
        {
        }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            // Nothing to load.
        }
    }
}
