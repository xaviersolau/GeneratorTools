// ----------------------------------------------------------------------
// <copyright file="MetadataDeclarationFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    /// <summary>
    /// Declaration factory.
    /// </summary>
    internal class MetadataDeclarationFactory : IMetadataDeclarationFactory
    {
        private readonly MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax> metadataLoaderInterfaceDeclarationSyntax;
        private readonly MetadataGenericDeclarationLoader<ClassDeclarationSyntax> metadataLoaderClassDeclarationSyntax;
        private readonly MetadataGenericDeclarationLoader<StructDeclarationSyntax> metadataLoaderStructDeclarationSyntax;
        private readonly MetadataGenericDeclarationLoader<RecordDeclarationSyntax> metadataLoaderRecordDeclarationSyntax;
        private readonly MetadataEnumDeclarationLoader metadataLoaderEnumDeclarationSyntax;

        public MetadataDeclarationFactory(
            MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax> metadataLoaderInterfaceDeclarationSyntax,
            MetadataGenericDeclarationLoader<ClassDeclarationSyntax> metadataLoaderClassDeclarationSyntax,
            MetadataGenericDeclarationLoader<StructDeclarationSyntax> metadataLoaderStructDeclarationSyntax,
            MetadataGenericDeclarationLoader<RecordDeclarationSyntax> metadataLoaderRecordDeclarationSyntax,
            MetadataEnumDeclarationLoader metadataLoaderEnumDeclarationSyntax)
        {
            this.metadataLoaderInterfaceDeclarationSyntax = metadataLoaderInterfaceDeclarationSyntax;
            this.metadataLoaderClassDeclarationSyntax = metadataLoaderClassDeclarationSyntax;
            this.metadataLoaderStructDeclarationSyntax = metadataLoaderStructDeclarationSyntax;
            this.metadataLoaderRecordDeclarationSyntax = metadataLoaderRecordDeclarationSyntax;
            this.metadataLoaderEnumDeclarationSyntax = metadataLoaderEnumDeclarationSyntax;
        }

        /// <inheritdoc/>
        public IDeclaration<SyntaxNode> CreateDeclaration(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, string location)
        {
            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            var name = metadataReader.GetString(typeDefinition.Name);

            if (!name.StartsWith("<", StringComparison.Ordinal))
            {
                var attributes = typeDefinition.Attributes;

                var visibility = attributes & TypeAttributes.VisibilityMask;

                if (visibility == TypeAttributes.Public || visibility == TypeAttributes.NestedPublic)
                {
                    var classSemantics = attributes & TypeAttributes.ClassSemanticsMask;

                    if (classSemantics == TypeAttributes.Interface)
                    {
                        return CreateInterfaceDeclaration(metadataReader, typeDefinitionHandle, location);
                    }
                    else if (classSemantics == TypeAttributes.Class)
                    {
                        if (MetadataGenericDeclarationLoader<SyntaxNode>.ProbeValueType(metadataReader, typeDefinition))
                        {
                            if (MetadataGenericDeclarationLoader<SyntaxNode>.ProbeRecordStructType(metadataReader, typeDefinition))
                            {
                                return CreateRecordStructDeclaration(metadataReader, typeDefinitionHandle, location);
                            }

                            return CreateStructDeclaration(metadataReader, typeDefinitionHandle, location);
                        }

                        if (MetadataGenericDeclarationLoader<SyntaxNode>.ProbeEnumType(metadataReader, typeDefinition))
                        {
                            return CreateEnumDeclaration(metadataReader, typeDefinitionHandle, location);
                        }

                        if (MetadataGenericDeclarationLoader<SyntaxNode>.ProbeRecordType(metadataReader, typeDefinition))
                        {
                            return CreateRecordDeclaration(metadataReader, typeDefinitionHandle, location);
                        }

                        return CreateClassDeclaration(metadataReader, typeDefinitionHandle, location);
                    }
                }
            }

            return null;
        }

        private InterfaceDeclaration CreateInterfaceDeclaration(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, string location)
        {
            var decl = new InterfaceDeclaration(
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNamespace(metadataReader, typeDefinitionHandle),
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(metadataReader, typeDefinitionHandle),
                new MetadataTypeSyntaxNodeProvider<InterfaceDeclarationSyntax>(metadataReader, typeDefinitionHandle),
                null,
                location,
                this.metadataLoaderInterfaceDeclarationSyntax);

            MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax>.Setup(decl, typeDefinitionHandle);

            return decl;
        }

        private ClassDeclaration CreateClassDeclaration(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, string location)
        {
            var decl = new ClassDeclaration(
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNamespace(metadataReader, typeDefinitionHandle),
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(metadataReader, typeDefinitionHandle),
                new MetadataTypeSyntaxNodeProvider<ClassDeclarationSyntax>(metadataReader, typeDefinitionHandle),
                null,
                location,
                this.metadataLoaderClassDeclarationSyntax);

            MetadataGenericDeclarationLoader<ClassDeclarationSyntax>.Setup(decl, typeDefinitionHandle);

            return decl;
        }

        private RecordDeclaration CreateRecordDeclaration(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, string location)
        {
            var decl = new RecordDeclaration(
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNamespace(metadataReader, typeDefinitionHandle),
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(metadataReader, typeDefinitionHandle),
                new MetadataTypeSyntaxNodeProvider<RecordDeclarationSyntax>(metadataReader, typeDefinitionHandle),
                null,
                location,
                this.metadataLoaderRecordDeclarationSyntax);

            MetadataGenericDeclarationLoader<RecordDeclarationSyntax>.Setup(decl, typeDefinitionHandle);

            return decl;
        }

        private RecordStructDeclaration CreateRecordStructDeclaration(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, string location)
        {
            var decl = new RecordStructDeclaration(
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNamespace(metadataReader, typeDefinitionHandle),
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(metadataReader, typeDefinitionHandle),
                new MetadataTypeSyntaxNodeProvider<RecordDeclarationSyntax>(metadataReader, typeDefinitionHandle),
                null,
                location,
                this.metadataLoaderRecordDeclarationSyntax);

            MetadataGenericDeclarationLoader<RecordDeclarationSyntax>.Setup(decl, typeDefinitionHandle);

            return decl;
        }

        private StructDeclaration CreateStructDeclaration(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, string location)
        {
            var decl = new StructDeclaration(
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNamespace(metadataReader, typeDefinitionHandle),
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(metadataReader, typeDefinitionHandle),
                new MetadataTypeSyntaxNodeProvider<StructDeclarationSyntax>(metadataReader, typeDefinitionHandle),
                null,
                location,
                this.metadataLoaderStructDeclarationSyntax);

            MetadataGenericDeclarationLoader<StructDeclarationSyntax>.Setup(decl, typeDefinitionHandle);

            return decl;
        }

        private EnumDeclaration CreateEnumDeclaration(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, string location)
        {
            var decl = new EnumDeclaration(
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNamespace(metadataReader, typeDefinitionHandle),
                MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(metadataReader, typeDefinitionHandle),
                new MetadataTypeSyntaxNodeProvider<EnumDeclarationSyntax>(metadataReader, typeDefinitionHandle),
                null,
                location,
                this.metadataLoaderEnumDeclarationSyntax);

            MetadataEnumDeclarationLoader.Setup(decl, typeDefinitionHandle);

            return decl;
        }
    }
}
