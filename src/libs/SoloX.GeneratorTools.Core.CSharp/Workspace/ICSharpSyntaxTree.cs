// ----------------------------------------------------------------------
// <copyright file="ICSharpSyntaxTree.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// Interface describing a CSharp SyntaxTree.
    /// </summary>
    public interface ICSharpSyntaxTree : ICSharpFile
    {
        /// <summary>
        /// Gets the CSharp SyntaxTree.
        /// </summary>
        SyntaxTree SyntaxTree { get; }
    }
}
