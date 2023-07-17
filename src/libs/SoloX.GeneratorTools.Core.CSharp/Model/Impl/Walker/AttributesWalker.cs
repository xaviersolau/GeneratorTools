// ----------------------------------------------------------------------
// <copyright file="AttributesWalker.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker
{
    internal class AttributesWalker : CSharpSyntaxWalker
    {
        private readonly IDeclarationResolver resolver;
        private readonly List<IAttributeUse> attributesList;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;

        private string? identifier;
        private IReadOnlyList<IDeclarationUse<SyntaxNode>> genericAttributeArguments = Array.Empty<IDeclarationUse<SyntaxNode>>();

        public AttributesWalker(
            IDeclarationResolver resolver,
            IGenericDeclaration<SyntaxNode> genericDeclaration,
            List<IAttributeUse> attributesList)
        {
            this.resolver = resolver;
            this.attributesList = attributesList;
            this.genericDeclaration = genericDeclaration;
        }

        public override void VisitAttribute(AttributeSyntax node)
        {
            this.identifier = null;
            this.genericAttributeArguments = Array.Empty<IDeclarationUse<SyntaxNode>>();

            this.Visit(node.Name);

            var syntaxNodeProvider = new ParserSyntaxNodeProvider<AttributeSyntax>(node);

            var attributeDeclaration = this.resolver.Resolve(
                this.identifier,
                this.genericAttributeArguments,
                this.genericDeclaration);

            if (attributeDeclaration != null)
            {
                this.attributesList.Add(new AttributeUse(
                    attributeDeclaration,
                    syntaxNodeProvider));
            }
            else
            {
                this.attributesList.Add(new UnknownAttributeUse(
                    new UnknownDeclaration(this.identifier),
                    syntaxNodeProvider));
            }
        }

        public override void VisitGenericName(GenericNameSyntax node)
        {
            LoadIdentifierToken(node.Identifier);

            var useWalker = new DeclarationUseWalker(this.resolver, this.genericDeclaration);

            var typeArgumentes = new List<IDeclarationUse<SyntaxNode>>();

            foreach (var typeArgument in node.TypeArgumentList.Arguments)
            {
                var use = useWalker.Visit(typeArgument);

                typeArgumentes.Add(use);
            }

            this.genericAttributeArguments = typeArgumentes;
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            var identifierToken = node.Identifier;
            LoadIdentifierToken(identifierToken);
        }

        private void LoadIdentifierToken(SyntaxToken identifierToken)
        {
            var identifier = identifierToken.ToString();

            if (!identifier.EndsWith(nameof(Attribute), StringComparison.InvariantCulture))
            {
                identifier = $"{identifier}{nameof(Attribute)}";
            }

            this.identifier = identifier;
        }
    }
}
