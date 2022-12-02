// ----------------------------------------------------------------------
// <copyright file="AutomatedWalker.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedWalker : CSharpSyntaxWalker
    {
        private const string GeneratorAttributesNameSpace = "SoloX.GeneratorTools.Core.CSharp.Generator.Attributes";

        private readonly TextWriter textWriter;
        private readonly IDeclaration<SyntaxNode> pattern;
        private readonly IAutomatedStrategy strategy;

        private readonly IList<AttributeSyntax> subRepeatAttributes = new List<AttributeSyntax>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomatedWalker"/> class.
        /// </summary>
        /// <param name="textWriter">The writer where to write generated code.</param>
        /// <param name="pattern">The pattern reference.</param>
        /// <param name="strategy">Automated strategy that manage repeat feature.</param>
        public AutomatedWalker(
            TextWriter textWriter,
            IDeclaration<SyntaxNode> pattern,
            IAutomatedStrategy strategy)
        {
            this.textWriter = textWriter;
            this.pattern = pattern;
            this.strategy = strategy;
        }

        /// <inheritdoc/>
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var nameTxt = node.Name.ToString();

            if (GeneratorAttributesNameSpace.Equals(nameTxt, StringComparison.Ordinal))
            {
                var txt = node.ToFullString();

                this.strategy.RepeatNameSpace(
                    ns =>
                    {
                        this.Write(txt.Replace(GeneratorAttributesNameSpace, ns));
                    });
            }
            else
            {
                if (!nameTxt.StartsWith("SoloX.GeneratorTools.Core.CSharp.Generator", StringComparison.Ordinal) &&
                    !nameTxt.StartsWith("SoloX.GeneratorTools.Attributes", StringComparison.Ordinal) &&
                    !nameTxt.StartsWith("SoloX.GeneratorTools.Generator.Patterns", StringComparison.Ordinal) &&
                    !this.strategy.IgnoreUsingDirective(nameTxt))
                {
                    this.Write(node.ToFullString());
                }
            }
        }

        /// <inheritdoc/>
        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            this.Write(node.NamespaceKeyword.ToFullString());
            this.Write(node.Name.ToFullString()
                .Replace(this.pattern.DeclarationNameSpace, this.strategy.GetCurrentNameSpace()));
            this.Write(node.OpenBraceToken.ToFullString());

            foreach (var usingNode in node.Usings)
            {
                this.Visit(usingNode);
            }

            foreach (var member in node.Members)
            {
                this.Visit(member);
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        /// <inheritdoc/>
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
            {
                this.strategy.RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        new AutomatedWalker(this.textWriter, this.pattern, itemStrategy)
                            .WriteClassDeclaration(node);
                    });
            }
            else
            {
                this.WriteClassDeclaration(node);
            }
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (node.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
            {
                this.strategy.RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        new AutomatedWalker(this.textWriter, this.pattern, itemStrategy)
                            .WriteInterfaceDeclaration(node);
                    });
            }
            else
            {
                this.WriteInterfaceDeclaration(node);
            }
        }

        public override void VisitParameterList(ParameterListSyntax node)
        {
            this.WriteToken(node.OpenParenToken);

            var firstParameter = true;

            var tkns = node.ChildTokens().First().ToFullString().Replace("(", ", ");

            foreach (var parameter in node.Parameters)
            {
                if (parameter.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
                {
                    this.subRepeatAttributes.Add(attributeSyntax);

                    this.strategy.RepeatDeclaration(
                        attributeSyntax,
                        itemStrategy =>
                        {
                            if (firstParameter)
                            {
                                firstParameter = false;
                            }
                            else
                            {
                                this.Write(tkns);
                            }

                            new AutomatedWalker(this.textWriter, this.pattern, itemStrategy)
                                .WriteParameter(parameter);
                        });
                }
                else
                {
                    if (firstParameter)
                    {
                        firstParameter = false;
                    }
                    else
                    {
                        this.Write(tkns);
                    }

                    this.WriteParameter(parameter);
                }
            }

            this.WriteToken(node.CloseParenToken);
        }

        public override void VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            this.WriteToken(node.OpenBracketToken);

            var firstParameter = true;

            var tkns = node.ChildTokens().First().ToFullString().Replace("[", ", ");

            foreach (var parameter in node.Parameters)
            {
                if (parameter.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
                {
                    this.subRepeatAttributes.Add(attributeSyntax);

                    this.strategy.RepeatDeclaration(
                        attributeSyntax,
                        itemStrategy =>
                        {
                            if (firstParameter)
                            {
                                firstParameter = false;
                            }
                            else
                            {
                                this.Write(tkns);
                            }

                            new AutomatedWalker(this.textWriter, this.pattern, itemStrategy)
                                .WriteParameter(parameter);
                        });
                }
                else
                {
                    if (firstParameter)
                    {
                        firstParameter = false;
                    }
                    else
                    {
                        this.Write(tkns);
                    }

                    this.WriteParameter(parameter);
                }
            }

            this.WriteToken(node.CloseBracketToken);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (node.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
            {
                this.strategy.RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        new AutomatedWalker(this.textWriter, this.pattern, itemStrategy)
                            .WritePropertyDeclaration(node);
                    });
            }
            else
            {
                this.WritePropertyDeclaration(node);
            }
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            if (node.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
            {
                this.strategy.RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        new AutomatedWalker(this.textWriter, this.pattern, itemStrategy)
                            .WriteFieldDeclaration(node);
                    });
            }
            else
            {
                this.WriteFieldDeclaration(node);
            }
        }

        public override void VisitAccessorList(AccessorListSyntax node)
        {
            this.WriteToken(node.OpenBraceToken);

            foreach (var accessor in node.Accessors)
            {
                this.Visit(accessor);
            }

            this.WriteToken(node.CloseBraceToken);
        }

        public override void VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            this.Write(node.Modifiers.ToFullString());
            this.WriteToken(node.Keyword);
            this.Visit(node.Body);
            this.Visit(node.ExpressionBody);
            this.WriteToken(node.SemicolonToken);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);
            this.Write(node.Modifiers.ToFullString());
            this.WriteToken(node.Identifier);
            this.WriteNode(node.ParameterList);
            this.Visit(node.Body);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
            {
                this.strategy.RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        new AutomatedWalker(this.textWriter, this.pattern, itemStrategy)
                            .WriteMethodDeclaration(node);
                    });
            }
            else
            {
                this.WriteMethodDeclaration(node);
            }
        }

        public override void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            if (node.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
            {
                this.strategy.RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        new AutomatedWalker(this.textWriter, this.pattern, itemStrategy)
                            .WriteIndexerDeclaration(node);
                    });
            }
            else
            {
                this.WriteIndexerDeclaration(node);
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

        public override void VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            this.WriteToken(node.ArrowToken);

            this.Visit(node.Expression);
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            this.WriteToken(node.ReturnKeyword);
            this.Visit(node.Expression);
            this.WriteToken(node.SemicolonToken);
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            this.WriteToken(node.NewKeyword);
            this.WriteNode(node.Type);
            if (node.ArgumentList != null)
            {
                this.WriteToken(node.ArgumentList.OpenParenToken);
                foreach (var argument in node.ArgumentList.Arguments)
                {
                    this.WriteNode(argument);
                }

                this.WriteToken(node.ArgumentList.CloseParenToken);
            }

            if (node.Initializer != null)
            {
                this.WriteToken(node.Initializer.OpenBraceToken);

                var tknList = node.Initializer.ChildTokens();
#pragma warning disable CA1851 // Possible multiple enumerations of 'IEnumerable' collection
                var tkns = tknList.Count() > 2 ? tknList.ElementAt(1).ToFullString() : ", ";
#pragma warning restore CA1851 // Possible multiple enumerations of 'IEnumerable' collection

                foreach (var expression in node.Initializer.Expressions)
                {
                    var textExpression = expression.ToFullString();

                    if (this.TryMatchSubRepeatAttribute(out var attributeSyntax, textExpression))
                    {
                        this.strategy.RepeatDeclaration(
                            attributeSyntax,
                            itemStrategy =>
                            {
                                this.textWriter.Write(itemStrategy.ApplyPatternReplace(textExpression));

                                this.Write(tkns);
                            });
                    }
                    else
                    {
                        this.WriteNode(expression);

                        this.Write(tkns);
                    }
                }

                this.WriteToken(node.Initializer.CloseBraceToken);
            }
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            this.WriteToken(node.Identifier);
            this.Visit(node.Initializer);
        }

        public override void VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            this.WriteToken(node.EqualsToken);
            this.WriteNode(node.Value);
        }

        public override void VisitArgumentList(ArgumentListSyntax node)
        {
            this.WriteToken(node.OpenParenToken);

            var isFirst = true;

            foreach (var argument in node.Arguments)
            {
                var textExpression = argument.ToFullString();
                if (this.TryMatchSubRepeatAttribute(out var attributeSyntax, textExpression))
                {
                    this.strategy.RepeatDeclaration(
                        attributeSyntax,
                        itemStrategy =>
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                            }
                            else
                            {
                                this.Write(", ");
                            }

                            new AutomatedWalker(this.textWriter, this.pattern, itemStrategy).Visit(argument);
                        });
                }
                else
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        this.Write(", ");
                    }

                    this.Visit(argument);
                }
            }

            this.WriteToken(node.CloseParenToken);
        }

        public override void VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            this.WriteToken(node.OpenBracketToken);

            var isFirst = true;

            foreach (var argument in node.Arguments)
            {
                var textExpression = argument.ToFullString();
                if (this.TryMatchSubRepeatAttribute(out var attributeSyntax, textExpression))
                {
                    this.strategy.RepeatDeclaration(
                        attributeSyntax,
                        itemStrategy =>
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                            }
                            else
                            {
                                this.Write(", ");
                            }

                            new AutomatedWalker(this.textWriter, this.pattern, itemStrategy).Visit(argument);
                        });
                }
                else
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        this.Write(", ");
                    }

                    this.Visit(argument);
                }
            }

            this.WriteToken(node.CloseBracketToken);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            this.WriteToken(node.Identifier);
        }

        private bool TryMatchSubRepeatAttribute(out AttributeSyntax attributeSyntax, string expression)
        {
            attributeSyntax = null;
            foreach (var subRepeatAttribute in this.subRepeatAttributes)
            {
                if (this.strategy.TryMatchRepeatDeclaration(subRepeatAttribute, expression))
                {
                    attributeSyntax = subRepeatAttribute;
                    return true;
                }
            }

            return false;
        }

        private void WriteInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);

            this.Write(node.Modifiers.ToFullString());
            this.Write(node.Keyword.ToFullString());
            this.WriteToken(node.Identifier);

            if (node.TypeParameterList != null)
            {
                this.Write(node.TypeParameterList.ToFullString());
            }

            if (node.BaseList != null)
            {
                this.WriteNode(node.BaseList);
            }

            this.Write(node.ConstraintClauses.ToFullString());

            this.Write(node.OpenBraceToken.ToFullString());

            foreach (var member in node.Members)
            {
                this.Visit(member);
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        private void WriteClassDeclaration(ClassDeclarationSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);

            this.Write(node.Modifiers.ToFullString());
            this.Write(node.Keyword.ToFullString());
            this.WriteToken(node.Identifier);

            if (node.TypeParameterList != null)
            {
                this.Write(node.TypeParameterList.ToFullString());
            }

            if (node.BaseList != null)
            {
                this.WriteNode(node.BaseList);
            }

            this.Write(node.ConstraintClauses.ToFullString());

            this.Write(node.OpenBraceToken.ToFullString());

            foreach (var member in node.Members)
            {
                this.Visit(member);
            }

            this.Write(node.CloseBraceToken.ToFullString());
        }

        private void WriteMethodDeclaration(MethodDeclarationSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);

            this.Write(node.Modifiers.ToFullString());
            this.WriteNode(node.ReturnType);

            this.WriteNode(node.ExplicitInterfaceSpecifier);

            this.WriteToken(node.Identifier);

            this.WriteNode(node.TypeParameterList);

            this.Visit(node.ParameterList);

            foreach (var constraintClauses in node.ConstraintClauses)
            {
                this.WriteNode(constraintClauses);
            }

            this.Visit(node.Body);
            this.Visit(node.ExpressionBody);
            this.Write(node.SemicolonToken.ToFullString());
        }

        private void WriteIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);

            this.Write(node.Modifiers.ToFullString());
            this.WriteNode(node.Type);

            this.WriteNode(node.ExplicitInterfaceSpecifier);

            this.WriteToken(node.ThisKeyword);

            this.Visit(node.ParameterList);

            this.Visit(node.AccessorList);
            this.Visit(node.ExpressionBody);
            this.Write(node.SemicolonToken.ToFullString());
        }

        private void WritePropertyDeclaration(PropertyDeclarationSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);
            this.Write(node.Modifiers.ToFullString());

            this.WriteNode(node.Type);

            this.WriteNode(node.ExplicitInterfaceSpecifier);
            this.Write(node.Identifier.ToFullString());

            this.Visit(node.AccessorList);
            this.Visit(node.ExpressionBody);
            this.Write(node.SemicolonToken.ToFullString());
        }

        private void WriteFieldDeclaration(FieldDeclarationSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);
            this.Write(node.Modifiers.ToFullString());

            this.WriteNode(node.Declaration.Type);

            foreach (var child in node.Declaration.ChildNodesAndTokens())
            {
                if (child.IsNode)
                {
                    var childNode = child.AsNode();

                    if (!object.ReferenceEquals(node.Declaration.Type, childNode))
                    {
                        this.Visit(childNode);
                    }
                }
                else
                {
                    this.WriteToken(child.AsToken());
                }
            }

            this.Write(node.SemicolonToken.ToFullString());
        }

        private void WriteParameter(ParameterSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);

            this.Write(node.Modifiers.ToFullString());
            this.Write(node.Type.ToFullString());
            this.Write(node.Identifier.ToFullString());

            if (node.Default != null)
            {
                this.Write(node.Default.ToFullString());
            }
        }

        private void WriteAttributeLists(SyntaxList<AttributeListSyntax> attributeLists)
        {
            foreach (var attrList in attributeLists)
            {
                var found = false;
                foreach (var attr in attrList.Attributes)
                {
                    if (!attr.IsAttributeName<PatternAttribute>() &&
                        !attr.IsAttributeName<RepeatAttribute>() &&
                        !attr.IsAttributeName<ReplacePatternAttribute>())
                    {
                        if (!found)
                        {
                            this.WriteToken(attrList.OpenBracketToken);
                            found = true;
                        }

                        this.Write(attr.ToFullString());
                    }
                }

                if (found)
                {
                    this.WriteToken(attrList.CloseBracketToken);
                }
                else
                {
                    var line = attrList.OpenBracketToken.ToFullString().Replace("[", string.Empty)
                        + attrList.CloseBracketToken.ToFullString().Replace("]", string.Empty);

                    var newLine = "\r\n";
                    var idx = line.LastIndexOf(newLine, StringComparison.InvariantCulture);
                    if (idx < 0)
                    {
                        newLine = "\n";
                        idx = line.LastIndexOf(newLine, StringComparison.InvariantCulture);
                    }

                    if (idx >= 0)
                    {
                        this.Write(line.Substring(0, idx).TrimEnd(' ', '\t'));
                        this.Write(line.Substring(idx + newLine.Length));
                    }
                    else
                    {
                        this.Write(line);
                    }
                }
            }
        }

        private void WriteNode(SyntaxNode node)
        {
            if (node != null)
            {
                this.Write(node.ToFullString());
            }
        }

        private void WriteToken(SyntaxToken token)
        {
            this.Write(token.ToFullString());
        }

        private void Write(string text)
        {
            this.textWriter.Write(this.strategy.ApplyPatternReplace(text));
        }
    }
}
