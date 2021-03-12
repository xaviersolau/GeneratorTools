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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.Generator.Writer;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl
{
    /// <summary>
    /// String replace writer.
    /// </summary>
    public class StringReplaceWriter : INodeWriter
    {
        private readonly Func<SyntaxNode, bool> nodeFilter;
        private string oldString;
        private IReadOnlyList<string> newStringList;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReplaceWriter"/> class.
        /// </summary>
        /// <param name="oldString">The string to be replaced.</param>
        /// <param name="newString">The string to use instead.</param>
        /// <param name="nodeFilter">The node filter to use to prevent direct substitution.</param>
        public StringReplaceWriter(string oldString, string newString, Func<SyntaxNode, bool> nodeFilter = null)
        {
            this.oldString = oldString;
            this.newStringList = new[] { newString };
            this.nodeFilter = nodeFilter ?? DefaultNodeFilter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReplaceWriter"/> class.
        /// </summary>
        /// <remarks>The pattern string substitution will be repeated for every new string in the newStringList.</remarks>
        /// <param name="oldString">The string to be replaced.</param>
        /// <param name="newStringList">The string list to use instead.</param>
        /// <param name="nodeFilter">The node filter to use to prevent direct substitution (return true to prevent substitution).</param>
        public StringReplaceWriter(string oldString, IReadOnlyList<string> newStringList, Func<SyntaxNode, bool> nodeFilter = null)
        {
            this.oldString = oldString;
            this.newStringList = newStringList;
            this.nodeFilter = nodeFilter ?? DefaultNodeFilter;
        }

        /// <inheritdoc/>
        public bool Write(SyntaxNode node, Action<string> write)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node), $"The argument {nameof(node)} was null.");
            }

            if (this.nodeFilter(node))
            {
                return false;
            }

            var txt = node.ToFullString();

            if (txt.Contains(this.oldString))
            {
                foreach (var newString in this.newStringList)
                {
                    write(txt.Replace(this.oldString, newString));
                }

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
                foreach (var newString in this.newStringList)
                {
                    write(txt.Replace(this.oldString, newString));
                }

                return true;
            }

            return false;
        }

        private static bool DefaultNodeFilter(SyntaxNode node)
        {
            return node is MethodDeclarationSyntax
                || node is ClassDeclarationSyntax
                || node is InterfaceDeclarationSyntax
                || node is StructDeclarationSyntax
                || node is EnumDeclarationSyntax;
        }
    }
}
