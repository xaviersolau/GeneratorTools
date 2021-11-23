// ----------------------------------------------------------------------
// <copyright file="ConstantExpressionSyntaxEvaluator.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator.SubEvaluator;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator
{
    /// <summary>
    /// Evaluate the string constant expression syntax node.
    /// </summary>
    /// <typeparam name="T">Expected value type.</typeparam>
    public class ConstantExpressionSyntaxEvaluator<T> : CSharpSyntaxVisitor<T>
    {
        private static readonly NameOfExpressionSyntaxEvaluator NameOfEvaluator = new NameOfExpressionSyntaxEvaluator();

#pragma warning disable CA1062 // Valider les arguments de méthodes publiques
        /// <inheritdoc/>
        public override T VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is IdentifierNameSyntax identifier && identifier.Identifier.Text == "nameof")
            {
                var arg = node.ArgumentList.Arguments.Single();

                return ConvertToT(NameOfEvaluator.Visit(arg));
            }

            return base.VisitInvocationExpression(node);
        }

        /// <inheritdoc/>
        public override T VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            return ConvertToT(node.Type.ToString());
        }

        /// <inheritdoc/>
        public override T VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            return ConvertToT(node.Token.Value);
        }

        /// <inheritdoc/>
        public override T VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            return LoadFromArrayInitializerExpression(node.Initializer);
        }

        /// <inheritdoc/>
        public override T VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            return LoadFromArrayInitializerExpression(node.Initializer);
        }
#pragma warning restore CA1062 // Valider les arguments de méthodes publiques

        private static T LoadFromArrayInitializerExpression(InitializerExpressionSyntax initializer)
        {
            if (IsTArrayOfType<string>())
            {
                return LoadArray<string>(initializer);
            }
            else if (IsTArrayOfType<int>())
            {
                return LoadArray<int>(initializer);
            }
            else if (IsTArrayOfType<double>())
            {
                return LoadArray<double>(initializer);
            }

            return default;
        }

        private static T LoadArray<TItem>(InitializerExpressionSyntax initializer)
        {
            var size = initializer.Expressions.Count;
            var values = new TItem[size];

            var evaluator = new ConstantExpressionSyntaxEvaluator<TItem>();
            var idx = 0;
            foreach (var expression in initializer.Expressions)
            {
                var value = evaluator.Visit(expression);
                values[idx++] = value;
            }

            return ConvertToT(values);
        }

        private static T ConvertToT(object value)
        {
            return (value is T t) ? t : default;
        }

        private static bool IsTArrayOfType<TItem>()
            => typeof(T) == typeof(TItem[]) || typeof(T) == typeof(IEnumerable<TItem>);
    }
}
