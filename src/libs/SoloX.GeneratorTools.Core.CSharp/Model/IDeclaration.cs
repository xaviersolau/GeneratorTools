// ----------------------------------------------------------------------
// <copyright file="IDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Base Declaration interface describing CSharp class, interface, enum...
    /// </summary>
    public interface IDeclaration
    {
        /// <summary>
        /// Gets the declaration name space.
        /// </summary>
        string DeclarationNameSpace { get; }

        /// <summary>
        /// Gets the declaration name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the declaration full name.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the declaration syntax node.
        /// </summary>
        CSharpSyntaxNode SyntaxNode { get; }

        /// <summary>
        /// Gets the using directives for the current declaration.
        /// </summary>
        IReadOnlyList<string> UsingDirectives { get; }
    }
}
