// ----------------------------------------------------------------------
// <copyright file="INodeWriter.cs" company="Xavier Solau">
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
    /// Interface to write a property or other nodes.
    /// </summary>
    public interface INodeWriter
    {
        /// <summary>
        /// Write the code from the given node.
        /// </summary>
        /// <param name="node">The node to write.</param>
        /// <param name="write">The write delegate.</param>
        /// <returns>True if the node is written.</returns>
        bool Write(SyntaxNode node, Action<string> write);

        /// <summary>
        /// Write the code from the given token.
        /// </summary>
        /// <param name="token">The token to write.</param>
        /// <param name="write">The write delegate.</param>
        /// <returns>True if the node is written.</returns>
        bool Write(SyntaxToken token, Action<string> write);
    }
}
