// ----------------------------------------------------------------------
// <copyright file="ParserSyntaxNodeProvider.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

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
