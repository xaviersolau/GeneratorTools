// ----------------------------------------------------------------------
// <copyright file="ReflectionIndexerSyntaxNodeProvider.cs" company="Xavier Solau">
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
    internal class ReflectionIndexerSyntaxNodeProvider : AReflectionSyntaxNodeProvider<IndexerDeclarationSyntax>
    {
        private readonly PropertyInfo property;
        private readonly ISyntaxNodeProvider<SyntaxNode> propertyTypeNodeProvider;

        public ReflectionIndexerSyntaxNodeProvider(
            PropertyInfo property,
            ISyntaxNodeProvider<SyntaxNode> propertyTypeNodeProvider)
        {
            this.property = property;
            this.propertyTypeNodeProvider = propertyTypeNodeProvider;
        }

        protected override IndexerDeclarationSyntax Generate()
        {
            var indexerParameters = this.property.GetIndexParameters();
            var node = GetSyntaxNode($"public {this.propertyTypeNodeProvider.SyntaxNode.ToString()} this[{string.Join(",", indexerParameters.Select(x => x.ParameterType.FullName + " " + x.Name))}] {{ get; set; }}");
            return (IndexerDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single();
        }
    }
}
