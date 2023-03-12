// ----------------------------------------------------------------------
// <copyright file="MetadataTypeSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
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

            var rawName = MetadataGenericDeclarationLoader<SyntaxNode>.LoadString(metadataReader, typeDefinition.Name);

            this.name = MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(rawName);

            var type = string.Empty;
            if (typeof(TNode) == typeof(InterfaceDeclarationSyntax))
            {
                type = "interface";
            }
            else if (typeof(TNode) == typeof(EnumDeclarationSyntax))
            {
                type = "enum";
            }
            else if (typeof(TNode) == typeof(StructDeclarationSyntax))
            {
                type = "struct";
            }
            else if (typeof(TNode) == typeof(ClassDeclarationSyntax))
            {
                type = "class";
            }
            else if (typeof(TNode) == typeof(RecordDeclarationSyntax))
            {
                if (MetadataGenericDeclarationLoader<RecordDeclarationSyntax>.ProbeRecordStructType(metadataReader, typeDefinition))
                {
                    type = "record struct";
                }
                else
                {
                    type = "record";
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            this.classSemantic = type;
        }

        protected override TNode Generate()
        {
            var node = GetSyntaxNode($"public {this.classSemantic} {this.name} {{}}");
            return (TNode)((CompilationUnitSyntax)node).Members.Single();
        }
    }
}
