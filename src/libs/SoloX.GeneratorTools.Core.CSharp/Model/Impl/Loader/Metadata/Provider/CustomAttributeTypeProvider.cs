// ----------------------------------------------------------------------
// <copyright file="CustomAttributeTypeProvider.cs" company="Xavier Solau">
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
using System.Linq;
using System.Reflection.Metadata;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata.Provider
{
    internal class CustomAttributeTypeProvider : ICustomAttributeTypeProvider<IDeclarationUse<SyntaxNode>>
    {
        private readonly IDeclarationResolver resolver;

        public CustomAttributeTypeProvider(IDeclarationResolver resolver)
        {
            this.resolver = resolver;
        }

        public IDeclarationUse<SyntaxNode> GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
#pragma warning disable IDE0072 // Add missing cases
            return typeCode switch
            {
                PrimitiveTypeCode.Boolean => SignatureTypeProvider.CreatePredefinedDeclarationUse("bool"),
                PrimitiveTypeCode.Char => SignatureTypeProvider.CreatePredefinedDeclarationUse("char"),
                PrimitiveTypeCode.Byte => SignatureTypeProvider.CreatePredefinedDeclarationUse("byte"),
                PrimitiveTypeCode.SByte => SignatureTypeProvider.CreatePredefinedDeclarationUse("sbyte"),
                PrimitiveTypeCode.Int16 => SignatureTypeProvider.CreatePredefinedDeclarationUse("short"),
                PrimitiveTypeCode.Int32 => SignatureTypeProvider.CreatePredefinedDeclarationUse("int"),
                PrimitiveTypeCode.Int64 => SignatureTypeProvider.CreatePredefinedDeclarationUse("long"),
                PrimitiveTypeCode.UInt16 => SignatureTypeProvider.CreatePredefinedDeclarationUse("ushort"),
                PrimitiveTypeCode.UInt32 => SignatureTypeProvider.CreatePredefinedDeclarationUse("uint"),
                PrimitiveTypeCode.UInt64 => SignatureTypeProvider.CreatePredefinedDeclarationUse("ulong"),
                PrimitiveTypeCode.Double => SignatureTypeProvider.CreatePredefinedDeclarationUse("double"),
                PrimitiveTypeCode.Single => SignatureTypeProvider.CreatePredefinedDeclarationUse("float"),
                PrimitiveTypeCode.String => SignatureTypeProvider.CreatePredefinedDeclarationUse("string"),
                PrimitiveTypeCode.Object => SignatureTypeProvider.CreatePredefinedDeclarationUse("object"),
                _ => throw new NotImplementedException(),
            };
#pragma warning restore IDE0072 // Add missing cases
        }

        public IDeclarationUse<SyntaxNode> GetSystemType()
        {
            var declaration = this.resolver.Resolve("System.Type", null) ?? new UnknownDeclaration("System", "Type");

            return new GenericDeclarationUse(
                new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(declaration.FullName),
                declaration,
                Array.Empty<IDeclarationUse<SyntaxNode>>());
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

        public IDeclarationUse<SyntaxNode> GetTypeFromSerializedName(string name)
        {
            var assemblyQualifiedName = name.Split(',');
            var fullName = assemblyQualifiedName[0];

            var fn = fullName.Split('.');
            string nameSpace = null;
            var n = fullName;
            if (fn.Length > 1)
            {
                nameSpace = string.Join(".", fn.Take(fn.Length - 1));
                n = fn[fn.Length - 1];
            }

            var declaration = this.resolver.Resolve(fullName, null) ?? new UnknownDeclaration(nameSpace, MetadataGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(n));

            return new GenericDeclarationUse(
                new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(declaration.FullName),
                declaration,
                Array.Empty<IDeclarationUse<SyntaxNode>>());
        }

        public PrimitiveTypeCode GetUnderlyingEnumType(IDeclarationUse<SyntaxNode> type)
        {
            if (Enum.TryParse<PrimitiveTypeCode>(type.Declaration.Name, out var res) && type.Declaration.FullName == $"System.{res}")
            {
                return res;
            }

            if (type.Declaration is EnumDeclaration enumDeclaration)
            {
                var underlyingType = enumDeclaration.UnderlyingType;
                if (underlyingType != null)
                {
                    if (underlyingType is IPredefinedDeclarationUse predefinedDeclarationUse)
                    {
                        return predefinedDeclarationUse.Declaration.FullName switch
                        {
                            "byte" => PrimitiveTypeCode.Byte,
                            "sbyte" => PrimitiveTypeCode.SByte,
                            "short" => PrimitiveTypeCode.Int16,
                            "ushort" => PrimitiveTypeCode.UInt16,
                            "int" => PrimitiveTypeCode.Int32,
                            "uint" => PrimitiveTypeCode.UInt32,
                            "long" => PrimitiveTypeCode.Int64,
                            "ulong" => PrimitiveTypeCode.UInt64,
                            "double" => PrimitiveTypeCode.Double,
                            "float" => PrimitiveTypeCode.Single,
                            _ => throw new NotSupportedException($"Unsupported predefined type: {predefinedDeclarationUse.Declaration.FullName}"),
                        };
                    }

                    if (Enum.TryParse<PrimitiveTypeCode>(underlyingType.Declaration.Name, out res) && underlyingType.Declaration.FullName == $"System.{res}")
                    {
                        return res;
                    }
                }

                return PrimitiveTypeCode.UInt32;
            }

            return PrimitiveTypeCode.UInt32;
        }

        public bool IsSystemType(IDeclarationUse<SyntaxNode> type)
        {
            return type.Declaration.FullName == "System.Type";
        }
    }
}
