// ----------------------------------------------------------------------
// <copyright file="AMetadataSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal abstract class AMetadataSyntaxNodeProvider<TNode> : ISyntaxNodeProvider<TNode>
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
