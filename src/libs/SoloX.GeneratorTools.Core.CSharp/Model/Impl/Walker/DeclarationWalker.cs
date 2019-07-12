// ----------------------------------------------------------------------
// <copyright file="DeclarationWalker.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker
{
    internal class DeclarationWalker : CSharpSyntaxWalker
    {
        private readonly Stack<string> nameSpace =
            new Stack<string>(new[] { string.Empty });

        private readonly Stack<List<string>> usingDirectives =
            new Stack<List<string>>(new[] { new List<string>() });

        private readonly IList<IDeclaration<SyntaxNode>> declarations;

        private readonly string location;

        public DeclarationWalker(IList<IDeclaration<SyntaxNode>> declarations, string location)
        {
            this.declarations = declarations;
            this.location = location;
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var currentUsingDirectives = this.usingDirectives.Peek();

            currentUsingDirectives.Add(node.Name.ToString());

            base.VisitUsingDirective(node);
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();

            this.nameSpace.Push(ADeclaration<SyntaxNode>.GetFullName(currentNameSpace, node.Name.ToString()));

            var currentUsingDirectives = this.usingDirectives.Peek();
            this.usingDirectives.Push(new List<string>(currentUsingDirectives));

            base.VisitNamespaceDeclaration(node);

            this.usingDirectives.Pop();
            this.nameSpace.Pop();
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var interfaceDeclaration = new InterfaceDeclaration(
                currentNameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<InterfaceDeclarationSyntax>(node),
                currentUsingDirectives,
                this.location,
                ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>.Shared);

            this.declarations.Add(interfaceDeclaration);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var classDeclaration = new ClassDeclaration(
                currentNameSpace,
                node.Identifier.ToString(),
                new ParserSyntaxNodeProvider<ClassDeclarationSyntax>(node),
                currentUsingDirectives,
                this.location,
                ParserGenericDeclarationLoader<ClassDeclarationSyntax>.Shared);

            this.declarations.Add(classDeclaration);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var structDeclaration = new StructDeclaration(
                currentNameSpace,
                node.Identifier.ToString(),
                new ParserSyntaxNodeProvider<StructDeclarationSyntax>(node),
                currentUsingDirectives,
                this.location,
                ParserGenericDeclarationLoader<StructDeclarationSyntax>.Shared);

            this.declarations.Add(structDeclaration);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var enumDeclaration = new EnumDeclaration(
                currentNameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<EnumDeclarationSyntax>(node),
                currentUsingDirectives,
                this.location);

            this.declarations.Add(enumDeclaration);
        }
    }
}
