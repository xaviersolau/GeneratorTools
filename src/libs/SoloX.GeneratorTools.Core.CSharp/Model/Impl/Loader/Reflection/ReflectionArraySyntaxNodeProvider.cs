// ----------------------------------------------------------------------
// <copyright file="ReflectionArraySyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionArraySyntaxNodeProvider : AReflectionSyntaxNodeProvider<ArrayTypeSyntax>
    {
        private readonly int arrayCount;
        private readonly ISyntaxNodeProvider<SyntaxNode> typeSyntaxNodeProvider;

        public ReflectionArraySyntaxNodeProvider(
            int arrayCount,
            ISyntaxNodeProvider<SyntaxNode> typeSyntaxNodeProvider)
        {
            this.arrayCount = arrayCount;
            this.typeSyntaxNodeProvider = typeSyntaxNodeProvider;
        }

        protected override ArrayTypeSyntax Generate()
        {
            var array = string.Empty;

            for (var i = 0; i < this.arrayCount; i++)
            {
                array += "[]";
            }

            var type = this.typeSyntaxNodeProvider.SyntaxNode.ToString();

            var node = GetSyntaxNode($"{type}{array} x;");
            var statement = (LocalDeclarationStatementSyntax)(((GlobalStatementSyntax)((CompilationUnitSyntax)node).Members.Single()).Statement);
            return (ArrayTypeSyntax)statement.Declaration.Type;
        }
    }
}
