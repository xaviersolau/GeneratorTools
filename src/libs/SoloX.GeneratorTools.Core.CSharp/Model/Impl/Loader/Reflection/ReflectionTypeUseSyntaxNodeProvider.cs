// ----------------------------------------------------------------------
// <copyright file="ReflectionTypeUseSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionTypeUseSyntaxNodeProvider<TNode> : AReflectionSyntaxNodeProvider<TNode>
        where TNode : NameSyntax
    {
        private readonly Type typeUsed;

        public ReflectionTypeUseSyntaxNodeProvider(Type typeUsed)
        {
            this.typeUsed = typeUsed;
        }

        protected override TNode Generate()
        {
            var node = GetSyntaxNode($"{this.typeUsed.FullName} x;");
            var statement = (LocalDeclarationStatementSyntax)(((GlobalStatementSyntax)((CompilationUnitSyntax)node).Members.Single()).Statement);
            return (TNode)statement.Declaration.Type;
        }
    }
}
