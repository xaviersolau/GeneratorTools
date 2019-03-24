// ----------------------------------------------------------------------
// <copyright file="NodeWriter.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl
{
    /// <summary>
    /// Property writer implementation.
    /// </summary>
    public class NodeWriter : INodeWriter
    {
        private readonly IInterfaceDeclaration itfPattern;
        private readonly IInterfaceDeclaration declaration;

        private readonly IPropertyDeclaration itfPatternProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeWriter"/> class.
        /// </summary>
        /// <param name="itfPattern">Interface pattern.</param>
        /// <param name="itfDeclaration">Interface declaration to implement.</param>
        public NodeWriter(
            IInterfaceDeclaration itfPattern,
            IInterfaceDeclaration itfDeclaration)
        {
            this.itfPattern = itfPattern;
            this.declaration = itfDeclaration;

            this.itfPatternProperty = this.itfPattern.Properties.Single();
        }

        /// <inheritdoc/>
        public bool Write(CSharpSyntaxNode node, Action<string> write)
        {
            if (node is PropertyDeclarationSyntax propertyNode)
            {
                var propertyName = this.itfPatternProperty.Name;

                if (propertyName != propertyNode.Identifier.Text)
                {
                    return false;
                }

                var lowPropertyName = GetFirstCharLoweredName(propertyName);

                foreach (var itemProperties in this.declaration.Properties)
                {
                    var itemPropertyName = itemProperties.Name;
                    var lowItemPropertyName = GetFirstCharLoweredName(itemPropertyName);
                    write(propertyNode.AttributeLists.ToFullString());
                    write(propertyNode.Modifiers.ToFullString());
                    write($"{itemProperties.PropertyType.SyntaxNode.ToString()} ");
                    write(propertyNode.Identifier.ToFullString()
                        .Replace(propertyName, itemPropertyName));
                    write(propertyNode.AccessorList.ToFullString()
                        .Replace(lowPropertyName, lowItemPropertyName));
                }
            }
            else if (node is FieldDeclarationSyntax fieldNode)
            {
                var variableNode = fieldNode.Declaration.Variables.Single();

                var propertyName = this.itfPatternProperty.Name;
                var lowPropertyName = GetFirstCharLoweredName(propertyName);

                var variableName = variableNode.Identifier.Text;

                if (!variableName.Contains(propertyName) && variableName != lowPropertyName)
                {
                    return false;
                }

                foreach (var itemProperties in this.declaration.Properties)
                {
                    var itemPropertyName = itemProperties.Name;
                    var lowItemPropertyName = GetFirstCharLoweredName(itemPropertyName);

                    write(fieldNode.AttributeLists.ToFullString());
                    write(fieldNode.Modifiers.ToFullString());
                    write($"{itemProperties.PropertyType.SyntaxNode.ToString()} ");
                    write(variableNode.Identifier.ToFullString()
                        .Replace(propertyName, itemPropertyName)
                        .Replace(lowPropertyName, lowItemPropertyName));
                    if (variableNode.Initializer != null)
                    {
                        write(variableNode.Initializer.ToFullString());
                    }

                    write(fieldNode.SemicolonToken.ToFullString());
                }
            }

            return true;
        }

        private static string GetFirstCharLoweredName(string name)
        {
            return $"{char.ToLowerInvariant(name[0])}{name.Substring(1)}";
        }
    }
}
