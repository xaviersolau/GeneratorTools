// ----------------------------------------------------------------------
// <copyright file="MetadataAttributeSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System.Linq;
using System.Reflection.Metadata;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataAttributeSyntaxNodeProvider : AMetadataSyntaxNodeProvider<AttributeSyntax>
    {
        private readonly string fullName;
        private readonly CustomAttributeValue<IDeclarationUse<SyntaxNode>> value;

        public MetadataAttributeSyntaxNodeProvider(string fullName, CustomAttributeValue<IDeclarationUse<SyntaxNode>> value)
        {
            this.fullName = fullName;
            this.value = value;
        }

        protected override AttributeSyntax Generate()
        {
            var ctorArgs = string.Empty;

            foreach (var namedArgument in this.value.NamedArguments)
            {
                var argName = namedArgument.Name;
                var argType = namedArgument.Type;
                var argValue = namedArgument.Value;

                if (!string.IsNullOrEmpty(ctorArgs))
                {
                    ctorArgs += ", ";
                }

                ctorArgs += argName + "=" + ConvertToString(argValue, argType);
            }

            foreach (var fixedArgument in this.value.FixedArguments)
            {
                var argType = fixedArgument.Type;
                var argValue = fixedArgument.Value;

                if (!string.IsNullOrEmpty(ctorArgs))
                {
                    ctorArgs += ", ";
                }
                ctorArgs += ConvertToString(argValue, argType);
            }


            if (!string.IsNullOrEmpty(ctorArgs))
            {
                ctorArgs = $"({ctorArgs})";
            }

            var node = GetSyntaxNode($"[{this.fullName}{ctorArgs}] public int Property {{ get; set; }}");
            return ((PropertyDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single())
                .AttributeLists.Single().Attributes.Single();
        }

        private static string ConvertToString(object argValue, IDeclarationUse<SyntaxNode> argType)
        {
            if (argType.Declaration.FullName == "System.Type" && argValue is IDeclarationUse<SyntaxNode> declarationUse)
            {
                return $"typeof({declarationUse.SyntaxNodeProvider.SyntaxNode})";
            }
            else if (argType.Declaration.FullName == "System.String" || argType.Declaration.FullName == "string")
            {
                return $"\"{argValue}\"";
            }

            return argValue.ToString();
        }
    }
}
