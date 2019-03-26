// ----------------------------------------------------------------------
// <copyright file="ImplementationGeneratorWalker.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.Generator.Writer;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    /// <summary>
    /// Generator walker that generates the given interface declaration implementation using a interface and implementation pattern.
    /// </summary>
    internal class ImplementationGeneratorWalker : CSharpSyntaxWalker
    {
        private readonly TextWriter writer;
        private readonly Func<string, string> textSubstitutionHandler;
        private readonly IInterfaceDeclaration itfPattern;
        private readonly IGenericDeclaration implPattern;
        private readonly IInterfaceDeclaration declaration;
        private readonly string implName;
        private readonly string implNameSpace;
        private readonly IWriterSelector writerSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementationGeneratorWalker"/> class.
        /// </summary>
        /// <param name="writer">The writer where to write generated code.</param>
        /// <param name="itfPattern">Interface pattern.</param>
        /// <param name="implPattern">Implementation pattern.</param>
        /// <param name="itfDeclaration">Interface declaration to implement.</param>
        /// <param name="implName">Implementation name.</param>
        /// <param name="implNameSpace">Implementation name space.</param>
        /// <param name="writerSelector">Writer selector.</param>
        /// <param name="textSubstitutionHandler">Optional text substitution handler.</param>
        public ImplementationGeneratorWalker(
            TextWriter writer,
            IInterfaceDeclaration itfPattern,
            IGenericDeclaration implPattern,
            IInterfaceDeclaration itfDeclaration,
            string implName,
            string implNameSpace,
            IWriterSelector writerSelector,
            Func<string, string> textSubstitutionHandler = null)
        {
            this.writer = writer;
            this.itfPattern = itfPattern;
            this.implPattern = implPattern;
            this.declaration = itfDeclaration;
            this.implName = implName;
            this.implNameSpace = implNameSpace;
            this.writerSelector = writerSelector;
            this.textSubstitutionHandler = textSubstitutionHandler ?? SameText;
        }

        /// <inheritdoc/>
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var txt = node.ToFullString()
                .Replace(this.implPattern.Name, this.implName)
                .Replace(this.itfPattern.DeclarationNameSpace, this.declaration.DeclarationNameSpace);

            this.Write(txt);
        }

        /// <inheritdoc/>
        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            this.Write(node.NamespaceKeyword.ToFullString());
            this.Write(node.Name.ToFullString()
                .Replace(this.implPattern.DeclarationNameSpace, this.implNameSpace));
            this.Write(node.OpenBraceToken.ToFullString());
            this.Write(node.Usings.ToFullString());

            foreach (var member in node.Members)
            {
                this.Visit(member);
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        /// <inheritdoc/>
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            this.Write(node.AttributeLists.ToFullString());
            this.Write(node.Modifiers.ToFullString());
            this.Write(node.Keyword.ToFullString());
            this.Write(node.Identifier.ToFullString()
                .Replace(this.implPattern.Name, this.implName));

            if (node.TypeParameterList != null)
            {
                this.Write(node.TypeParameterList.ToFullString());
            }

            this.Write(node.BaseList.ToFullString()
                .Replace(this.itfPattern.Name, this.declaration.Name));
            this.Write(node.ConstraintClauses.ToFullString());

            this.Write(node.OpenBraceToken.ToFullString());

            foreach (var member in node.Members)
            {
                if (!this.writerSelector.SelectAndProcessWriter(member, this.Write))
                {
                    this.Write(member.ToFullString());
                }
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        private static string SameText(string s) => s;

        private void Write(string text)
        {
            this.writer.Write(this.textSubstitutionHandler(text));
        }
    }
}
