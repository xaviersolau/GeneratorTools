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
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedWalker : CSharpSyntaxWalker
    {
        private const string GeneratorAttributesNameSpace = "SoloX.GeneratorTools.Core.CSharp.Generator.Attributes";

        private readonly TextWriter textWriter;
        private readonly IDeclaration<SyntaxNode> pattern;

        private readonly Stack<IReplacePatternHandler> strategiesReplacePatternHandlers = new Stack<IReplacePatternHandler>();

        private class StrategyCount
        {
            public StrategyCount(IAutomatedStrategy strategy)
            {
                Strategy = strategy;
                Count = 1;
            }

            public IAutomatedStrategy Strategy { get; }
            public int Count { get; set; }
        }

        private Stack<StrategyCount> strategies;


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
            this.strategies = new Stack<StrategyCount>();
            this.strategies.Push(new StrategyCount(strategy));

            this.strategiesReplacePatternHandlers.Push(strategy.CreateReplacePatternHandler());
        }

        private IAutomatedStrategy CurrentStrategy()
        {
            return this.strategies.Peek().Strategy;
        }

        private void PushReplacePatternHandlers(IAutomatedStrategy strategy)
        {
            this.strategiesReplacePatternHandlers.Push(strategy.CreateReplacePatternHandler());
        }
        private void PopReplacePatternHandlers()
        {
            this.strategiesReplacePatternHandlers.Pop();
        }

        private void PushStrategy(IAutomatedStrategy strategy)
        {
            var last = this.strategies.Peek();
            if (object.ReferenceEquals(last.Strategy, strategy))
            {
                last.Count++;
            }
            else
            {
                this.strategies.Push(new StrategyCount(strategy));
            }
        }

        private void PopStrategy(IAutomatedStrategy strategy)
        {
            var last = this.strategies.Peek();
            if (object.ReferenceEquals(last.Strategy, strategy))
            {
                last.Count--;

                if (last.Count == 0)
                {
                    this.strategies.Pop();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var nameTxt = node.Name.ToString();

            if (GeneratorAttributesNameSpace.Equals(nameTxt, StringComparison.Ordinal))
            {
                var txt = node.ToFullString();

                CurrentStrategy().RepeatNameSpace(
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
                    !CurrentStrategy().IgnoreUsingDirective(nameTxt))
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
                .Replace(this.pattern.DeclarationNameSpace, CurrentStrategy().GetCurrentNameSpace()));
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
                CurrentStrategy().RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        PushReplacePatternHandlers(itemStrategy);
                        PushStrategy(itemStrategy);

                        WriteClassDeclaration(node);

                        PopStrategy(itemStrategy);
                        PopReplacePatternHandlers();
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
                CurrentStrategy().RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        PushReplacePatternHandlers(itemStrategy);
                        PushStrategy(itemStrategy);

                        WriteInterfaceDeclaration(node);

                        PopStrategy(itemStrategy);
                        PopReplacePatternHandlers();
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
                    CurrentStrategy().RepeatDeclaration(
                        attributeSyntax,
                        itemStrategy =>
                        {
                            PushReplacePatternHandlers(itemStrategy);
                            PushStrategy(itemStrategy);

                            if (firstParameter)
                            {
                                firstParameter = false;
                            }
                            else
                            {
                                this.Write(tkns);
                            }

                            WriteParameter(parameter);

                            // we must not pop the strategy because the parameter list strategy must be given to the parent (to make it available to the method body)
                            //PopStrategy();

                            // But the replace pattern handler must be removed from the stack
                            PopReplacePatternHandlers();
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
                    CurrentStrategy().RepeatDeclaration(
                        attributeSyntax,
                        itemStrategy =>
                        {
                            PushReplacePatternHandlers(itemStrategy);
                            PushStrategy(itemStrategy);

                            if (firstParameter)
                            {
                                firstParameter = false;
                            }
                            else
                            {
                                this.Write(tkns);
                            }

                            WriteParameter(parameter);

                            PopStrategy(itemStrategy);
                            PopReplacePatternHandlers();
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
                CurrentStrategy().RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        PushReplacePatternHandlers(itemStrategy);
                        PushStrategy(itemStrategy);

                        WritePropertyDeclaration(node);

                        PopStrategy(itemStrategy);
                        PopReplacePatternHandlers();
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
                CurrentStrategy().RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        PushReplacePatternHandlers(itemStrategy);
                        PushStrategy(itemStrategy);

                        WriteFieldDeclaration(node);

                        PopStrategy(itemStrategy);
                        PopReplacePatternHandlers();
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
            this.WriteConstructorDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax))
            {
                CurrentStrategy().RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        PushReplacePatternHandlers(itemStrategy);
                        PushStrategy(itemStrategy);

                        WriteMethodDeclaration(node);

                        PopStrategy(itemStrategy);
                        PopReplacePatternHandlers();
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
                CurrentStrategy().RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        PushReplacePatternHandlers(itemStrategy);
                        PushStrategy(itemStrategy);

                        WriteIndexerDeclaration(node);

                        PopStrategy(itemStrategy);
                        PopReplacePatternHandlers();
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
                this.Visit(node.ArgumentList);
            }

            if (node.Initializer != null)
            {
                this.WriteToken(node.Initializer.OpenBraceToken);

                var tknList = node.Initializer.ChildTokens();
#pragma warning disable CA1851 // Possible multiple enumerations of 'IEnumerable' collection
                var tkns = tknList.Count() > 2 ? tknList.ElementAt(1).ToFullString() : ", ";
#pragma warning restore CA1851 // Possible multiple enumerations of 'IEnumerable' collection

                var writeSemicolonAction = () => { Write(tkns); };

                foreach (var expression in node.Initializer.Expressions)
                {
                    if (!ProcessRepeatAffectation(expression, writeSemicolonAction))
                    {
                        Visit(expression);
                        writeSemicolonAction();
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
            if (!ProcessRepeatStatements(node))
            {
                var writeSemicolonAction = () => { WriteToken(node.SemicolonToken); };

                if (!ProcessRepeatAffectation(node.Expression, writeSemicolonAction))
                {
                    Visit(node.Expression);
                    writeSemicolonAction();
                }
            }
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            Visit(node.Left);
            WriteToken(node.OperatorToken);
            Visit(node.Right);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            base.VisitInvocationExpression(node);
        }

        public override void VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            WriteToken(node.AwaitKeyword);
            Visit(node.Expression);
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if (!ProcessRepeatAffectation(node))
            {
                Visit(node.Declaration);
                WriteToken(node.SemicolonToken);
            }
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            WriteNode(node.Type);

            foreach (var variable in node.Variables)
            {
                Visit(variable);
            }
        }

        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            this.WriteToken(node.Identifier);
            this.Visit(node.Initializer);
        }

        public override void VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            this.WriteToken(node.EqualsToken);
            this.Visit(node.Value);
        }

        public override void VisitArgumentList(ArgumentListSyntax node)
        {
            this.WriteToken(node.OpenParenToken);

            var isFirst = true;

            foreach (var argument in node.Arguments)
            {
                var writeComma = () =>
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        this.Write(", ");
                    }
                };

                if (!ProcessRepeatArgument(argument, writeComma))
                {
                    writeComma();
                    Visit(argument);
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
                var writeComma = () =>
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        this.Write(", ");
                    }
                };

                if (!ProcessRepeatArgument(argument, writeComma))
                {
                    writeComma();
                    Visit(argument);
                }
            }

            this.WriteToken(node.CloseBracketToken);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            this.WriteToken(node.Identifier);
        }

        public override void VisitGenericName(GenericNameSyntax node)
        {
            this.WriteToken(node.Identifier);

            this.Visit(node.TypeArgumentList);
        }

        public override void VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            this.WriteToken(node.LessThanToken);

            var isFirst = true;

            foreach (var argument in node.Arguments)
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

            this.WriteToken(node.GreaterThanToken);
        }

        public override void VisitPredefinedType(PredefinedTypeSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitThisExpression(ThisExpressionSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            WriteToken(node.NewKeyword);
            Visit(node.Type);
            Visit(node.Initializer);
        }

        public override void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            WriteToken(node.OpenBraceToken);

            var isFirst = true;

            foreach (var expression in node.Expressions)
            {
                var writeComma = () =>
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        this.Write(", ");
                    }
                };

                if (!ProcessRepeatArgument(expression, writeComma))
                {
                    writeComma();
                    Visit(expression);
                }
            }

            WriteToken(node.CloseBraceToken);
        }

        public override void VisitArrayType(ArrayTypeSyntax node)
        {
            Visit(node.ElementType);
            foreach (var rankSpecifier in node.RankSpecifiers)
            {
                Visit(rankSpecifier);
            }
        }

        public override void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            WriteNode(node);
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            WriteIfStatement(node);
        }

        private void WriteIfStatement(IfStatementSyntax node)
        {
            this.WriteToken(node.IfKeyword);
            this.WriteToken(node.OpenParenToken);
            this.WriteNode(node.Condition);
            this.WriteToken(node.CloseParenToken);
            this.Visit(node.Statement);
            if (node.Else != null)
            {
                this.Visit(node.Else);
            }
        }

        public override void VisitElseClause(ElseClauseSyntax node)
        {
            this.WriteToken(node.ElseKeyword);
            this.Visit(node.Statement);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
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

        public override void VisitForStatement(ForStatementSyntax node)
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

        public override void VisitThrowStatement(ThrowStatementSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            this.WriteNode(node);
        }

        public override void VisitTryStatement(TryStatementSyntax node)
        {
            this.WriteToken(node.TryKeyword);
            this.Visit(node.Block);

            if (node.Catches != null)
            {
                foreach (var catche in node.Catches)
                {
                    this.Visit(catche);
                }
            }

            if (node.Finally != null)
            {
                this.Visit(node.Finally);
            }
        }

        public override void VisitCatchClause(CatchClauseSyntax node)
        {
            this.WriteToken(node.CatchKeyword);

            if (node.Declaration != null)
            {
                this.Visit(node.Declaration);
            }

            this.Visit(node.Block);
        }

        public override void VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            this.WriteToken(node.OpenParenToken);

            this.Visit(node.Type);

            if (node.Identifier != null)
            {
                this.VisitToken(node.Identifier);
            }

            this.WriteToken(node.CloseParenToken);
        }

        public override void VisitQualifiedName(QualifiedNameSyntax node)
        {
            this.Visit(node.Left);
            this.WriteToken(node.DotToken);
            this.Visit(node.Right);
        }

        public override void VisitFinallyClause(FinallyClauseSyntax node)
        {
            this.WriteToken(node.FinallyKeyword);
            this.Visit(node.Block);
        }

        public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            if (node.AsyncKeyword != null)
            {
                this.WriteToken(node.AsyncKeyword);
            }

            this.WriteParameter(node.Parameter);

            this.WriteToken(node.ArrowToken);

            this.Visit(node.Body);
        }

        public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            if (node.AsyncKeyword != null)
            {
                this.WriteToken(node.AsyncKeyword);
            }

            this.Visit(node.ParameterList);

            this.WriteToken(node.ArrowToken);

            this.Visit(node.Body);
        }

        private bool ProcessRepeatAffectation(LocalDeclarationStatementSyntax node)
        {
            if (node.Declaration.Variables.Count > 1)
            {
                return false;
            }

            var variable = node.Declaration.Variables[0];

            var initializer = variable.Initializer;

            if (initializer == null)
            {
                return false;
            }

            if (initializer.Value is InvocationExpressionSyntax invocation)
            {
                var method = invocation.Expression.ToString();
                if (method == "Repeat.Affectation"
                    || (method.StartsWith("Repeat.Affectation<", StringComparison.Ordinal) && method.EndsWith(">", StringComparison.Ordinal)))
                {
                    var patternName = invocation.ArgumentList.Arguments.First().Expression;
                    var expression = invocation.ArgumentList.Arguments.Last().Expression;

                    ProcessSubRepeatAttribute(patternName, walker =>
                    {
                        walker.WriteToken(node.UsingKeyword);
                        walker.WriteNode(node.Declaration.Type);
                        walker.WriteToken(variable.Identifier);
                        walker.WriteToken(initializer.EqualsToken);
                        walker.WriteNode(expression);
                        walker.WriteToken(node.SemicolonToken);
                    });

                    return true;
                }
            }


            return false;
        }

        private bool ProcessRepeatAffectation(ExpressionSyntax expressionSyntax, Action writeSemicolon)
        {
            if (expressionSyntax is AssignmentExpressionSyntax assignment)
            {
                if (assignment.Right is InvocationExpressionSyntax invocation)
                {
                    var method = invocation.Expression.ToString();
                    if (method == "Repeat.Affectation"
                        || (method.StartsWith("Repeat.Affectation<", StringComparison.Ordinal) && method.EndsWith(">", StringComparison.Ordinal)))
                    {
                        var patternName = invocation.ArgumentList.Arguments.First().Expression;
                        var expression = invocation.ArgumentList.Arguments.Last().Expression;

                        ProcessSubRepeatAttribute(patternName, walker =>
                        {
                            walker.WriteNode(assignment.Left);
                            walker.WriteToken(assignment.OperatorToken);
                            walker.WriteNode(expression);
                            writeSemicolon();
                        });

                        return true;
                    }
                }
            }

            return false;
        }

        private bool ProcessRepeatStatements(ExpressionStatementSyntax expressionNode)
        {
            if (expressionNode.Expression is InvocationExpressionSyntax invocation)
            {
                var method = invocation.Expression.ToString();
                if (method == "Repeat.Statements")
                {
                    var patternName = invocation.ArgumentList.Arguments.First().Expression;

                    var expression = invocation.ArgumentList.Arguments.Last().Expression;

                    var repeatWalker = new RepeatWalker();
                    var statements = repeatWalker.Visit(expression);

                    var startTkns = expressionNode.GetLeadingTrivia().ToFullString();
                    var endTkns = expressionNode.GetTrailingTrivia().ToFullString();

                    ProcessSubRepeatAttribute(patternName, walker =>
                    {
                        var statementTxt = statements.ToFullString().Substring(statements.GetLeadingTrivia().ToFullString().Length);

                        if (statements is BlockSyntax block)
                        {
                            walker.Write(startTkns + statementTxt + endTkns);
                        }
                        else
                        {
                            walker.Write(startTkns + statementTxt + ';' + endTkns);
                        }
                    });

                    return true;
                }
            }

            return false;
        }

        private bool ProcessRepeatArgument(ArgumentSyntax argument, Action writeCommaToken)
        {
            return ProcessRepeatArgument(argument.Expression, writeCommaToken);
        }

        private bool ProcessRepeatArgument(ExpressionSyntax expressionSyntax, Action writeCommaToken)
        {
            if (expressionSyntax is InvocationExpressionSyntax invocation)
            {
                var method = invocation.Expression.ToString();
                if (method == "Repeat.Argument")
                {
                    var patternName = invocation.ArgumentList.Arguments.First().Expression;
                    var expression = invocation.ArgumentList.Arguments.Last().Expression;

                    ProcessSubRepeatAttribute(patternName, walker =>
                    {
                        writeCommaToken();

                        walker.WriteNode(expression);
                    });

                    return true;
                }
            }

            return false;
        }

        internal class RepeatWalker : CSharpSyntaxVisitor<SyntaxNode>
        {
            public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
            {
                return node.Body;
            }
        }


        private void ProcessSubRepeatAttribute(SyntaxNode patternName, Action<AutomatedWalker> repeatHandler)
        {
            var strategiesReversed = new List<IAutomatedStrategy>(this.strategies.Select(s => s.Strategy));

            foreach (var strategy in strategiesReversed)
            {
                strategy.TryMatchAndRepeatStatement(
                    patternName,
                    null,
                    null,
                    itemStrategy =>
                    {
                        PushReplacePatternHandlers(itemStrategy);
                        PushStrategy(itemStrategy);

                        repeatHandler(this);

                        PopStrategy(itemStrategy);
                        PopReplacePatternHandlers();
                    });
            }
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

        private void WriteConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            this.WriteAttributeLists(node.AttributeLists);
            this.Write(node.Modifiers.ToFullString());
            this.WriteToken(node.Identifier);
            this.WriteNode(node.ParameterList);

            if (node.Initializer != null)
            {
                this.WriteToken(node.Initializer.ColonToken);
                this.WriteToken(node.Initializer.ThisOrBaseKeyword);
                this.Visit(node.Initializer.ArgumentList);
            }

            this.Visit(node.Body);
        }

        private void WriteMethodDeclaration(MethodDeclarationSyntax node)
        {
            IReplacePatternHandler replacePatternHandler = null;
            if (node.AttributeLists.TryMatchAttributeName<ReplacePatternAttribute>(out var replaceAttributeSyntax))
            {
                replacePatternHandler = this.CurrentStrategy().CreateReplacePatternHandler(replaceAttributeSyntax);
            }

            this.WriteAttributeLists(node.AttributeLists);

            this.Write(node.Modifiers.ToFullString());
            this.WriteNode(node.ReturnType);

            this.WriteNode(node.ExplicitInterfaceSpecifier);

            this.WriteToken(node.Identifier);

            this.WriteNode(node.TypeParameterList);

            var savedStrategies = this.strategies;

            this.strategies = new Stack<StrategyCount>();

            foreach (var strategy in savedStrategies.Reverse())
            {
                this.strategies.Push(new StrategyCount(strategy.Strategy)
                {
                    Count = strategy.Count,
                });
            }

            if (node.AttributeLists.TryMatchAttributeName<RepeatAttribute>(out var attributeSyntax, true))
            {
                CurrentStrategy().RepeatDeclaration(
                    attributeSyntax,
                    itemStrategy =>
                    {
                        PushStrategy(itemStrategy);
                    });
            }

            this.Visit(node.ParameterList);

            if (replacePatternHandler != null)
            {
                this.strategiesReplacePatternHandlers.Push(replacePatternHandler);
            }

            foreach (var constraintClauses in node.ConstraintClauses)
            {
                this.WriteNode(constraintClauses);
            }

            this.Visit(node.Body);
            this.Visit(node.ExpressionBody);
            this.Write(node.SemicolonToken.ToFullString());

            if (replacePatternHandler != null)
            {
                PopReplacePatternHandlers();
            }

            this.strategies = savedStrategies;
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

            if (node.Type != null)
            {
                this.Write(node.Type.ToFullString());
            }

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
                        this.Write(line.TrimEnd(' ', '\t'));
                    }
                }
            }
        }

        private void WriteNode(SyntaxNode node)
        {
            if (node != null)
            {
                Write(node.ToFullString());
            }
        }

        private void WriteToken(SyntaxToken token)
        {
            if (token != null)
            {
                Write(token.ToFullString());
            }
        }

        private void Write(string text)
        {
            foreach (var replacePatternHandler in this.strategiesReplacePatternHandlers)
            {
                text = replacePatternHandler.ApplyOn(text);
            }

            this.textWriter.Write(text);
        }
    }
}
