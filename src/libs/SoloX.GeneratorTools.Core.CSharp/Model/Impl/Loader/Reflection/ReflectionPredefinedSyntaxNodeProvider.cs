// ----------------------------------------------------------------------
// <copyright file="ReflectionPredefinedSyntaxNodeProvider.cs" company="SoloX Software">
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
    internal class ReflectionPredefinedSyntaxNodeProvider : AReflectionSyntaxNodeProvider<PredefinedTypeSyntax>
    {
        private Type predefinedType;

        public ReflectionPredefinedSyntaxNodeProvider(Type predefinedType)
        {
            this.predefinedType = predefinedType;
        }

        protected override PredefinedTypeSyntax Generate()
        {
            string typeName;
            if (this.predefinedType == typeof(int))
            {
                typeName = "int";
            }
            else
            {
                throw new NotSupportedException();
            }

            var node = GetSyntaxNode($"{typeName} x;");
            return (PredefinedTypeSyntax)((FieldDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single()).Declaration.Type;
        }
    }
}
