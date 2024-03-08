// ----------------------------------------------------------------------
// <copyright file="ParserGenericDeclarationLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Exceptions;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;
using SoloX.GeneratorTools.Core.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser
{
    internal class ParserGenericDeclarationLoader<TNode> : AGenericDeclarationLoader<TNode>
        where TNode : TypeDeclarationSyntax
    {
        private readonly IGeneratorLogger<ParserGenericDeclarationLoader<TNode>> logger;

        public ParserGenericDeclarationLoader(IGeneratorLogger<ParserGenericDeclarationLoader<TNode>> logger)
        {
            this.logger = logger;
        }

        internal override void Load(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            try
            {
                this.logger.LogDebug($"Loading {declaration.FullName} from source code");

                LoadGenericParameters(declaration);
                LoadExtends(declaration, resolver);
                LoadMembers(declaration, resolver);
                LoadAttributes(declaration, resolver);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while loading {declaration.FullName}: {ex.Message}");

                // Make sure all collection are assigned.
                declaration.GenericParameters ??= Array.Empty<IGenericParameterDeclaration>();
                declaration.Extends ??= Array.Empty<IDeclarationUse<SyntaxNode>>();
                declaration.Members ??= Array.Empty<IMemberDeclaration<SyntaxNode>>();
                declaration.Attributes ??= Array.Empty<IAttributeUse>();

                throw;
            }
        }

        internal override ISyntaxNodeProvider<TypeParameterListSyntax> GetTypeParameterListSyntaxProvider(
            AGenericDeclaration<TNode> declaration)
        {
            return new ParserSyntaxNodeProvider<TypeParameterListSyntax>(
                declaration.SyntaxNodeProvider.SyntaxNode.TypeParameterList);
        }

        /// <summary>
        /// Load the generic parameters from the type parameter list node.
        /// </summary>
        private static void LoadGenericParameters(AGenericDeclaration<TNode> declaration)
        {
            var parameterList = declaration.SyntaxNodeProvider.SyntaxNode.TypeParameterList;
            if (parameterList != null)
            {
                var parameterSet = new List<GenericParameterDeclaration>();
                foreach (var parameter in parameterList.Parameters)
                {
                    parameterSet.Add(new GenericParameterDeclaration(
                        parameter.Identifier.Text,
                        new ParserSyntaxNodeProvider<TypeParameterSyntax>(parameter)));
                }

                declaration.GenericParameters = parameterSet;

                var map = parameterSet.ToDictionary(x => x.Name);

                var constraintClauses = declaration.SyntaxNodeProvider.SyntaxNode.ConstraintClauses;

                foreach (var constraintClause in constraintClauses)
                {
                    var parameterName = constraintClause.Name.Identifier.Text;

                    var genericParameterDeclaration = map[parameterName];

                    foreach (var constraint in constraintClause.Constraints)
                    {
                        // TODO Process all constraints.

                        if (constraint.Kind() == SyntaxKind.StructConstraint)
                        {
                            genericParameterDeclaration.SetValueType(true);
                        }
                    }
                }
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
        private static void LoadExtends(
            AGenericDeclaration<TNode> declaration,
            IDeclarationResolver resolver)
        {
            var baseListSyntax = declaration.SyntaxNodeProvider.SyntaxNode.BaseList;

            if (baseListSyntax != null)
            {
                var baseWalker = new DeclarationUseWalker(resolver, declaration);
                var uses = new List<IDeclarationUse<SyntaxNode>>();

                foreach (var node in baseListSyntax.ChildNodes())
                {
                    var use = baseWalker.Visit(node);

                    if (use == null)
                    {
                        throw new ParserException("Unable to load Declaration use.", node);
                    }

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
        private static void LoadMembers(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var memberList = new List<IMemberDeclaration<SyntaxNode>>();
            var membersWalker = new MembersWalker(resolver, declaration, memberList);

            membersWalker.Visit(declaration.SyntaxNodeProvider.SyntaxNode);

            declaration.Members = memberList.Count > 0 ? memberList.ToArray() : Array.Empty<IMemberDeclaration<SyntaxNode>>();
        }

        private static void LoadAttributes(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver)
        {
            var attributeList = new List<IAttributeUse>();
            var attributesWalker = new AttributesWalker(resolver, declaration, attributeList);

            attributesWalker.Visit(declaration.SyntaxNodeProvider.SyntaxNode);

            declaration.Attributes = attributeList.Count > 0 ? attributeList.ToArray() : Array.Empty<IAttributeUse>();
        }
    }
}
