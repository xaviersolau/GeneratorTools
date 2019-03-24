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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl
{
    /// <summary>
    /// Write selector.
    /// </summary>
    public class WriterSelector : IWriterSelector
    {
        private readonly IEnumerable<NodeWriter> propertyWriters;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriterSelector"/> class.
        /// </summary>
        /// <param name="propertyWriters">The property writers.</param>
        public WriterSelector(params NodeWriter[] propertyWriters)
        {
            this.propertyWriters = propertyWriters;
        }

        /// <inheritdoc/>
        public bool SelectAndProcessWriter(CSharpSyntaxNode node, Action<string> write)
        {
            return this.propertyWriters.Select(w => w.Write(node, write)).FirstOrDefault();
        }
    }
}
