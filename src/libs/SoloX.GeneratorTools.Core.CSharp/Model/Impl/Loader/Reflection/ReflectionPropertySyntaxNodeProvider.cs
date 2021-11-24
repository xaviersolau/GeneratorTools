// ----------------------------------------------------------------------
// <copyright file="ReflectionPropertySyntaxNodeProvider.cs" company="Xavier Solau">
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
    internal class ReflectionPropertySyntaxNodeProvider : AReflectionSyntaxNodeProvider<PropertyDeclarationSyntax>
    {
        private readonly PropertyInfo property;
        private readonly ISyntaxNodeProvider<SyntaxNode> propertyTypeNodeProvider;

        public ReflectionPropertySyntaxNodeProvider(
            PropertyInfo property,
            ISyntaxNodeProvider<SyntaxNode> propertyTypeNodeProvider)
        {
            this.property = property;
            this.propertyTypeNodeProvider = propertyTypeNodeProvider;
        }

        protected override PropertyDeclarationSyntax Generate()
        {
            var node = GetSyntaxNode($"public {this.propertyTypeNodeProvider.SyntaxNode.ToString()} {this.property.Name} {{ get; set; }}");
            return (PropertyDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single();
        }
    }
}
