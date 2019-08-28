// ----------------------------------------------------------------------
// <copyright file="ReflectionTypeUseSyntaxNodeProvider.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionTypeUseSyntaxNodeProvider<TNode> : AReflectionSyntaxNodeProvider<TNode>
        where TNode : SimpleNameSyntax
    {
        private Type typeUsed;

        public ReflectionTypeUseSyntaxNodeProvider(Type typeUsed)
        {
            this.typeUsed = typeUsed;
        }

        protected override TNode Generate()
        {
            var node = GetSyntaxNode($"{this.typeUsed.Name} x;");
            return (TNode)((FieldDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single()).Declaration.Type;
        }
    }
}
