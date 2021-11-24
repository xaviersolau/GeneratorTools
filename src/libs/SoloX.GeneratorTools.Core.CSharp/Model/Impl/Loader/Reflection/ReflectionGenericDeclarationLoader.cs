// ----------------------------------------------------------------------
// <copyright file="ReflectionGenericDeclarationLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionGenericDeclarationLoader<TNode> : AGenericDeclarationLoader<TNode>
        where TNode : SyntaxNode
    {
        private readonly ILogger<ReflectionGenericDeclarationLoader<TNode>> logger;

        public ReflectionGenericDeclarationLoader(ILogger<ReflectionGenericDeclarationLoader<TNode>> logger)
        {
            this.logger = logger;
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

        internal static bool TryGetPredefinedDeclarationUse(
            Type type,
            out PredefinedDeclarationUse typeUse)
        {
            typeUse = null;
            if (type == typeof(byte))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "byte");
            }
            else if (type == typeof(short))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "short");
            }
            else if (type == typeof(int))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "int");
            }
            else if (type == typeof(long))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "long");
            }
            else if (type == typeof(string))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "string");
            }
            else if (type == typeof(object))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "object");
            }
            else if (type == typeof(float))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "float");
            }
            else if (type == typeof(double))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "double");
            }
            else if (type == typeof(decimal))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "decimal");
            }
            else if (type == typeof(char))
            {
                typeUse = CreatePredefinedDeclarationUse(type, "char");
            }

            return typeUse != null;
        }

        internal static IDeclarationUse<SyntaxNode> GetDeclarationUseFrom(
            Type type,
            IDeclarationResolver resolver,
            int arrayCount = 0)
        {
            if (type.IsArray)
            {
                var eltType = type.GetElementType();
                return GetDeclarationUseFrom(eltType, resolver, arrayCount + 1);
            }

            if (TryGetPredefinedDeclarationUse(type, out var typeUse))
            {
                typeUse.ArraySpecification = CreateArraySpecification(
                    arrayCount,
                    typeUse.SyntaxNodeProvider);
                return typeUse;
            }

            var interfaceDeclaration = resolver.Resolve(type);

            if (interfaceDeclaration == null)
            {
                var unknownDeclarationUse = new UnknownDeclarationUse(
                    new ReflectionTypeUseSyntaxNodeProvider<IdentifierNameSyntax>(type),
                    new UnknownDeclaration(GetNameWithoutGeneric(type.Name)));
                unknownDeclarationUse.ArraySpecification = CreateArraySpecification(
                    arrayCount,
                    unknownDeclarationUse.SyntaxNodeProvider);
                return unknownDeclarationUse;
            }

            IReadOnlyCollection<IDeclarationUse<SyntaxNode>> genericParameters;

            if (type.IsGenericType)
            {
                var uses = new List<IDeclarationUse<SyntaxNode>>();
                foreach (var typeArg in type.GenericTypeArguments)
                {
                    uses.Add(GetDeclarationUseFrom(typeArg, resolver));
                }

                genericParameters = uses;
            }
            else
            {
                genericParameters = Array.Empty<IDeclarationUse<SyntaxNode>>();
            }

            var genericDeclarationUse = new GenericDeclarationUse(
                new ReflectionTypeUseSyntaxNodeProvider<SimpleNameSyntax>(type),
                interfaceDeclaration,
                genericParameters);
            genericDeclarationUse.ArraySpecification = CreateArraySpecification(
                arrayCount,
                genericDeclarationUse.SyntaxNodeProvider);
            return genericDeclarationUse;
        }

        internal override void Load(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            LoadGenericParameters(declaration);
            LoadExtends(declaration, resolver);
            LoadMembers(declaration, resolver);
            LoadAttributes(declaration, resolver);
        }

        internal override ISyntaxNodeProvider<TypeParameterListSyntax> GetTypeParameterListSyntaxProvider(
            AGenericDeclaration<TNode> declaration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Setup the given declaration to be loaded by reflection from the given type.
        /// </summary>
        /// <param name="decl">The declaration that will be loaded.</param>
        /// <param name="type">The type to load the declaration from.</param>
        internal static void Setup(AGenericDeclaration<TNode> decl, Type type)
        {
            decl.SetData(type);
        }

        private static IArraySpecification CreateArraySpecification(
            int arrayCount,
            ISyntaxNodeProvider<SyntaxNode> syntaxNodeProvider)
        {
            return arrayCount != 0
                ? new ArraySpecification(
                    arrayCount,
                    new ReflectionArraySyntaxNodeProvider(arrayCount, syntaxNodeProvider))
                : null;
        }

        private static PredefinedDeclarationUse CreatePredefinedDeclarationUse(
            Type type,
            string typeName)
        {
            var predefinedDeclarationUse = new PredefinedDeclarationUse(
                new ReflectionPredefinedSyntaxNodeProvider(type),
                typeName);
            return predefinedDeclarationUse;
        }

        private static IReadOnlyCollection<IGenericParameterDeclaration> LoadGenericParameters(MethodInfo methodInfo)
        {
            IReadOnlyCollection<IGenericParameterDeclaration> genericParameters;
            if (methodInfo.IsGenericMethod)
            {
                var parameterSet = new List<IGenericParameterDeclaration>();
                foreach (var parameter in methodInfo.GetGenericArguments())
                {
                    parameterSet.Add(new GenericParameterDeclaration(
                        parameter.Name,
                        null));
                }

                genericParameters = parameterSet;
            }
            else
            {
                genericParameters = Array.Empty<IGenericParameterDeclaration>();
            }

            return genericParameters;
        }

        private static IReadOnlyCollection<IParameterDeclaration> LoadParameters(MethodInfo method, IDeclarationResolver resolver)
        {
            IReadOnlyCollection<IParameterDeclaration> parameters;

            var paramInfos = method.GetParameters();

            if (paramInfos != null && paramInfos.Any())
            {
                var parameterSet = new List<IParameterDeclaration>();
                foreach (var pi in paramInfos)
                {
                    parameterSet.Add(new ParameterDeclaration(pi.Name, GetDeclarationUseFrom(pi.ParameterType, resolver), null));
                }

                parameters = parameterSet;
            }
            else
            {
                parameters = Array.Empty<IParameterDeclaration>();
            }

            return parameters;
        }

        /// <summary>
        /// Load the generic parameters from the type parameter list node.
        /// </summary>
        private static void LoadGenericParameters(AGenericDeclaration<TNode> declaration)
        {
            var declarationType = declaration.GetData<Type>();

            if (declarationType.IsGenericTypeDefinition)
            {
                var parameterSet = new List<IGenericParameterDeclaration>();

                foreach (var parameter in declarationType.GetTypeInfo().GenericTypeParameters)
                {
                    parameterSet.Add(new GenericParameterDeclaration(parameter.Name, null));
                }

                declaration.GenericParameters = parameterSet;
            }
            else
            {
                declaration.GenericParameters = Array.Empty<IGenericParameterDeclaration>();
            }
        }

        /// <summary>
        /// Load extends statement list.
        /// </summary>
        /// <param name="declaration">The declaration to load.</param>
        /// <param name="resolver">The resolver to resolve dependencies.</param>
        private static void LoadExtends(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var declarationType = declaration.GetData<Type>();
            var extendedInterfaces = declarationType.GetInterfaces();
            if ((extendedInterfaces != null && extendedInterfaces.Any()) || declarationType.BaseType != null)
            {
                var uses = new List<IDeclarationUse<SyntaxNode>>();

                if (declarationType.BaseType != null)
                {
                    uses.Add(GetDeclarationUseFrom(declarationType.BaseType, resolver));
                }

                if (extendedInterfaces != null)
                {
                    foreach (var extendedInterface in extendedInterfaces)
                    {
                        uses.Add(GetDeclarationUseFrom(extendedInterface, resolver));
                    }
                }

                declaration.Extends = uses;
            }
            else
            {
                declaration.Extends = Array.Empty<IDeclarationUse<SyntaxNode>>();
            }
        }

        /// <summary>
        /// Load member list.
        /// </summary>
        /// <param name="declaration">The declaration to load.</param>
        /// <param name="resolver">The resolver to resolve dependencies.</param>
        private void LoadMembers(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var memberList = new List<IMemberDeclaration<SyntaxNode>>();

            try
            {
                foreach (var property in declaration.GetData<Type>().GetProperties())
                {
                    var propertyType = GetDeclarationUseFrom(property.PropertyType, resolver);
                    memberList.Add(
                        new PropertyDeclaration(
                            property.Name,
                            propertyType,
                            new ReflectionPropertySyntaxNodeProvider(property, propertyType.SyntaxNodeProvider),
                            property.CanRead,
                            property.CanWrite));
                }

                foreach (var method in declaration.GetData<Type>().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    var returnType = GetDeclarationUseFrom(method.ReturnType, resolver);

                    var genericParameters = LoadGenericParameters(method);

                    var parameters = LoadParameters(method, resolver);

                    memberList.Add(
                        new MethodDeclaration(
                            method.Name,
                            returnType,
                            new ReflectionMethodSyntaxNodeProvider(method, returnType.SyntaxNodeProvider),
                            genericParameters,
                            parameters));
                }
            }
            catch (TypeLoadException e)
            {
                this.logger?.LogWarning($"Could not load properties from {declaration.GetData<Type>()} ({e.Message})");
            }
            catch (FileNotFoundException e)
            {
                this.logger?.LogWarning($"Could not load properties from {declaration.GetData<Type>()} ({e.Message})");
            }

            declaration.Members = memberList.Any() ? memberList.ToArray() : Array.Empty<IMemberDeclaration<SyntaxNode>>();
        }

        private static void LoadAttributes(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var attributeList = new List<IAttributeUse>();

            var declType = declaration.GetData<Type>();

            foreach (var customAttribute in declType.CustomAttributes)
            {
                attributeList.Add(
                    new AttributeUse(
                        GetDeclarationUseFrom(customAttribute.AttributeType, resolver).Declaration,
                        new ReflectionAttributeSyntaxNodeProvider(customAttribute)));
            }

            declaration.Attributes = attributeList.Any() ? attributeList.ToArray() : Array.Empty<IAttributeUse>();
        }
    }
}
