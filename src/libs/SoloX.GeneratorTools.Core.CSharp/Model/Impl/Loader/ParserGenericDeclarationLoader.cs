// ----------------------------------------------------------------------
// <copyright file="ParserGenericDeclarationLoader.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader
{
    internal class ParserGenericDeclarationLoader<TNode> : AGenericDeclarationLoader<TNode>
        where TNode : SyntaxNode
    {
        private static readonly Func<TNode, BaseListSyntax> BaseListGetter;
        private static readonly Func<TNode, TypeParameterListSyntax> TypeParameterListGetter;

#pragma warning disable CA1810 // Initialize reference type static fields inline
        static ParserGenericDeclarationLoader()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>.Shared
                = new ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>();
            ParserGenericDeclarationLoader<ClassDeclarationSyntax>.Shared
                = new ParserGenericDeclarationLoader<ClassDeclarationSyntax>();
            ParserGenericDeclarationLoader<StructDeclarationSyntax>.Shared
                = new ParserGenericDeclarationLoader<StructDeclarationSyntax>();

            ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>.BaseListGetter
                = s => s.BaseList;
            ParserGenericDeclarationLoader<ClassDeclarationSyntax>.BaseListGetter
                = s => s.BaseList;
            ParserGenericDeclarationLoader<StructDeclarationSyntax>.BaseListGetter
                = s => s.BaseList;

            ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>.TypeParameterListGetter
                = s => s.TypeParameterList;
            ParserGenericDeclarationLoader<ClassDeclarationSyntax>.TypeParameterListGetter
                = s => s.TypeParameterList;
            ParserGenericDeclarationLoader<StructDeclarationSyntax>.TypeParameterListGetter
                = s => s.TypeParameterList;
        }

        internal static ParserGenericDeclarationLoader<TNode> Shared { get; private set; }

        internal override void Load(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            this.LoadGenericParameters(declaration);
            this.LoadExtends(declaration, resolver);
            this.LoadMembers(declaration, resolver);
        }

        internal override ISyntaxNodeProvider<TypeParameterListSyntax> GetTypeParameterListSyntaxProvider(
            AGenericDeclaration<TNode> declaration)
        {
            return new ParserSyntaxNodeProvider<TypeParameterListSyntax>(
                TypeParameterListGetter(declaration.SyntaxNodeProvider.SyntaxNode));
        }

        /// <summary>
        /// Load the generic parameters from the type parameter list node.
        /// </summary>
        private void LoadGenericParameters(AGenericDeclaration<TNode> declaration)
        {
            var parameterList = TypeParameterListGetter(declaration.SyntaxNodeProvider.SyntaxNode);
            if (parameterList != null)
            {
                var parameterSet = new List<IGenericParameterDeclaration>();
                foreach (var parameter in parameterList.Parameters)
                {
                    parameterSet.Add(new GenericParameterDeclaration(
                        parameter.Identifier.Text,
                        new ParserSyntaxNodeProvider<TypeParameterSyntax>(parameter)));
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
        private void LoadExtends(
            AGenericDeclaration<TNode> declaration,
            IDeclarationResolver resolver)
        {
            BaseListSyntax baseListSyntax = BaseListGetter(declaration.SyntaxNodeProvider.SyntaxNode);

            if (baseListSyntax != null)
            {
                var baseWalker = new DeclarationUseWalker(resolver, declaration);
                var uses = new List<IDeclarationUse<SyntaxNode>>();

                foreach (var node in baseListSyntax.ChildNodes())
                {
                    var use = baseWalker.Visit(node);

                    if (use.Declaration is IGenericDeclarationImpl genericDeclaration)
                    {
                        genericDeclaration.AddExtendedBy(declaration);
                    }

                    uses.Add(use);
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
            var membersWalker = new MembersWalker(resolver, declaration, memberList);

            membersWalker.Visit(declaration.SyntaxNodeProvider.SyntaxNode);

            declaration.Members = memberList.Any() ? memberList.ToArray() : Array.Empty<IMemberDeclaration<SyntaxNode>>();
        }
    }
}
