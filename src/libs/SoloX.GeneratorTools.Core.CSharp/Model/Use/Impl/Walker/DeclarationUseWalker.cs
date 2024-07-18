// ----------------------------------------------------------------------
// <copyright file="DeclarationUseWalker.cs" company="Xavier Solau">
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
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker
{
    internal class DeclarationUseWalker : CSharpSyntaxVisitor<IDeclarationUse<SyntaxNode>>
    {
        private readonly IDeclarationResolver resolver;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;
        private string currentQualifiedName = string.Empty;

        public DeclarationUseWalker(IDeclarationResolver resolver, IGenericDeclaration<SyntaxNode> genericDeclaration)
        {
            this.resolver = resolver;
            this.genericDeclaration = genericDeclaration;
        }

        public override IDeclarationUse<SyntaxNode> VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            return this.Visit(node.Type);
        }

        public override IDeclarationUse<SyntaxNode> VisitGenericName(GenericNameSyntax node)
        {
            var identifier = node.Identifier.Text;
            var fullIdentifier = MakeFullQualifiedName(identifier);

            var tparams = new List<IDeclarationUse<SyntaxNode>>();
            foreach (var item in node.TypeArgumentList.Arguments)
            {
                tparams.Add(item.Accept(this));
            }

            var genericDeclaration = this.resolver.Resolve(fullIdentifier, tparams, this.genericDeclaration);

            if (genericDeclaration != null)
            {
                return new GenericDeclarationUse(
                    new ParserSyntaxNodeProvider<GenericNameSyntax>(node),
                    genericDeclaration,
                    tparams);
            }

            return new UnknownGenericDeclarationUse(
                new ParserSyntaxNodeProvider<GenericNameSyntax>(node),
                new UnknownDeclaration(this.currentQualifiedName, identifier),
                tparams);
        }

        private string MakeFullQualifiedName(string identifier)
        {
            return string.IsNullOrEmpty(this.currentQualifiedName)
                ? identifier
                : this.currentQualifiedName + '.' + identifier;
        }

        public override IDeclarationUse<SyntaxNode> VisitQualifiedName(QualifiedNameSyntax node)
        {
            var left = node.Left;

            this.currentQualifiedName = left.ToString();

            var right = node.Right;

            var use = Visit(right);

            this.currentQualifiedName = string.Empty;

            return use;
        }

        public override IDeclarationUse<SyntaxNode> VisitIdentifierName(IdentifierNameSyntax node)
        {
            var identifier = node.Identifier.Text;
            var fullIdentifier = MakeFullQualifiedName(identifier);

            IDeclaration<SyntaxNode> declaration = this.genericDeclaration?.GenericParameters
                .FirstOrDefault(p => p.Name == fullIdentifier);
            if (declaration != null)
            {
                return new GenericParameterDeclarationUse(
                    new ParserSyntaxNodeProvider<IdentifierNameSyntax>(node),
                    declaration);
            }

            declaration = this.resolver.Resolve(fullIdentifier, this.genericDeclaration);

            if (declaration != null)
            {
                if (declaration is IGenericDeclaration<SyntaxNode> genericDeclaration)
                {
                    return new GenericDeclarationUse(
                        new ParserSyntaxNodeProvider<IdentifierNameSyntax>(node),
                        genericDeclaration,
                        Array.Empty<IDeclarationUse<SyntaxNode>>());
                }
                else
                {
                    return new BasicDeclarationUse(new ParserSyntaxNodeProvider<IdentifierNameSyntax>(node), declaration);
                }
            }

            return new UnknownDeclarationUse(
                new ParserSyntaxNodeProvider<IdentifierNameSyntax>(node),
                new UnknownDeclaration(this.currentQualifiedName, identifier));
        }

        public override IDeclarationUse<SyntaxNode> VisitArrayType(ArrayTypeSyntax node)
        {
            var elementDeclarationUse = this.Visit(node.ElementType);

            ((IArrayDeclarationUseImpl)elementDeclarationUse).ArraySpecification = new ArraySpecification(
                node.RankSpecifiers.Count,
                new ParserSyntaxNodeProvider<ArrayTypeSyntax>(node));

            return elementDeclarationUse;
        }

        public override IDeclarationUse<SyntaxNode> VisitPredefinedType(PredefinedTypeSyntax node)
        {
            return new PredefinedDeclarationUse(
                new ParserSyntaxNodeProvider<PredefinedTypeSyntax>(node),
                node.Keyword.Text);
        }

        public override IDeclarationUse<SyntaxNode> VisitNullableType(NullableTypeSyntax node)
        {
            var declarationUse = this.Visit(node.ElementType);

            if (declarationUse.Declaration.IsValueType)
            {
                var tparams = new[] { declarationUse };
                var genericDeclaration = this.resolver.Resolve("System.Nullable", tparams, this.genericDeclaration);

                // TODO Set ParserSyntaxNodeProvider
                return new GenericDeclarationUse(
                    //new ParserSyntaxNodeProvider<NullableTypeSyntax>(null),
                    null,
                    genericDeclaration,
                    tparams);
            }

            return declarationUse;
        }

        public override IDeclarationUse<SyntaxNode> VisitPrimaryConstructorBaseType(PrimaryConstructorBaseTypeSyntax node)
        {
            return Visit(node.Type);
        }
    }
}
