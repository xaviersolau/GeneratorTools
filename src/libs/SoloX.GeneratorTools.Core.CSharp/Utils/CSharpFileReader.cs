// ----------------------------------------------------------------------
// <copyright file="CSharpFileReader.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Utils
{
    /// <summary>
    /// CSharp file reader.
    /// </summary>
    public static class CSharpFileReader
    {
        /// <summary>
        /// Read the given file and apply the CSharp Syntax Walker on the resulting syntactic tree.
        /// </summary>
        /// <param name="file">The file to parse.</param>
        /// <param name="walker">The walker to apply.</param>
        public static void Read(string file, CSharpSyntaxWalker walker)
        {
            SyntaxTree syntaxTree;
            using (var srcFile = File.OpenRead(file))
            {
                var src = SourceText.From(srcFile);
                syntaxTree = CSharpSyntaxTree.ParseText(src);
            }

            walker.Visit(syntaxTree.GetRoot());
        }
    }
}
