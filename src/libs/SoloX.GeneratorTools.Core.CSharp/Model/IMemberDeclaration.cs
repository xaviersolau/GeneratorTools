// ----------------------------------------------------------------------
// <copyright file="IMemberDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Member declaration base interface.
    /// </summary>
    public interface IMemberDeclaration
    {
        /// <summary>
        /// Gets the member name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the member declaration syntax node.
        /// </summary>
        CSharpSyntaxNode SyntaxNode { get; }
    }
}
