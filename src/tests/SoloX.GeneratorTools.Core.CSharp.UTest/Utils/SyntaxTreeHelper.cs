﻿// ----------------------------------------------------------------------
// <copyright file="SyntaxTreeHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Utils
{
    public static class SyntaxTreeHelper
    {
        /// <summary>
        /// Generate a syntax node given the declaration use code statement.
        /// </summary>
        /// <param name="typeStatement">The CSharp code to generate the type syntax node from.</param>
        /// <returns>The syntax node.</returns>
        public static TypeSyntax GetTypeSyntax(string typeStatement)
        {
            var syntaxNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode($"{typeStatement} a;");

            var field = (LocalDeclarationStatementSyntax)((GlobalStatementSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single()).Statement;
            return field.Declaration.Type;
        }

        /// <summary>
        /// Generate a property declaration syntax as if it was declared in a class implementation.
        /// </summary>
        /// <param name="type">The property type.</param>
        /// <param name="name">The property name.</param>
        /// <param name="field">The field to get or set.</param>
        /// <returns>The PropertyDeclarationSyntax node.</returns>
        public static PropertyDeclarationSyntax GetPropertyImplSyntax(string type, string name, string field = null)
        {
            var getImpl = "get;";
            var setImpl = "set;";

            if (!string.IsNullOrEmpty(field))
            {
                getImpl = $"get {{ return {field}; }}";
                setImpl = $"set {{ {field} = value; }}";
            }

            var syntaxNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode($"public {type} {name} {{ {getImpl} {setImpl} }}");
            return (PropertyDeclarationSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single();
        }

        /// <summary>
        /// Generate a property declaration syntax as if it was declared in a class implementation with
        /// an expression body.
        /// </summary>
        /// <param name="type">The property type.</param>
        /// <param name="name">The property name.</param>
        /// <param name="expressionBody">The expression body.</param>
        /// <returns>The PropertyDeclarationSyntax node.</returns>
        public static PropertyDeclarationSyntax GetExpressionBodyPropertyImplSyntax(string type, string name, string expressionBody)
        {
            var syntaxNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode($"public {type} {name} => {expressionBody};");
            return (PropertyDeclarationSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single();
        }

        /// <summary>
        /// Generate a field declaration syntax as if it was declared in a class implementation.
        /// </summary>
        /// <param name="type">The field type.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The FieldDeclarationSyntax node.</returns>
        public static FieldDeclarationSyntax GetFieldSyntax(string type, string name)
        {
            var syntaxNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode($"public {type} {name};");
            return (FieldDeclarationSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single();
        }

        /// <summary>
        /// Generate a field declaration syntax as if it was declared in a class implementation.
        /// </summary>
        /// <param name="type">The field type.</param>
        /// <param name="name">The field name.</param>
        /// <param name="init">The field initializer.</param>
        /// <returns>The FieldDeclarationSyntax node.</returns>
        public static FieldDeclarationSyntax GetFieldSyntax(string type, string name, string init)
        {
            var syntaxNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode($"public {type} {name} = {init};");
            return (FieldDeclarationSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single();
        }

        /// <summary>
        /// Generate a local declaration syntax as if it was declared in a method implementation
        /// (for a variable declaration by instance).
        /// </summary>
        /// <param name="type">The variable type.</param>
        /// <param name="name">The variable name.</param>
        /// <returns>The VariableDeclarationSyntax node.</returns>
        public static LocalDeclarationStatementSyntax GetLocalDeclarationStatementSyntax(string type, string name, string init)
        {
            var syntaxNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode($"void Method(){{{type} {name} = {init};}}");

            var localFunctionStatementSyntax = (LocalFunctionStatementSyntax)((GlobalStatementSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single()).Statement;

            var statementSyntax = localFunctionStatementSyntax.Body.Statements.First();
            return (LocalDeclarationStatementSyntax)statementSyntax;
        }

        /// <summary>
        /// Generate a expression syntax.
        /// </summary>
        /// <param name="expressionBody">The expression body.</param>
        /// <returns>The ExpressionSyntax node.</returns>
        public static ExpressionSyntax GetExpressionSyntax(string expressionBody)
        {
            var syntaxNode = GetLocalDeclarationStatementSyntax("var", "name", expressionBody);

            return syntaxNode.Declaration.Variables.First().Initializer.Value;
        }

        /// <summary>
        /// Generate a method declaration syntax as if it was declared in a class implementation.
        /// </summary>
        /// <param name="type">The method return type.</param>
        /// <param name="name">The method name.</param>
        /// <param name="argument">The argument name.</param>
        /// <returns>The MethodDeclarationSyntax node.</returns>
        public static MethodDeclarationSyntax GetMethodSyntax(string type, string name, string argument, string otherArguments)
        {
            if (otherArguments == null)
            {
                otherArguments = string.Empty;
            }

            var syntaxNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode($"public class X {{ public {type} {name}({type} {argument}{otherArguments}){{}}}}");

            return (MethodDeclarationSyntax)((ClassDeclarationSyntax)((CompilationUnitSyntax)syntaxNode).Members.First()).Members.Single();
        }

        public static ISyntaxNodeProvider<TNode> GetSyntaxNodeProvider<TNode>(TNode node)
            where TNode : SyntaxNode
        {
            var nodeProviderMock = new Mock<ISyntaxNodeProvider<TNode>>();
            nodeProviderMock.Setup(p => p.SyntaxNode).Returns(node);
            return nodeProviderMock.Object;
        }
    }
}
