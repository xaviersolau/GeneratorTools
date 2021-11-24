// ----------------------------------------------------------------------
// <copyright file="IWriterSelector.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.Generator.Writer
{
    /// <summary>
    /// Writer selector interface.
    /// </summary>
    public interface IWriterSelector
    {
        /// <summary>
        /// Select and process the writer for the given node.
        /// </summary>
        /// <param name="node">The node to write.</param>
        /// <param name="write">The write delegate.</param>
        /// <returns>True if a writer actually wrote the code.</returns>
        bool SelectAndProcessWriter(SyntaxNode node, Action<string> write);

        /// <summary>
        /// Select and process the writer for the given token.
        /// </summary>
        /// <param name="token">The token to write.</param>
        /// <param name="write">The write delegate.</param>
        /// <returns>True if a writer actually wrote the code.</returns>
        bool SelectAndProcessWriter(SyntaxToken token, Action<string> write);
    }
}
