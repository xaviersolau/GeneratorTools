// ----------------------------------------------------------------------
// <copyright file="MembersWalker.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker
{
    internal class MembersWalker : CSharpSyntaxWalker
    {
        private readonly IDeclarationResolver resolver;
        private readonly List<IMemberDeclaration> memberList;
        private readonly IGenericDeclaration genericDeclaration;

        public MembersWalker(IDeclarationResolver resolver, IGenericDeclaration genericDeclaration, List<IMemberDeclaration> memberList)
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

            this.memberList.Add(new PropertyDeclaration(identifier, use, node));
        }
    }
}
