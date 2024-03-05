// ----------------------------------------------------------------------
// <copyright file="ReflectionMethodSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionMethodSyntaxNodeProvider : AReflectionSyntaxNodeProvider<MethodDeclarationSyntax>
    {
        private readonly MethodInfo method;
        private readonly ISyntaxNodeProvider<SyntaxNode> methodTypeNodeProvider;

        public ReflectionMethodSyntaxNodeProvider(
            MethodInfo method,
            ISyntaxNodeProvider<SyntaxNode> methodTypeNodeProvider)
        {
            this.method = method;
            this.methodTypeNodeProvider = methodTypeNodeProvider;
        }

        protected override MethodDeclarationSyntax Generate()
        {
            var methodParameters = this.method.GetParameters();
            var node = GetSyntaxNode($"public {this.methodTypeNodeProvider.SyntaxNode.ToString()} {this.method.Name}({string.Join(",", methodParameters.Select(x => x.ParameterType.FullName + " " + x.Name))}) {{}}");
            return (MethodDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single();
        }
    }
}
