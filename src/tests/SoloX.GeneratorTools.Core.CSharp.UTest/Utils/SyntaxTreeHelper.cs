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
            var syntaxNode = GetSyntaxNode($"{typeStatement} a;");

            var field = (FieldDeclarationSyntax)((CompilationUnitSyntax)syntaxNode).Members.Single();
            return field.Declaration.Type;
        }

        private static SyntaxNode GetSyntaxNode(string text)
        {
            var src = SourceText.From(text);
            var syntaxTree = CSharpSyntaxTree.ParseText(src);

            return syntaxTree.GetRoot();
        }
    }
}
