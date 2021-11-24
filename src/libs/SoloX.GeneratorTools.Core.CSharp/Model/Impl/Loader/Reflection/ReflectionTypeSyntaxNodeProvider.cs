// ----------------------------------------------------------------------
// <copyright file="ReflectionTypeSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionTypeSyntaxNodeProvider<TNode> : AReflectionSyntaxNodeProvider<TNode>
        where TNode : MemberDeclarationSyntax
    {
        private readonly Type declarationType;

        public ReflectionTypeSyntaxNodeProvider(Type declarationType)
        {
            this.declarationType = declarationType;
        }

        protected override TNode Generate()
        {
            var type = "class";
            if (this.declarationType.IsInterface)
            {
                type = "interface";
            }
            else if (this.declarationType.IsEnum)
            {
                type = "enum";
            }
            else if (this.declarationType.IsValueType)
            {
                type = "struct";
            }

            var node = GetSyntaxNode($"public {type} {this.declarationType.Name} {{}}");
            return (TNode)((CompilationUnitSyntax)node).Members.Single();
        }
    }
}
