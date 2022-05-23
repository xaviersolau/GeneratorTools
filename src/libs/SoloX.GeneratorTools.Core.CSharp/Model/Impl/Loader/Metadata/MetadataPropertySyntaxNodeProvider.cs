// ----------------------------------------------------------------------
// <copyright file="MetadataPropertySyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataPropertySyntaxNodeProvider<TNode> : AMetadataSyntaxNodeProvider<TNode>
        where TNode : MemberDeclarationSyntax
    {
        protected override TNode Generate()
        {
            throw new NotImplementedException();
        }
    }
}
