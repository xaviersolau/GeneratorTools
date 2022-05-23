// ----------------------------------------------------------------------
// <copyright file="MetadataGenericDeclarationLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata.Provider;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataGenericDeclarationLoader<TNode> : AGenericDeclarationLoader<TNode>
        where TNode : SyntaxNode
    {
        private readonly IGeneratorLogger<MetadataGenericDeclarationLoader<TNode>> logger;

        public MetadataGenericDeclarationLoader(IGeneratorLogger<MetadataGenericDeclarationLoader<TNode>> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Setup the given declaration to be loaded by reflection from the given type.
        /// </summary>
        /// <param name="decl">The declaration that will be loaded.</param>
        /// <param name="typeDefinitionHandle">The type definition handle to load the declaration from.</param>
        internal static void Setup(AGenericDeclaration<TNode> decl, TypeDefinitionHandle typeDefinitionHandle)
        {
            decl.SetData(typeDefinitionHandle);
        }

        internal override ISyntaxNodeProvider<TypeParameterListSyntax> GetTypeParameterListSyntaxProvider(AGenericDeclaration<TNode> declaration)
        {
            throw new NotImplementedException();
        }

        internal override void Load(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var assemblyPath = declaration.Location;

            using var portableExecutableReader = new PEReader(File.OpenRead(assemblyPath));

            var metadataReader = portableExecutableReader.GetMetadataReader();

            LoadGenericParameters(metadataReader, declaration);
            LoadExtends(metadataReader, declaration, resolver);
            LoadMembers(metadataReader, declaration, resolver);
            LoadAttributes(metadataReader, declaration, resolver);
        }

        private static void LoadAttributes(MetadataReader metadataReader, AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var attributeList = new List<IAttributeUse>();

            var typeDefinitionHandle = declaration.GetData<TypeDefinitionHandle>();

            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            foreach (var attributeHandle in typeDefinition.GetCustomAttributes())
            {
                var attribute = metadataReader.GetCustomAttribute(attributeHandle);

                IDeclarationUse<SyntaxNode> attributeDeclarationUse = null;

#pragma warning disable IDE0010 // Add missing cases
                switch (attribute.Constructor.Kind)
                {
                    case HandleKind.MethodDefinition:
                        var methodDefinitionHandle = (MethodDefinitionHandle)attribute.Constructor;

                        var methodDefinition = metadataReader.GetMethodDefinition(methodDefinitionHandle);

                        //var methodDefinitionName = LoadString(metadataReader, methodDefinition.Name);

                        var methodDeclaringTypeHandle = methodDefinition.GetDeclaringType();

                        attributeDeclarationUse = LoadDeclarationUseFromTypeDefinition(metadataReader, methodDeclaringTypeHandle, resolver);

                        break;
                    case HandleKind.MemberReference:
                        var memberReferenceHandler = (MemberReferenceHandle)attribute.Constructor;
                        var memberReference = metadataReader.GetMemberReference(memberReferenceHandler);

                        attributeDeclarationUse = GetDeclarationUseFrom(metadataReader, memberReference.Parent, resolver, declaration);

                        //var memberReferenceName = LoadString(metadataReader, memberReference.Name);

                        break;
                    default:
                        throw new NotSupportedException();
                }
#pragma warning restore IDE0010 // Add missing cases

                var value = attribute.DecodeValue(
                    new CustomAttributeTypeProvider(resolver));

                attributeList.Add(
                    new AttributeUse(
                        attributeDeclarationUse.Declaration,
                        new MetadataAttributeSyntaxNodeProvider(attributeDeclarationUse.Declaration.FullName, value)));
            }

            declaration.Attributes = attributeList;
        }

        private static void LoadMembers(MetadataReader metadataReader, AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var memberList = new List<IMemberDeclaration<SyntaxNode>>();

            var typeDefinitionHandle = declaration.GetData<TypeDefinitionHandle>();

            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            foreach (var propertyHandle in typeDefinition.GetProperties())
            {
                var property = metadataReader.GetPropertyDefinition(propertyHandle);

                var propertyName = LoadString(metadataReader, property.Name);

                var propertySignature = property.DecodeSignature(
                    new SignatureTypeProvider(resolver),
                    new GenericResolver(declaration));

                var accessors = property.GetAccessors();

                var getterHandle = accessors.Getter;
                var setterHandle = accessors.Setter;

                var propertyType = propertySignature.ReturnType;
                memberList.Add(
                    new PropertyDeclaration(
                        propertyName,
                        propertyType,
                        new MetadataPropertySyntaxNodeProvider<PropertyDeclarationSyntax>(),
                        !getterHandle.IsNil,
                        !setterHandle.IsNil));
            }

            foreach (var methodHandle in typeDefinition.GetMethods())
            {
                var methodDefinition = metadataReader.GetMethodDefinition(methodHandle);

                var attributes = methodDefinition.Attributes;

                var methodName = LoadString(metadataReader, methodDefinition.Name);
                if ((attributes & MethodAttributes.Public) != 0 && (attributes & MethodAttributes.SpecialName) == 0)
                {
                    var genericParameters = LoadGenericParameters(metadataReader, methodDefinition);

                    var methodSignature = methodDefinition.DecodeSignature(
                        new SignatureTypeProvider(resolver),
                        new GenericResolver(declaration, genericParameters));


                    var parameters = LoadParameters(metadataReader, methodDefinition, methodSignature);

                    memberList.Add(
                        new MethodDeclaration(
                            methodName,
                            methodSignature.ReturnType,
                            new ReflectionMethodSyntaxNodeProvider(),
                            genericParameters,
                            parameters));
                }
            }

            declaration.Members = memberList;
        }

        private static IReadOnlyCollection<IParameterDeclaration> LoadParameters(
            MetadataReader metadataReader,
            MethodDefinition methodDefinition,
            MethodSignature<IDeclarationUse<SyntaxNode>> methodSignature)
        {
            var parameterSet = new List<IParameterDeclaration>();

            foreach (var parameterHandle in methodDefinition.GetParameters())
            {
                var parameter = metadataReader.GetParameter(parameterHandle);

                if (parameter.SequenceNumber > 0)
                {
                    var parameterName = LoadString(metadataReader, parameter.Name);

                    parameterSet.Add(new ParameterDeclaration(parameterName, methodSignature.ParameterTypes[parameter.SequenceNumber - 1], null));
                }
            }

            if (parameterSet.Count != methodSignature.ParameterTypes.Length)
            {

            }

            return parameterSet;
        }

        private static IReadOnlyCollection<IGenericParameterDeclaration> LoadGenericParameters(MetadataReader metadataReader, MethodDefinition methodDefinition)
        {
            var parameterSet = new List<IGenericParameterDeclaration>();
            foreach (var genericParameterHandle in methodDefinition.GetGenericParameters())
            {
                if (!genericParameterHandle.IsNil)
                {
                    var genericParameter = metadataReader.GetGenericParameter(genericParameterHandle);

                    var parameterName = metadataReader.GetString(genericParameter.Name);

                    var parameterIndex = genericParameter.Index;

                    parameterSet.Add(new GenericParameterDeclaration(
                        parameterName,
                        null));
                }
            }

            return parameterSet;
        }

        private static void LoadExtends(MetadataReader metadataReader, AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var typeDefinitionHandle = declaration.GetData<TypeDefinitionHandle>();

            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            var uses = new List<IDeclarationUse<SyntaxNode>>();

            if (!typeDefinition.BaseType.IsNil)
            {
                uses.Add(GetDeclarationUseFrom(metadataReader, typeDefinition.BaseType, resolver, declaration));
            }

            foreach (var interfaceImplementation in typeDefinition.GetInterfaceImplementations())
            {
                if (!interfaceImplementation.IsNil)
                {
                    uses.Add(GetDeclarationUseFrom(metadataReader, interfaceImplementation, resolver, declaration));
                }
            }

            declaration.Extends = uses;
        }

        private static void LoadGenericParameters(MetadataReader metadataReader, AGenericDeclaration<TNode> declaration)
        {
            var typeDefinitionHandle = declaration.GetData<TypeDefinitionHandle>();

            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            var parameterSet = new IGenericParameterDeclaration[typeDefinition.GetGenericParameters().Count];

            foreach (var genericParameterHandle in typeDefinition.GetGenericParameters())
            {
                if (!genericParameterHandle.IsNil)
                {
                    var genericParameter = metadataReader.GetGenericParameter(genericParameterHandle);

                    var parameterName = metadataReader.GetString(genericParameter.Name);

                    var parameterIndex = genericParameter.Index;

                    parameterSet[parameterIndex] = new GenericParameterDeclaration(parameterName, null);
                }
            }

            declaration.GenericParameters = parameterSet;
        }

        private static IDeclarationUse<SyntaxNode> GetDeclarationUseFrom(
            MetadataReader metadataReader,
            EntityHandle typeHandle,
            IDeclarationResolver resolver,
            IGenericDeclaration<TNode> declarationContext)
        {
            if (typeHandle.IsNil)
            {
                return null;
            }

#pragma warning disable IDE0010 // Add missing cases
            switch (typeHandle.Kind)
            {
                case HandleKind.TypeDefinition:
                    return LoadDeclarationUseFromTypeDefinition(metadataReader, (TypeDefinitionHandle)typeHandle, resolver);
                case HandleKind.TypeReference:
                    return LoadDeclarationUseFromTypeReference(metadataReader, (TypeReferenceHandle)typeHandle, resolver);
                case HandleKind.TypeSpecification:
                    return LoadDeclarationUseFromTypeSpecification(metadataReader, (TypeSpecificationHandle)typeHandle, resolver, declarationContext);
                default:
                    break;
            }
#pragma warning restore IDE0010 // Add missing cases

            return null;
        }

        private static IDeclarationUse<SyntaxNode> LoadDeclarationUseFromTypeSpecification(
            MetadataReader metadataReader,
            TypeSpecificationHandle typeSpecificationHandle,
            IDeclarationResolver resolver,
            IGenericDeclaration<TNode> declarationContext)
        {
            var typeSpecification = metadataReader.GetTypeSpecification(typeSpecificationHandle);

            var declarationUse = typeSpecification.DecodeSignature(
                    new SignatureTypeProvider(resolver),
                    new GenericResolver(declarationContext));

            return declarationUse;

        }

        private static IDeclarationUse<SyntaxNode> LoadDeclarationUseFromTypeReference(MetadataReader metadataReader, TypeReferenceHandle typeReferenceHandle, IDeclarationResolver resolver)
        {
            var typeReference = metadataReader.GetTypeReference(typeReferenceHandle);

            var ns = LoadString(metadataReader, typeReference.Namespace);
            var name = LoadString(metadataReader, typeReference.Name);
            var fullName = GetFullName(ns, name);

            var declaration = resolver.Resolve(fullName, null);
            if (declaration == null)
            {
                declaration = new UnknownDeclaration(ns, GetNameWithoutGeneric(name));
            }

            var genericDeclarationUse = new GenericDeclarationUse(
                new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(declaration.FullName),
                declaration,
                Array.Empty<IDeclarationUse<SyntaxNode>>());

            return genericDeclarationUse;
        }

        private static IDeclarationUse<SyntaxNode> LoadDeclarationUseFromTypeDefinition(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, IDeclarationResolver resolver)
        {
            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            var name = LoadString(metadataReader, typeDefinition.Name);

            var ns = LoadString(metadataReader, typeDefinition.Namespace);

            var fullName = GetFullName(ns, name);

            var declaration = resolver.Resolve(fullName, null);
            if (declaration == null)
            {
                declaration = new UnknownDeclaration(ns, GetNameWithoutGeneric(name));
            }

            var genericDeclarationUse = new GenericDeclarationUse(
                new MetadataTypeUseSyntaxNodeProvider<NameSyntax>(declaration.FullName),
                declaration,
                Array.Empty<IDeclarationUse<SyntaxNode>>());

            return genericDeclarationUse;
        }

        internal static string GetNamespace(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle)
        {
            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            var ns = LoadString(metadataReader, typeDefinition.Namespace);

            return ns;
        }

        internal static string GetName(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle)
        {
            var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

            var name = LoadString(metadataReader, typeDefinition.Name);

            return name;
        }

        internal static string GetNameWithoutGeneric(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle)
        {
            return GetNameWithoutGeneric(GetName(metadataReader, typeDefinitionHandle));
        }

        internal static string GetNameWithoutGeneric(string name)
        {
            var genericIdx = name.IndexOf('`');
            if (genericIdx >= 0)
            {
                return name.Substring(0, genericIdx);
            }

            return name;
        }

        private static string LoadString(MetadataReader metadataReader, StringHandle stringHandle)
        {
            return stringHandle.IsNil ? null : metadataReader.GetString(stringHandle);
        }

        internal static string GetFullName(string ns, string name)
        {
            if (ns == null)
            {
                return name;
            }
            return $"{ns}.{name}";
        }
    }
}
