// ----------------------------------------------------------------------
// <copyright file="SyntaxTreeHelper.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
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

            var field = (FieldDeclarationSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single();
            return field.Declaration.Type;
        }

        /// <summary>
        /// Generate a property declaration syntax as if it was declared in a class implementation.
        /// </summary>
        /// <param name="type">The property type.</param>
        /// <param name="name">The property name.</param>
        /// <param name="field">The field to get or set.</param>
        /// <returns>The PropertyDeclarationSyntax node.</returns>
        public static SyntaxNode GetPropertyImplSyntax(string type, string name, string field = null)
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
        /// Generate a field declaration syntax as if it was declared in a class implementation.
        /// </summary>
        /// <param name="type">The field type.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The FieldDeclarationSyntax node.</returns>
        public static SyntaxNode GetFieldSyntax(string type, string name)
        {
            var syntaxNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode($"public {type} {name};");
            return (FieldDeclarationSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single();
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
