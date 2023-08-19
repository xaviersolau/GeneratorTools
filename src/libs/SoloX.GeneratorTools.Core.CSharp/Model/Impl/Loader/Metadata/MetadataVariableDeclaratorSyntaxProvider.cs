// ----------------------------------------------------------------------
// <copyright file="MetadataVariableDeclaratorSyntaxProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataVariableDeclaratorSyntaxProvider : AMetadataSyntaxNodeProvider<VariableDeclaratorSyntax>
    {
        private readonly string fieldName;
        private readonly object fieldValue;
        private readonly ISyntaxNodeProvider<SyntaxNode> fieldTypeNodeProvider;

        public MetadataVariableDeclaratorSyntaxProvider(string fieldName, object fieldValue, ISyntaxNodeProvider<SyntaxNode> fieldTypeNodeProvider)
        {
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
            this.fieldTypeNodeProvider = fieldTypeNodeProvider;
        }

        protected override VariableDeclaratorSyntax Generate()
        {
            var node = GetSyntaxNode($"public const {this.fieldTypeNodeProvider.SyntaxNode.ToString()} {this.fieldName} = {this.fieldValue};");
            return ((FieldDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single()).Declaration.Variables.Single();
        }
    }
}
