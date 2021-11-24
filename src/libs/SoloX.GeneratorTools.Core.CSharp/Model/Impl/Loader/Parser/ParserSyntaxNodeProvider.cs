// ----------------------------------------------------------------------
// <copyright file="ParserSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser
{
    internal class ParserSyntaxNodeProvider<TNode> : ISyntaxNodeProvider<TNode>
        where TNode : SyntaxNode
    {
        public ParserSyntaxNodeProvider(TNode syntaxNode)
        {
            this.SyntaxNode = syntaxNode;
        }

        public TNode SyntaxNode { get; private set; }
    }
}
