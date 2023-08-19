// ----------------------------------------------------------------------
// <copyright file="ReflectionVariableDeclaratorSyntaxProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionVariableDeclaratorSyntaxProvider : AReflectionSyntaxNodeProvider<VariableDeclaratorSyntax>
    {
        private readonly FieldInfo field;
        private readonly ISyntaxNodeProvider<SyntaxNode> fieldTypeNodeProvider;

        public ReflectionVariableDeclaratorSyntaxProvider(
            FieldInfo field,
            ISyntaxNodeProvider<SyntaxNode> fieldTypeNodeProvider)
        {
            this.field = field;
            this.fieldTypeNodeProvider = fieldTypeNodeProvider;
        }

        protected override VariableDeclaratorSyntax Generate()
        {
            var node = GetSyntaxNode($"public const {this.fieldTypeNodeProvider.SyntaxNode.ToString()} {this.field.Name} = {this.field.GetValue(null)};");
            return ((FieldDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single()).Declaration.Variables.Single();
        }
    }
}
