// ----------------------------------------------------------------------
// <copyright file="ReflectionPredefinedSyntaxNodeProvider.cs" company="Xavier Solau">
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
    internal class ReflectionPredefinedSyntaxNodeProvider : AReflectionSyntaxNodeProvider<PredefinedTypeSyntax>
    {
        private readonly Type predefinedType;

        public ReflectionPredefinedSyntaxNodeProvider(Type predefinedType)
        {
            this.predefinedType = predefinedType;
        }

        protected override PredefinedTypeSyntax Generate()
        {
            string typeName;
            if (this.predefinedType == typeof(byte))
            {
                typeName = "byte";
            }
            else if (this.predefinedType == typeof(short))
            {
                typeName = "short";
            }
            else if (this.predefinedType == typeof(int))
            {
                typeName = "int";
            }
            else if (this.predefinedType == typeof(long))
            {
                typeName = "long";
            }
            else if (this.predefinedType == typeof(string))
            {
                typeName = "string";
            }
            else if (this.predefinedType == typeof(object))
            {
                typeName = "object";
            }
            else if (this.predefinedType == typeof(float))
            {
                typeName = "float";
            }
            else if (this.predefinedType == typeof(double))
            {
                typeName = "double";
            }
            else if (this.predefinedType == typeof(decimal))
            {
                typeName = "decimal";
            }
            else if (this.predefinedType == typeof(char))
            {
                typeName = "char";
            }
            else
            {
                throw new NotSupportedException();
            }

            var node = GetSyntaxNode($"{typeName} x;");

            var statement = (LocalDeclarationStatementSyntax)(((GlobalStatementSyntax)((CompilationUnitSyntax)node).Members.Single()).Statement);
            return (PredefinedTypeSyntax)statement.Declaration.Type;
        }
    }
}
