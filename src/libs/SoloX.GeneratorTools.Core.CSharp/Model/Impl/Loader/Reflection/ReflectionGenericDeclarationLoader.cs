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
            if (type.Namespace == "System" && (type.IsPrimitive || type == typeof(string) || type == typeof(object)))
            {
                return new PredefinedDeclarationUse(null, type.Name);
            }

            var interfaceDeclaration = resolver.Resolve(type);

            if (interfaceDeclaration == null)
            {
                return new UnknownDeclarationUse(null, new UnknownDeclaration(type.Name));
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

            return new GenericDeclarationUse(null, interfaceDeclaration, genericParameters);
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
        /// Load the generic parameters from the type parameter list node.
        /// </summary>
        protected void LoadGenericParameters(AGenericDeclaration<TNode> declaration)
        {
            if (declaration.DeclarationType.IsGenericTypeDefinition)
            {
                var parameterSet = new List<IGenericParameterDeclaration>();

                foreach (var parameter in declaration.DeclarationType.GetTypeInfo().GenericTypeParameters)
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
            var extendedInterfaces = declaration.DeclarationType.GetInterfaces();
            if (extendedInterfaces != null && extendedInterfaces.Any())
            {
                var uses = new List<IDeclarationUse<SyntaxNode>>();

                foreach (var extendedInterface in extendedInterfaces)
                {
                    uses.Add(GetDeclarationUseFrom(extendedInterface, resolver));
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

            foreach (var property in declaration.DeclarationType.GetProperties())
            {
                memberList.Add(
                    new PropertyDeclaration(
                        property.Name,
                        GetDeclarationUseFrom(property.PropertyType, resolver),
                        null));
            }

            declaration.Members = memberList.Any() ? memberList.ToArray() : Array.Empty<IMemberDeclaration<SyntaxNode>>();
        }
    }
}
