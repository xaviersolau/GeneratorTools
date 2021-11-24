// ----------------------------------------------------------------------
// <copyright file="IArraySpecification.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Array specification interface.
    /// </summary>
    public interface IArraySpecification
    {
        /// <summary>
        /// Gets the declaration syntax node provider.
        /// </summary>
        ISyntaxNodeProvider<ArrayTypeSyntax> SyntaxNodeProvider { get; }

        /// <summary>
        /// Gets the array count.
        /// </summary>
        int ArrayCount { get; }
    }
}
