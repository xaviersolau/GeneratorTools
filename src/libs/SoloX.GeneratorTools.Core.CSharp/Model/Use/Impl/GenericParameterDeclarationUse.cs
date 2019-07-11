// ----------------------------------------------------------------------
// <copyright file="GenericParameterDeclarationUse.cs" company="SoloX Software">
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

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Generic parameter declaration use.
    /// </summary>
    public class GenericParameterDeclarationUse : ADeclarationUse<SimpleNameSyntax>, IGenericParameterDeclarationUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNode">The declaration use syntax node.</param>
        /// <param name="declaration">The declaration in use.</param>
        public GenericParameterDeclarationUse(SimpleNameSyntax syntaxNode, IDeclaration<SyntaxNode> declaration)
            : base(syntaxNode, declaration)
        {
        }
    }
}
