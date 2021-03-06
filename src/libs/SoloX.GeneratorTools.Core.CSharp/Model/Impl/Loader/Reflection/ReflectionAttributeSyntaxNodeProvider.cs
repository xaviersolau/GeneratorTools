﻿// ----------------------------------------------------------------------
// <copyright file="ReflectionAttributeSyntaxNodeProvider.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionAttributeSyntaxNodeProvider : AReflectionSyntaxNodeProvider<AttributeSyntax>
    {
        private readonly CustomAttributeData customAttribute;

        public ReflectionAttributeSyntaxNodeProvider(CustomAttributeData customAttribute)
        {
            this.customAttribute = customAttribute;
        }

        protected override AttributeSyntax Generate()
        {
            var ctorArgs = string.Empty;

            if (this.customAttribute.ConstructorArguments.Any())
            {
                ctorArgs = $"({string.Join(",", this.customAttribute.ConstructorArguments.Select(arg => arg.ToString()))})";
            }

            var node = GetSyntaxNode($"[{this.customAttribute.AttributeType.FullName}{ctorArgs}] public int Property {{ get; set; }}");
            return ((PropertyDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single())
                .AttributeLists.Single().Attributes.Single();
        }
    }
}
