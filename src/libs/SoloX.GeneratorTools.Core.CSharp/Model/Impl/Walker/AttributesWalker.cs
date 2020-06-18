// ----------------------------------------------------------------------
// <copyright file="AttributesWalker.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
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
            var identifier = node.Name.ToString();

            if (!identifier.EndsWith(nameof(Attribute), StringComparison.InvariantCulture))
            {
                identifier = $"{identifier}{nameof(Attribute)}";
            }

            var syntaxNodeProvider = new ParserSyntaxNodeProvider<AttributeSyntax>(node);

            var attributeDeclaration = this.resolver.Resolve(
                identifier,
                Array.Empty<IDeclarationUse<SyntaxNode>>(),
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
                    new UnknownDeclaration(identifier),
                    syntaxNodeProvider));
            }
        }
    }
}
