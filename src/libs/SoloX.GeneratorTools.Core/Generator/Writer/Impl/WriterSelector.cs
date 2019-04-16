// ----------------------------------------------------------------------
// <copyright file="WriterSelector.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.Generator.Writer.Impl
{
    /// <summary>
    /// Write selector.
    /// </summary>
    public class WriterSelector : IWriterSelector
    {
        private readonly IEnumerable<INodeWriter> writers;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriterSelector"/> class.
        /// </summary>
        /// <param name="writers">The node writers.</param>
        public WriterSelector(params INodeWriter[] writers)
        {
            this.writers = writers;
        }

        /// <inheritdoc/>
        public bool SelectAndProcessWriter(SyntaxNode node, Action<string> write)
        {
            return (from w in this.writers
                    let writen = w.Write(node, write)
                    where writen
                    select writen).FirstOrDefault();
        }

        /// <inheritdoc/>
        public bool SelectAndProcessWriter(SyntaxToken token, Action<string> write)
        {
            return (from w in this.writers
                    let writen = w.Write(token, write)
                    where writen
                    select writen).FirstOrDefault();
        }
    }
}
