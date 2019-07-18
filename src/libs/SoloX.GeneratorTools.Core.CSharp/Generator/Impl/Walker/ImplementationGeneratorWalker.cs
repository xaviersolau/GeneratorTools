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
        private readonly IGenericDeclaration<SyntaxNode> implPattern;
        private readonly IInterfaceDeclaration declaration;
        private readonly string implName;
        private readonly string implNameSpace;
        private readonly IWriterSelector writerSelector;

        private bool isPackStatements = false;

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
            IGenericDeclaration<SyntaxNode> implPattern,
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
            if (!this.writerSelector.SelectAndProcessWriter(node, this.Write))
            {
                var txt = node.ToFullString()
                    .Replace(this.implPattern.Name, this.implName)
                    .Replace(this.itfPattern.DeclarationNameSpace, this.declaration.DeclarationNameSpace);

                if (!txt.Contains("SoloX.GeneratorTools.Core.CSharp.Generator.Attributes"))
                {
                    this.Write(txt);
                }
            }
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

            if (node.BaseList != null)
            {
                this.Write(node.BaseList.ToFullString()
                    .Replace(this.itfPattern.Name, this.declaration.Name));
            }

            this.Write(node.ConstraintClauses.ToFullString());

            this.Write(node.OpenBraceToken.ToFullString());

            foreach (var member in node.Members)
            {
                this.Visit(member);
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            var previousIsPackStatement = this.isPackStatements;
            try
            {
                this.WriteAttributeLists(node.AttributeLists, out this.isPackStatements);
                this.Write(node.Modifiers.ToFullString());
                this.WriteToken(node.Identifier);
                this.WriteNode(node.ParameterList);
                this.Visit(node.Body);
            }
            finally
            {
                this.isPackStatements = previousIsPackStatement;
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var previousIsPackStatement = this.isPackStatements;
            try
            {
                this.WriteAttributeLists(node.AttributeLists, out this.isPackStatements);

                this.Write(node.Modifiers.ToFullString());
                this.WriteNode(node.ReturnType);

                this.WriteNode(node.ExplicitInterfaceSpecifier);

                this.WriteToken(node.Identifier);

                this.WriteNode(node.TypeParameterList);
                this.WriteNode(node.ParameterList);
                foreach (var constraintClauses in node.ConstraintClauses)
                {
                    this.WriteNode(constraintClauses);
                }

                this.Visit(node.Body);
                this.Visit(node.ExpressionBody);
                this.Write(node.SemicolonToken.ToFullString());
            }
            finally
            {
                this.isPackStatements = previousIsPackStatement;
            }
        }

        public override void VisitBlock(BlockSyntax node)
        {
            this.Write(node.OpenBraceToken.ToFullString());
            foreach (var statement in node.Statements)
            {
                this.Visit(statement);
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitThrowStatement(ThrowStatementSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            if (this.isPackStatements)
            {
                this.WriteNode(node);
            }
            else
            {
                this.Write(node.IfKeyword.ToFullString());
                this.Write(node.OpenParenToken.ToFullString());
                this.WriteNode(node.Condition);
                this.Write(node.CloseParenToken.ToFullString());
                this.Visit(node.Statement);
                if (node.Else != null)
                {
                    this.Visit(node.Else);
                }
            }
        }

        public override void VisitElseClause(ElseClauseSyntax node)
        {
            this.Write(node.ElseKeyword.ToFullString());
            this.Visit(node.Statement);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            if (this.isPackStatements)
            {
                this.WriteNode(node);
            }
            else
            {
                this.Write(node.ForEachKeyword.ToFullString());
                this.Write(node.OpenParenToken.ToFullString());
                this.WriteNode(node.Type);
                this.Write(node.Identifier.ToFullString());
                this.Write(node.InKeyword.ToFullString());
                this.WriteNode(node.Expression);
                this.Write(node.CloseParenToken.ToFullString());
                this.Visit(node.Statement);
            }
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            if (this.isPackStatements)
            {
                this.WriteNode(node);
            }
            else
            {
                this.Write(node.ForKeyword.ToFullString());
                this.Write(node.OpenParenToken.ToFullString());
                this.WriteNode(node.Declaration);
                this.Write(node.FirstSemicolonToken.ToFullString());
                this.WriteNode(node.Condition);
                this.Write(node.SecondSemicolonToken.ToFullString());
                foreach (var incrementor in node.Incrementors)
                {
                    this.WriteNode(incrementor);
                }

                this.Write(node.CloseParenToken.ToFullString());
                this.Visit(node.Statement);
            }
        }

        private static string SameText(string s) => s;

        private void WriteNode(SyntaxNode node)
        {
            if (node != null)
            {
                if (!this.writerSelector.SelectAndProcessWriter(node, this.Write))
                {
                    this.Write(node.ToFullString());
                }
            }
        }

        private void WriteToken(SyntaxToken token)
        {
            if (token != null)
            {
                if (!this.writerSelector.SelectAndProcessWriter(token, this.Write))
                {
                    this.Write(token.ToFullString());
                }
            }
        }

        private void Write(string text)
        {
            this.writer.Write(this.textSubstitutionHandler(text));
        }

        private void WriteAttributeLists(SyntaxList<AttributeListSyntax> attributeLists, out bool isPackStatements)
        {
            isPackStatements = false;

            if (attributeLists != null)
            {
                foreach (var attrList in attributeLists)
                {
                    bool found = false;
                    foreach (var attr in attrList.Attributes)
                    {
                        var attrName = attr.Name.ToString();
                        if (attrName == "PackStatements" || attrName == "PackStatementsAttribute")
                        {
                            isPackStatements = true;
                        }
                        else
                        {
                            if (!found)
                            {
                                this.Write(attrList.OpenBracketToken.ToFullString());
                                found = true;
                            }

                            this.WriteNode(attr);
                        }
                    }

                    if (found)
                    {
                        this.Write(attrList.CloseBracketToken.ToFullString());
                    }
                    else
                    {
                        var line = attrList.OpenBracketToken.ToFullString().Replace("[", string.Empty)
                            + attrList.CloseBracketToken.ToFullString().Replace("]", string.Empty);
                        this.Write(line.Replace("        \r\n", string.Empty));
                    }
                }
            }
        }
    }
}
