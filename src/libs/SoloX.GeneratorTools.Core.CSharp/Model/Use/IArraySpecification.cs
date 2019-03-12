// ----------------------------------------------------------------------
// <copyright file="IArraySpecification.cs" company="SoloX Software">
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

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Array specification interface.
    /// </summary>
    public interface IArraySpecification
    {
        /// <summary>
        /// Gets the declaration syntax node.
        /// </summary>
        SyntaxList<ArrayRankSpecifierSyntax> SyntaxNode { get; }
    }
}
