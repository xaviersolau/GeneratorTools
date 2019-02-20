// ----------------------------------------------------------------------
// <copyright file="ClassDeclaration.cs" company="SoloX Software">
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
    /// Class declaration implementation.
    /// </summary>
    public class ClassDeclaration : AGenericDeclaration, IClassDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The class declaration name space.</param>
        /// <param name="syntaxNode">The class syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this class.</param>
        public ClassDeclaration(string nameSpace, ClassDeclarationSyntax syntaxNode, IReadOnlyList<string> usingDirectives)
            : base(nameSpace, syntaxNode.Identifier.ToString(), syntaxNode, syntaxNode.TypeParameterList, usingDirectives)
        {
            this.ClassDeclarationSyntax = syntaxNode;
        }

        /// <inheritdoc/>
        public ClassDeclarationSyntax ClassDeclarationSyntax { get; }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            this.LoadGenericParameters();
            this.LoadExtends(resolver, this.ClassDeclarationSyntax.BaseList);
        }
    }
}
