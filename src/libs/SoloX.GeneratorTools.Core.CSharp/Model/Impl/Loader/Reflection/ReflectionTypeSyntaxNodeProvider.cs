// ----------------------------------------------------------------------
// <copyright file="ReflectionTypeSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
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
            var type = string.Empty;
            if (typeof(TNode) == typeof(InterfaceDeclarationSyntax))
            {
                type = "interface";
            }
            else if (typeof(TNode) == typeof(EnumDeclarationSyntax))
            {
                type = "enum";
            }
            else if (typeof(TNode) == typeof(StructDeclarationSyntax))
            {
                type = "struct";
            }
            else if (typeof(TNode) == typeof(ClassDeclarationSyntax))
            {
                type = "class";
            }
            else if (typeof(TNode) == typeof(RecordDeclarationSyntax))
            {
                if (ReflectionGenericDeclarationLoader<RecordDeclarationSyntax>.ProbeRecordStructType(this.declarationType))
                {
                    type = "record struct";
                }
                else
                {
                    type = "record";
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            if (this.declarationType.IsGenericType)
            {
                var name = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(this.declarationType.Name);

                var node = GetSyntaxNode($"public {type} {name} {{}}");
                return (TNode)((CompilationUnitSyntax)node).Members.Single();
            }
            else
            {
                var node = GetSyntaxNode($"public {type} {this.declarationType.Name} {{}}");
                return (TNode)((CompilationUnitSyntax)node).Members.Single();
            }
        }
    }
}
