// ----------------------------------------------------------------------
// <copyright file="MetadataTypeSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataTypeSyntaxNodeProvider<TNode> : AMetadataSyntaxNodeProvider<TNode>
        where TNode : MemberDeclarationSyntax
    {
        private readonly string classSemantic;
        private readonly string name;

        public MetadataTypeSyntaxNodeProvider(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle)
        {
            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);
            var attributes = typeDefinition.Attributes;
            var classSemantics = attributes & TypeAttributes.ClassSemanticsMask;
            this.name = MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(metadataReader, typeDefinitionHandle);

            this.classSemantic = "class";
            if (classSemantics == TypeAttributes.Interface)
            {
                this.classSemantic = "interface";
            }
            //else if (this.declarationType.IsEnum)
            //{
            //    type = "enum";
            //}
            //else if (this.declarationType.IsValueType)
            //{
            //    type = "struct";
            //}
        }

        protected override TNode Generate()
        {
            var node = GetSyntaxNode($"public {this.classSemantic} {this.name} {{}}");
            return (TNode)((CompilationUnitSyntax)node).Members.Single();
        }
    }
}
