// ----------------------------------------------------------------------
// <copyright file="SignatureTypeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using System;
using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata.Provider
{
    internal class SignatureTypeProvider : ISignatureTypeProvider<IDeclarationUse<SyntaxNode>, GenericResolver>
    {
        private readonly IDeclarationResolver resolver;

        public SignatureTypeProvider(IDeclarationResolver resolver)
        {
            this.resolver = resolver;
        }

        public IDeclarationUse<SyntaxNode> GetArrayType(IDeclarationUse<SyntaxNode> elementType, ArrayShape shape)
        {
            throw new NotImplementedException();
        }

        public IDeclarationUse<SyntaxNode> GetByReferenceType(IDeclarationUse<SyntaxNode> elementType)
        {
            // TODO convert to ref
            //throw new NotImplementedException();
            return elementType;
        }

        public IDeclarationUse<SyntaxNode> GetFunctionPointerType(MethodSignature<IDeclarationUse<SyntaxNode>> signature)
        {
            // TODO
            return null;
            //throw new NotImplementedException();
        }

        public IDeclarationUse<SyntaxNode> GetGenericInstantiation(IDeclarationUse<SyntaxNode> genericType, ImmutableArray<IDeclarationUse<SyntaxNode>> typeArguments)
        {
            return new GenericDeclarationUse(
                new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(genericType.Declaration.FullName, typeArguments),
                genericType.Declaration,
                typeArguments);
        }

        public IDeclarationUse<SyntaxNode> GetGenericMethodParameter(GenericResolver genericContext, int index)
        {
            return genericContext.ResolveMethodParameter(index);
        }

        public IDeclarationUse<SyntaxNode> GetGenericTypeParameter(GenericResolver genericContext, int index)
        {
            return genericContext.ResolveParameter(index);
        }

        public IDeclarationUse<SyntaxNode> GetModifiedType(IDeclarationUse<SyntaxNode> modifier, IDeclarationUse<SyntaxNode> unmodifiedType, bool isRequired)
        {
            // TODO modify
            //throw new NotImplementedException();
            return unmodifiedType;
        }

        public IDeclarationUse<SyntaxNode> GetPinnedType(IDeclarationUse<SyntaxNode> elementType)
        {
            // TODO convert to pinned
            //throw new NotImplementedException();
            return elementType;
        }

        public IDeclarationUse<SyntaxNode> GetPointerType(IDeclarationUse<SyntaxNode> elementType)
        {
            // TODO convert to pointer
            //throw new NotImplementedException();
            return elementType;
        }

        public IDeclarationUse<SyntaxNode> GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            if (typeCode == PrimitiveTypeCode.Int32)
            {
                return CreatePredefinedDeclarationUse("int");
            }

            var fullName = $"System.{typeCode}";
            var declaration = this.resolver.Resolve(fullName, null) ?? new UnknownDeclaration("System", typeCode.ToString());

            return new GenericDeclarationUse(
                new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(declaration.FullName),
                declaration,
                Array.Empty<IDeclarationUse<SyntaxNode>>());
        }

        internal static PredefinedDeclarationUse CreatePredefinedDeclarationUse(string typeName)
        {
            var predefinedDeclarationUse = new PredefinedDeclarationUse(
                new MetadataPredefinedSyntaxNodeProvider(typeName),
                typeName);
            return predefinedDeclarationUse;
        }

        public IDeclarationUse<SyntaxNode> GetSZArrayType(IDeclarationUse<SyntaxNode> elementType)
        {
            if (elementType is IGenericDeclarationUse genericDeclarationUse)
            {
                return new GenericDeclarationUse(
                    new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(elementType.Declaration.FullName, genericDeclarationUse.GenericParameters),
                    elementType.Declaration,
                    genericDeclarationUse.GenericParameters)
                {
                    ArraySpecification = new ArraySpecification((elementType.ArraySpecification?.ArrayCount ?? 0) + 1, null),
                };
            }
            else if (elementType is IPredefinedDeclarationUse predefinedDeclarationUse)
            {
                return new PredefinedDeclarationUse(
                    predefinedDeclarationUse.SyntaxNodeProvider,
                    predefinedDeclarationUse.Declaration.Name)
                {
                    ArraySpecification = new ArraySpecification((elementType.ArraySpecification?.ArrayCount ?? 0) + 1, null),
                };
            }
            else if (elementType is IGenericParameterDeclarationUse genericParameterDeclarationUse)
            {
                return new GenericParameterDeclarationUse(
                    genericParameterDeclarationUse.SyntaxNodeProvider,
                    genericParameterDeclarationUse.Declaration)
                {
                    ArraySpecification = new ArraySpecification((elementType.ArraySpecification?.ArrayCount ?? 0) + 1, null),
                };
            }

            throw new NotSupportedException();
        }

        public IDeclarationUse<SyntaxNode> GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            var typeDefinition = reader.GetTypeDefinition(handle);

            var name = reader.GetString(typeDefinition.Name);
            var nameSpace = reader.GetString(typeDefinition.Namespace);

            var nested = typeDefinition.IsNested;
            string fullName;

            if (nested)
            {
                var declaringTypeHandle = typeDefinition.GetDeclaringType();

                var declaringType = GetTypeFromDefinition(reader, declaringTypeHandle, rawTypeKind);

                fullName = $"{declaringType.Declaration.FullName}+{name}";
            }
            else
            {
                fullName = $"{nameSpace}.{name}";
            }

            var declaration = this.resolver.Resolve(fullName, null) ?? new UnknownDeclaration(nameSpace, MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(name));

            return new GenericDeclarationUse(
                new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(declaration.FullName),
                declaration,
                Array.Empty<IDeclarationUse<SyntaxNode>>());
        }

        public IDeclarationUse<SyntaxNode> GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            var typeReference = reader.GetTypeReference(handle);

            var name = reader.GetString(typeReference.Name);
            var nameSpace = reader.GetString(typeReference.Namespace);

            var resolutionScope = typeReference.ResolutionScope;

            string fullName = null;

            if (!resolutionScope.IsNil)
            {
#pragma warning disable IDE0010 // Add missing cases
                switch (resolutionScope.Kind)
                {
                    case HandleKind.ImportScope:

                        throw new NotImplementedException();

                        break;
                    case HandleKind.LocalScope:

                        throw new NotImplementedException();

                        break;
                    case HandleKind.AssemblyReference:
                        fullName = $"{nameSpace}.{name}";
                        break;
                    case HandleKind.TypeReference:
                        var refType = GetTypeFromReference(reader, (TypeReferenceHandle)resolutionScope, rawTypeKind);
                        fullName = $"{refType.Declaration.FullName}+{name}";
                        break;
                    default:
                        throw new NotImplementedException();
                }
#pragma warning restore IDE0010 // Add missing cases
            }

            var declaration = this.resolver.Resolve(fullName, null) ?? new UnknownDeclaration(nameSpace, MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(name));

            return new GenericDeclarationUse(
                new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(declaration.FullName),
                declaration,
                Array.Empty<IDeclarationUse<SyntaxNode>>());
        }

        public IDeclarationUse<SyntaxNode> GetTypeFromSpecification(MetadataReader reader, GenericResolver genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            throw new NotImplementedException();
        }
    }
}
