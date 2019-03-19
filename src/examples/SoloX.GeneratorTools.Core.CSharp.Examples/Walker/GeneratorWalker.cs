// ----------------------------------------------------------------------
// <copyright file="GeneratorWalker.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Walker
{
    internal class GeneratorWalker : CSharpSyntaxWalker
    {
        private readonly TextWriter writer;
        private readonly Func<string, string> textSubstitutionHandler;
        private readonly IInterfaceDeclaration itfPattern;
        private readonly IGenericDeclaration implPattern;
        private readonly IInterfaceDeclaration declaration;
        private readonly string implName;

        public GeneratorWalker(
            TextWriter writer,
            IInterfaceDeclaration itfPattern,
            IGenericDeclaration implPattern,
            IInterfaceDeclaration declaration,
            string implName,
            Func<string, string> textSubstitutionHandler = null)
        {
            this.writer = writer;
            this.itfPattern = itfPattern;
            this.implPattern = implPattern;
            this.declaration = declaration;
            this.implName = implName;
            this.textSubstitutionHandler = textSubstitutionHandler ?? SameText;
        }

        public override void Visit(SyntaxNode node)
        {
            base.Visit(node);
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var txt = node.ToFullString()
                .Replace(this.implPattern.Name, this.implName, StringComparison.InvariantCulture)
                .Replace(this.itfPattern.DeclarationNameSpace, this.declaration.DeclarationNameSpace, StringComparison.InvariantCulture);

            this.Write(txt);
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            this.Write(node.NamespaceKeyword.ToFullString());
            this.Write(node.Name.ToFullString());
            this.Write(node.OpenBraceToken.ToFullString());
            this.Write(node.Usings.ToFullString());

            foreach (var member in node.Members)
            {
                this.Visit(member);
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            this.Write(node.AttributeLists.ToFullString());
            this.Write(node.Modifiers.ToFullString());
            this.Write(node.Keyword.ToFullString());
            this.Write(node.Identifier.ToFullString()
                .Replace(this.implPattern.Name, this.implName, StringComparison.InvariantCulture));

            if (node.TypeParameterList != null)
            {
                this.Write(node.TypeParameterList.ToFullString());
            }

            this.Write(node.BaseList.ToFullString()
                .Replace(this.itfPattern.Name, this.declaration.Name, StringComparison.InvariantCulture));
            this.Write(node.ConstraintClauses.ToFullString());

            this.Write(node.OpenBraceToken.ToFullString());

            foreach (var member in node.Members)
            {
                this.Visit(member);
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var itfProperty = this.itfPattern.Members.Single();
            var propertyName = itfProperty.Name;

            if (node.Identifier.Text == propertyName)
            {
                var txt = node.ToFullString();
                foreach (var item in this.declaration.Members)
                {
                    this.Write(txt
                        .Replace(propertyName, item.Name, StringComparison.InvariantCulture));
                }
            }
            else
            {
                this.Write(node.ToFullString());
            }
        }

        private static string SameText(string s) => s;

        private void Write(string text)
        {
            this.writer.Write(this.textSubstitutionHandler(text));
        }
    }
}
