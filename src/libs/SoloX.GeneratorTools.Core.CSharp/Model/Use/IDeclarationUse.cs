// ----------------------------------------------------------------------
// <copyright file="IDeclarationUse.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Declaration use interface.
    /// </summary>
    public interface IDeclarationUse
    {
        /// <summary>
        /// Gets the declaration use syntax node.
        /// </summary>
        CSharpSyntaxNode SyntaxNode { get; }

        /// <summary>
        /// Gets the declaration in use.
        /// </summary>
        IDeclaration Declaration { get; }
    }
}
