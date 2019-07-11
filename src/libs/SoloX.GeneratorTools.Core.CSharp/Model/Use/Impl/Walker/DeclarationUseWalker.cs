﻿// ----------------------------------------------------------------------
// <copyright file="DeclarationUseWalker.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker
{
    internal class DeclarationUseWalker : CSharpSyntaxVisitor<IDeclarationUse<SyntaxNode>>
    {
        private readonly IDeclarationResolver resolver;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;

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

            var tparams = new List<IDeclarationUse<SyntaxNode>>();
            foreach (var item in node.TypeArgumentList.Arguments)
            {
                tparams.Add(item.Accept(this));
            }

            var genericDeclaration = this.resolver.Resolve(identifier, tparams, this.genericDeclaration);

            if (genericDeclaration != null)
            {
                return new GenericDeclarationUse(node, genericDeclaration, tparams);
            }

            return new UnknownGenericDeclarationUse(node, new UnknownDeclaration(identifier), tparams);
        }

        public override IDeclarationUse<SyntaxNode> VisitIdentifierName(IdentifierNameSyntax node)
        {
            var identifier = node.Identifier.Text;

            IDeclaration<SyntaxNode> declaration = this.genericDeclaration.GenericParameters.FirstOrDefault(p => p.Name == identifier);
            if (declaration != null)
            {
                return new GenericParameterDeclarationUse(node, declaration);
            }

            declaration = this.resolver.Resolve(identifier, this.genericDeclaration);

            if (declaration != null)
            {
                if (declaration is IGenericDeclaration<SyntaxNode> genericDeclaration)
                {
                    return new GenericDeclarationUse(node, genericDeclaration, Array.Empty<IDeclarationUse<SyntaxNode>>());
                }
                else
                {
                    return new BasicDeclarationUse(node, declaration);
                }
            }

            return new UnknownDeclarationUse(node, new UnknownDeclaration(identifier));
        }

        public override IDeclarationUse<SyntaxNode> VisitArrayType(ArrayTypeSyntax node)
        {
            var elementDeclarationUse = this.Visit(node.ElementType);

            ((IArrayDeclarationUseImpl)elementDeclarationUse).ArraySpecification = new ArraySpecification(node.RankSpecifiers);

            return elementDeclarationUse;
        }

        public override IDeclarationUse<SyntaxNode> VisitPredefinedType(PredefinedTypeSyntax node)
        {
            return new PredefinedDeclarationUse(node, node.Keyword.Text);
        }
    }
}
