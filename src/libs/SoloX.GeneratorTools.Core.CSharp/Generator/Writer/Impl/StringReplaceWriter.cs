// ----------------------------------------------------------------------
// <copyright file="StringReplaceWriter.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.Generator.Writer;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl
{
    /// <summary>
    /// String replace writer.
    /// </summary>
    public class StringReplaceWriter : INodeWriter
    {
        private string oldString;
        private string newString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReplaceWriter"/> class.
        /// </summary>
        /// <param name="oldString">The string to be replaced.</param>
        /// <param name="newString">The string to use instead.</param>
        public StringReplaceWriter(string oldString, string newString)
        {
            this.oldString = oldString;
            this.newString = newString;
        }

        /// <inheritdoc/>
        public bool Write(SyntaxNode node, Action<string> write)
        {
            var txt = node.ToFullString();

            if (txt.Contains(this.oldString))
            {
                write(txt.Replace(this.oldString, this.newString));
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Write(SyntaxToken token, Action<string> write)
        {
            var txt = token.ToFullString();

            if (txt.Contains(this.oldString))
            {
                write(txt.Replace(this.oldString, this.newString));
                return true;
            }

            return false;
        }
    }
}
