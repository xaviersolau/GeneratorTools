// ----------------------------------------------------------------------
// <copyright file="ReflectionTypeSyntaxNodeProvider.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionTypeSyntaxNodeProvider<TNode> : AReflectionSyntaxNodeProvider<TNode>
        where TNode : MemberDeclarationSyntax
    {
        private Type declarationType;

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
