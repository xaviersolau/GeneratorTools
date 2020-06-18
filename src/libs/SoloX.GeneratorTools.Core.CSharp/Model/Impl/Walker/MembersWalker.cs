// ----------------------------------------------------------------------
// <copyright file="MembersWalker.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker
{
    internal class MembersWalker : CSharpSyntaxWalker
    {
        private readonly IDeclarationResolver resolver;
        private readonly List<IMemberDeclaration<SyntaxNode>> memberList;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;

        public MembersWalker(
            IDeclarationResolver resolver,
            IGenericDeclaration<SyntaxNode> genericDeclaration,
            List<IMemberDeclaration<SyntaxNode>> memberList)
        {
            this.resolver = resolver;
            this.memberList = memberList;
            this.genericDeclaration = genericDeclaration;
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var identifier = node.Identifier.ToString();

            var useWalker = new DeclarationUseWalker(this.resolver, this.genericDeclaration);
            var use = useWalker.Visit(node.Type);

            var canRead = node.AccessorList.Accessors.FirstOrDefault(a => a.Kind() == SyntaxKind.GetAccessorDeclaration) != null;
            var canWrite = node.AccessorList.Accessors.LastOrDefault(a => a.Kind() == SyntaxKind.SetAccessorDeclaration) != null;

            this.memberList.Add(new PropertyDeclaration(
                identifier,
                use,
                new ParserSyntaxNodeProvider<PropertyDeclarationSyntax>(node),
                canRead,
                canWrite));
        }
    }
}
