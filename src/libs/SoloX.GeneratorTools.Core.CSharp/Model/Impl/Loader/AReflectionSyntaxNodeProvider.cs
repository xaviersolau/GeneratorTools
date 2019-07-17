// ----------------------------------------------------------------------
// <copyright file="AReflectionSyntaxNodeProvider.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader
{
    internal abstract class AReflectionSyntaxNodeProvider<TNode> : ISyntaxNodeProvider<TNode>
        where TNode : SyntaxNode
    {
        /// <inheritdoc/>
        public TNode SyntaxNode => this.Generate();

        /// <summary>
        /// Parse the given input.
        /// </summary>
        /// <param name="text">The input text to parse.</param>
        /// <returns>The CompilationUnitSyntax node.</returns>
        internal static SyntaxNode GetSyntaxNode(string text)
        {
            var src = SourceText.From(text);
            var syntaxTree = CSharpSyntaxTree.ParseText(src);

            return syntaxTree.GetRoot();
        }

        protected abstract TNode Generate();
    }
}
