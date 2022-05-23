// ----------------------------------------------------------------------
// <copyright file="MetadataTypeUseSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataTypeUseSyntaxNodeProvider<TNode> : AMetadataSyntaxNodeProvider<TNode>
        where TNode : NameSyntax
    {
        private readonly string name;
        private readonly IEnumerable<IDeclarationUse<SyntaxNode>> typeArguments;

        public MetadataTypeUseSyntaxNodeProvider(string name, IEnumerable<IDeclarationUse<SyntaxNode>> typeArguments = null)
        {
            this.name = name;
            this.typeArguments = typeArguments;
        }

        protected override TNode Generate()
        {
            if (this.typeArguments != null)
            {
                throw new NotImplementedException();
            }

            var node = GetSyntaxNode($"{this.name} x;");
            var statement = (LocalDeclarationStatementSyntax)(((GlobalStatementSyntax)((CompilationUnitSyntax)node).Members.Single()).Statement);
            return (TNode)statement.Declaration.Type;
        }
    }
}
