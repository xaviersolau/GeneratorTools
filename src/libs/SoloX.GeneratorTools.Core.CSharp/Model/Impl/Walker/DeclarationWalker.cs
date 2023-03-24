// ----------------------------------------------------------------------
// <copyright file="DeclarationWalker.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
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

        private readonly IParserDeclarationFactory declarationFactory;

        private readonly IList<IDeclaration<SyntaxNode>> declarations;

        private readonly string location;

        private readonly IGlobalUsingDirectives globalUsing;

        public DeclarationWalker(IParserDeclarationFactory declarationFactory, IList<IDeclaration<SyntaxNode>> declarations, string location, IGlobalUsingDirectives globalUsing)
        {
            this.declarationFactory = declarationFactory;
            this.declarations = declarations;
            this.location = location;
            this.globalUsing = globalUsing;
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var currentUsingDirectives = this.usingDirectives.Peek();

            if (node.GlobalKeyword != null && node.GlobalKeyword.Kind() == SyntaxKind.GlobalKeyword)
            {
                this.globalUsing.Register(node.Name.ToString());
            }
            else
            {
                currentUsingDirectives.Add(node.Name.ToString());
            }

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

            var interfaceDeclaration = this.declarationFactory.CreateInterfaceDeclaration(
                currentNameSpace,
                new CSharpUsingDirectives(this.globalUsing, currentUsingDirectives),
                node,
                this.location);

            this.declarations.Add(interfaceDeclaration);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var classDeclaration = this.declarationFactory.CreateClassDeclaration(
                currentNameSpace,
                new CSharpUsingDirectives(this.globalUsing, currentUsingDirectives),
                node,
                this.location);

            this.declarations.Add(classDeclaration);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var structDeclaration = this.declarationFactory.CreateStructDeclaration(
                currentNameSpace,
                new CSharpUsingDirectives(this.globalUsing, currentUsingDirectives),
                node,
                this.location);

            this.declarations.Add(structDeclaration);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            var enumDeclaration = this.declarationFactory.CreateEnumDeclaration(
                currentNameSpace,
                new CSharpUsingDirectives(this.globalUsing, currentUsingDirectives),
                node,
                this.location);

            this.declarations.Add(enumDeclaration);
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            var currentNameSpace = this.nameSpace.Peek();
            var currentUsingDirectives = this.usingDirectives.Peek();

            IDeclaration<SyntaxNode> recordDeclaration = node.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword)
                ? this.declarationFactory.CreateRecordStructDeclaration(
                    currentNameSpace,
                    new CSharpUsingDirectives(this.globalUsing, currentUsingDirectives),
                    node,
                    this.location)
                : this.declarationFactory.CreateRecordDeclaration(
                    currentNameSpace,
                    new CSharpUsingDirectives(this.globalUsing, currentUsingDirectives),
                    node,
                    this.location);

            this.declarations.Add(recordDeclaration);
        }
    }
}
