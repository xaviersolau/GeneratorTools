// ----------------------------------------------------------------------
// <copyright file="ReflectionMethodSyntaxNodeProvider.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionMethodSyntaxNodeProvider : AReflectionSyntaxNodeProvider<MethodDeclarationSyntax>
    {
        private MethodInfo method;
        private ISyntaxNodeProvider<SyntaxNode> methodTypeNodeProvider;

        public ReflectionMethodSyntaxNodeProvider(
            MethodInfo method,
            ISyntaxNodeProvider<SyntaxNode> methodTypeNodeProvider)
        {
            this.method = method;
            this.methodTypeNodeProvider = methodTypeNodeProvider;
        }

        protected override MethodDeclarationSyntax Generate()
        {
            var node = GetSyntaxNode($"public {this.methodTypeNodeProvider.SyntaxNode.ToString()} {this.method.Name} {{ get; set; }}");
            return (MethodDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single();
        }
    }
}
