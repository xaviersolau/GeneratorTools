// ----------------------------------------------------------------------
// <copyright file="PropertyWriter.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.Generator.Writer;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl
{
    /// <summary>
    /// Property writer implementation.
    /// </summary>
    public class PropertyWriter : CSharpSyntaxVisitor<bool>, INodeWriter
    {
        private readonly IReadOnlyCollection<IPropertyDeclaration> declarationProperties;
        private readonly IPropertyDeclaration itfPatternProperty;

        private Action<string> write;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyWriter"/> class.
        /// </summary>
        /// <param name="itfPatternProperty">Interface property pattern.</param>
        /// <param name="itfDeclarationProperties">Interface declaration properties to implement.</param>
        public PropertyWriter(
            IPropertyDeclaration itfPatternProperty,
            IReadOnlyCollection<IPropertyDeclaration> itfDeclarationProperties)
        {
            this.declarationProperties = itfDeclarationProperties;

            this.itfPatternProperty = itfPatternProperty;
        }

        /// <inheritdoc/>
        public bool Write(SyntaxNode node, Action<string> write)
        {
            this.write = write;
            var res = this.Visit(node);
            this.write = null;
            return res;
        }

        /// <inheritdoc/>
        public bool Write(SyntaxToken token, Action<string> write)
        {
            return false;
        }

        /// <inheritdoc/>
        public override bool VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var propertyName = this.itfPatternProperty.Name;

            if (propertyName != node.Identifier.Text)
            {
                return false;
            }

            var lowPropertyName = GetFirstCharLoweredName(propertyName);

            foreach (var itemProperties in this.declarationProperties)
            {
                var itemPropertyName = itemProperties.Name;
                var lowItemPropertyName = GetFirstCharLoweredName(itemPropertyName);
                this.write(node.AttributeLists.ToFullString());
                this.write(node.Modifiers.ToFullString());

                var implType = node.Type.ToFullString();
                var itfType = this.itfPatternProperty.PropertyType.SyntaxNode.ToString();

                var declType = itemProperties.PropertyType.SyntaxNode.ToString();

                this.write(implType.Replace(itfType, declType));

                this.write(node.Identifier.ToFullString()
                    .Replace(propertyName, itemPropertyName));
                this.write(node.AccessorList.ToFullString()
                    .Replace(propertyName, itemPropertyName)
                    .Replace(lowPropertyName, lowItemPropertyName));
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            var variableNode = node.Declaration.Variables.Single();

            var propertyName = this.itfPatternProperty.Name;
            var lowPropertyName = GetFirstCharLoweredName(propertyName);

            var variableName = variableNode.Identifier.Text;

            if (!variableName.Contains(propertyName) && !variableName.StartsWith(lowPropertyName, StringComparison.InvariantCulture))
            {
                return false;
            }

            foreach (var itemProperties in this.declarationProperties)
            {
                var itemPropertyName = itemProperties.Name;
                var lowItemPropertyName = GetFirstCharLoweredName(itemPropertyName);

                this.write(node.AttributeLists.ToFullString());
                this.write(node.Modifiers.ToFullString());

                var implType = node.Declaration.Type.ToFullString();
                var itfType = this.itfPatternProperty.PropertyType.SyntaxNode.ToString();

                var declType = itemProperties.PropertyType.SyntaxNode.ToString();

                this.write(implType.Replace(itfType, declType));

                this.write(variableNode.Identifier.ToFullString()
                    .Replace(propertyName, itemPropertyName)
                    .Replace(lowPropertyName, lowItemPropertyName));
                if (variableNode.Initializer != null)
                {
                    this.write(variableNode.Initializer.ToFullString());
                }

                this.write(node.SemicolonToken.ToFullString());
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var propertyName = this.itfPatternProperty.Name;
            var lowPropertyName = GetFirstCharLoweredName(propertyName);

            var leftTxt = node.Left.ToFullString();
            var rightTxt = node.Right.ToFullString();

            var leftMatch = leftTxt.Contains(lowPropertyName) || leftTxt.Contains(propertyName);
            var rightMatch = rightTxt.Contains(lowPropertyName) || rightTxt.Contains(propertyName);

            if ((!leftMatch && !rightMatch) || (leftMatch && rightMatch))
            {
                return false;
            }

            if (rightMatch)
            {
                this.write(leftTxt);

                foreach (var itemProperties in this.declarationProperties)
                {
                    this.write(node.OperatorToken.ToFullString());

                    var itemPropertyName = itemProperties.Name;
                    var lowItemPropertyName = GetFirstCharLoweredName(itemPropertyName);

                    this.write(rightTxt
                        .Replace(propertyName, itemPropertyName)
                        .Replace(lowPropertyName, lowItemPropertyName));
                }
            }

            if (leftMatch)
            {
                foreach (var itemProperties in this.declarationProperties)
                {
                    var itemPropertyName = itemProperties.Name;
                    var lowItemPropertyName = GetFirstCharLoweredName(itemPropertyName);

                    this.write(leftTxt
                        .Replace(propertyName, itemPropertyName)
                        .Replace(lowPropertyName, lowItemPropertyName));

                    this.write(node.OperatorToken.ToFullString());
                }

                this.write(rightTxt);
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            return this.VisitStatement(node);
        }

        /// <inheritdoc/>
        public override bool VisitIfStatement(IfStatementSyntax node)
        {
            return this.VisitStatement(node);
        }

        /// <inheritdoc/>
        public override bool VisitForEachStatement(ForEachStatementSyntax node)
        {
            return this.VisitStatement(node);
        }

        /// <inheritdoc/>
        public override bool VisitThrowStatement(ThrowStatementSyntax node)
        {
            return this.VisitStatement(node);
        }

        /// <inheritdoc/>
        public override bool VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            return this.VisitStatement(node);
        }

        private static string GetFirstCharLoweredName(string name)
        {
            return $"{char.ToLowerInvariant(name[0])}{name.Substring(1)}";
        }

        private bool VisitStatement(SyntaxNode node)
        {
            var propertyName = this.itfPatternProperty.Name;
            var lowPropertyName = GetFirstCharLoweredName(propertyName);

            var txt = node.ToFullString();

            if (!txt.Contains(lowPropertyName) && !txt.Contains(propertyName))
            {
                return false;
            }

            foreach (var itemProperties in this.declarationProperties)
            {
                var itemPropertyName = itemProperties.Name;
                var lowItemPropertyName = GetFirstCharLoweredName(itemPropertyName);

                this.write(txt
                    .Replace(propertyName, itemPropertyName)
                    .Replace(lowPropertyName, lowItemPropertyName));
            }

            return true;
        }
    }
}
