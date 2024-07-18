// ----------------------------------------------------------------------
// <copyright file="MetadataEnumDeclarationLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata.Provider;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.Utils;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataEnumDeclarationLoader : AEnumDeclarationLoader
    {
        private readonly IGeneratorLogger<MetadataEnumDeclarationLoader> logger;

        public MetadataEnumDeclarationLoader(IGeneratorLogger<MetadataEnumDeclarationLoader> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Setup the given declaration to be loaded by reflection from the given type.
        /// </summary>
        /// <param name="decl">The declaration that will be loaded.</param>
        /// <param name="typeDefinitionHandle">The type definition handle to load the declaration from.</param>
        internal static void Setup(EnumDeclaration decl, TypeDefinitionHandle typeDefinitionHandle)
        {
            decl.SetData(typeDefinitionHandle);
        }

        internal override void Load(EnumDeclaration declaration, IDeclarationResolver resolver)
        {
            var assemblyPath = declaration.Location;

            using var portableExecutableReader = new PEReader(File.OpenRead(assemblyPath));

            var metadataReader = portableExecutableReader.GetMetadataReader();

            var typeDefinitionHandle = declaration.GetData<TypeDefinitionHandle>();

            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            var attributes = typeDefinition.Attributes;

            var classSemantics = attributes & TypeAttributes.ClassSemanticsMask;

            var layoutAttr = attributes & TypeAttributes.LayoutMask;

            var sealedAttr = attributes & TypeAttributes.Sealed;

            var name = declaration.FullName;

            var layout = typeDefinition.GetLayout();

            var baseName = string.Empty;

            if (typeDefinition.BaseType.Kind == HandleKind.TypeDefinition)
            {
                var baseTypeDefinition = metadataReader.GetTypeDefinition((TypeDefinitionHandle)typeDefinition.BaseType);

                var baseTypeName = MetadataGenericDeclarationLoader<SyntaxNode>.LoadString(metadataReader, baseTypeDefinition.Name);

                var baseTypeNs = MetadataGenericDeclarationLoader<SyntaxNode>.LoadString(metadataReader, baseTypeDefinition.Namespace);

                baseName = $"{baseTypeNs}.{baseTypeName}";
            }
            else if (typeDefinition.BaseType.Kind == HandleKind.TypeReference)
            {
                var baseTypeReference = metadataReader.GetTypeReference((TypeReferenceHandle)typeDefinition.BaseType);

                var baseTypeName = MetadataGenericDeclarationLoader<SyntaxNode>.LoadString(metadataReader, baseTypeReference.Name);

                var baseTypeNs = MetadataGenericDeclarationLoader<SyntaxNode>.LoadString(metadataReader, baseTypeReference.Namespace);

                baseName = $"{baseTypeNs}.{baseTypeName}";
            }

            var firstFieldHandle = typeDefinition.GetFields().FirstOrDefault();

            var underlyingType = typeof(Enum);

            if (firstFieldHandle != null)
            {
                var fieldDefinition = metadataReader.GetFieldDefinition(firstFieldHandle);

                var sig = fieldDefinition.DecodeSignature(
                    new SignatureTypeProvider(resolver),
                        new GenericResolver(null, null));

                if (sig.Declaration.FullName != typeof(int).FullName && sig.Declaration.FullName != "int")
                {
                    declaration.UnderlyingType = sig;
                }
            }
            LoadAttributes(metadataReader, declaration, resolver);
        }

        private static void LoadAttributes(MetadataReader metadataReader, EnumDeclaration declaration, IDeclarationResolver resolver)
        {
            var typeDefinitionHandle = declaration.GetData<TypeDefinitionHandle>();

            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            var customAttributeHandles = typeDefinition.GetCustomAttributes();

            var attributeList = MetadataGenericDeclarationLoader<SyntaxNode>.LoadCustomAttributes(metadataReader, null, resolver, customAttributeHandles);

            declaration.Attributes = attributeList;
        }
    }
}
