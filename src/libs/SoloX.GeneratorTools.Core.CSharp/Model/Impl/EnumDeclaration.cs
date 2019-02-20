// ----------------------------------------------------------------------
// <copyright file="EnumDeclaration.cs" company="SoloX Software">
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
    /// Enum declaration implementation.
    /// </summary>
    public class EnumDeclaration : ADeclaration, IEnumDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The enum declaration name space.</param>
        /// <param name="syntaxNode">The enum syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this enum.</param>
        public EnumDeclaration(string nameSpace, EnumDeclarationSyntax syntaxNode, IReadOnlyList<string> usingDirectives)
            : base(nameSpace, syntaxNode.Identifier.ToString(), syntaxNode, usingDirectives)
        {
            this.EnumDeclarationSyntax = syntaxNode;
        }

        /// <inheritdoc/>
        public EnumDeclarationSyntax EnumDeclarationSyntax { get; }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            // Nothing to load...
        }
    }
}
