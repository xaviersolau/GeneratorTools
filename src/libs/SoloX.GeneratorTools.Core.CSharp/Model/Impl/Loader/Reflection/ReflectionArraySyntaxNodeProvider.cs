// ----------------------------------------------------------------------
// <copyright file="ReflectionArraySyntaxNodeProvider.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionArraySyntaxNodeProvider : AReflectionSyntaxNodeProvider<ArrayTypeSyntax>
    {
        private int arrayCount;
        private ISyntaxNodeProvider<SyntaxNode> typeSyntaxNodeProvider;

        public ReflectionArraySyntaxNodeProvider(
            int arrayCount,
            ISyntaxNodeProvider<SyntaxNode> typeSyntaxNodeProvider)
        {
            this.arrayCount = arrayCount;
            this.typeSyntaxNodeProvider = typeSyntaxNodeProvider;
        }

        protected override ArrayTypeSyntax Generate()
        {
            var array = string.Empty;

            for (int i = 0; i < this.arrayCount; i++)
            {
                array += "[]";
            }

            var type = this.typeSyntaxNodeProvider.SyntaxNode.ToString();

            var node = GetSyntaxNode($"{type}{array} x;");
            return (ArrayTypeSyntax)((FieldDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single()).Declaration.Type;
        }
    }
}
