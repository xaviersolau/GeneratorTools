// ----------------------------------------------------------------------
// <copyright file="ReflectionAttributeSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Linq;
using System.Reflection;
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

            if (this.customAttribute.NamedArguments.Any())
            {
                ctorArgs = $"({string.Join(",", this.customAttribute.NamedArguments.Select(arg => arg.MemberName + "=" + arg.TypedValue.ToString()))})";
            }

            var node = GetSyntaxNode($"[{this.customAttribute.AttributeType.FullName}{ctorArgs}] public int Property {{ get; set; }}");
            return ((PropertyDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single())
                .AttributeLists.Single().Attributes.Single();
        }
    }
}
