// ----------------------------------------------------------------------
// <copyright file="DeclarationWalker.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker
{
    internal class DeclarationWalker : CSharpSyntaxWalker
    {
        private readonly Stack<string> nameSpace =
            new Stack<string>(new[] { string.Empty });

        private readonly Stack<List<string>> usingDirectives =
            new Stack<List<string>>(new[] { new List<string>() });

        private IList<IDeclaration> declarations;

        public DeclarationWalker(IList<IDeclaration> declarations)
        {
            this.declarations = declarations;
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

            this.nameSpace.Push(ADeclaration.GetFullName(currentNameSpace, node.Name.ToString()));

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

            var interfaceDeclaration = new InterfaceDeclaration(currentNameSpace, node, currentUsingDirectives);

            this.declarations.Add(interfaceDeclaration);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var classDeclaration = new ClassDeclaration(currentNameSpace, node, currentUsingDirectives);

            this.declarations.Add(classDeclaration);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var structDeclaration = new StructDeclaration(currentNameSpace, node, currentUsingDirectives);

            this.declarations.Add(structDeclaration);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var enumDeclaration = new EnumDeclaration(currentNameSpace, node, currentUsingDirectives);

            this.declarations.Add(enumDeclaration);
        }
    }
}
