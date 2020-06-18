// ----------------------------------------------------------------------
// <copyright file="IAttributeUse.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Attribute use model.
    /// </summary>
    public interface IAttributeUse
    {
        /// <summary>
        /// Gets the declaration use syntax node provider.
        /// </summary>
        ISyntaxNodeProvider<AttributeSyntax> SyntaxNodeProvider { get; }

        /// <summary>
        /// Gets attribute name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets attribute declaration.
        /// </summary>
        IDeclaration<SyntaxNode> Declaration { get; }
    }
}
