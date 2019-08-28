// ----------------------------------------------------------------------
// <copyright file="ReflectionGenericDeclarationLoader.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionGenericDeclarationLoader<TNode> : AGenericDeclarationLoader<TNode>
        where TNode : SyntaxNode
    {
#pragma warning disable CA1810 // Initialize reference type static fields inline
        static ReflectionGenericDeclarationLoader()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>.Shared
                = new ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>();
            ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>.Shared
                = new ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>();
            ReflectionGenericDeclarationLoader<StructDeclarationSyntax>.Shared
                = new ReflectionGenericDeclarationLoader<StructDeclarationSyntax>();
        }

        internal static ReflectionGenericDeclarationLoader<TNode> Shared { get; private set; }

        internal static string GetNameWithoutGeneric(string name)
        {
            var genericIdx = name.IndexOf('`');
            if (genericIdx >= 0)
            {
                return name.Substring(0, genericIdx);
            }

            return name;
        }

        internal static IDeclarationUse<SyntaxNode> GetDeclarationUseFrom(Type type, IDeclarationResolver resolver)
        {
            if (type.IsArray)
            {
                var eltType = type.GetElementType();
                var elementDeclarationUse = GetDeclarationUseFrom(eltType, resolver);

                ((IArrayDeclarationUseImpl)elementDeclarationUse).ArraySpecification = new ArraySpecification(default(SyntaxList<ArrayRankSpecifierSyntax>));

                return elementDeclarationUse;
            }

            if (type.Namespace == "System" && (type.IsPrimitive || type == typeof(string) || type == typeof(object)))
            {
                return new PredefinedDeclarationUse(new ReflectionPredefinedSyntaxNodeProvider(type), type.Name);
            }

            var interfaceDeclaration = resolver.Resolve(type);

            if (interfaceDeclaration == null)
            {
                return new UnknownDeclarationUse(
                    new ReflectionTypeUseSyntaxNodeProvider<IdentifierNameSyntax>(type),
                    new UnknownDeclaration(GetNameWithoutGeneric(type.Name)));
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

            return new GenericDeclarationUse(
                new ReflectionTypeUseSyntaxNodeProvider<SimpleNameSyntax>(type),
                interfaceDeclaration,
                genericParameters);
        }

        internal override void Load(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            this.LoadGenericParameters(declaration);
            this.LoadExtends(declaration, resolver);
            this.LoadMembers(declaration, resolver);
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
        internal void Setup(AGenericDeclaration<TNode> decl, Type type)
        {
            decl.SetData(type);
        }

        /// <summary>
        /// Load the generic parameters from the type parameter list node.
        /// </summary>
        protected void LoadGenericParameters(AGenericDeclaration<TNode> declaration)
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
        protected void LoadExtends(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
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
        protected void LoadMembers(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var memberList = new List<IMemberDeclaration<SyntaxNode>>();

            foreach (var property in declaration.GetData<Type>().GetProperties())
            {
                var propertyType = GetDeclarationUseFrom(property.PropertyType, resolver);
                memberList.Add(
                    new PropertyDeclaration(
                        property.Name,
                        propertyType,
                        new ReflectionPropertySyntaxNodeProvider(property, propertyType.SyntaxNodeProvider)));
            }

            declaration.Members = memberList.Any() ? memberList.ToArray() : Array.Empty<IMemberDeclaration<SyntaxNode>>();
        }
    }
}
