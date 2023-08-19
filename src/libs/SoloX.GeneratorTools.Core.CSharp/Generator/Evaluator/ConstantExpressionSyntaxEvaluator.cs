// ----------------------------------------------------------------------
// <copyright file="ConstantExpressionSyntaxEvaluator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator.SubEvaluator;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator
{
    /// <summary>
    /// Evaluate the string constant expression syntax node.
    /// </summary>
    /// <typeparam name="T">Expected value type.</typeparam>
    public class ConstantExpressionSyntaxEvaluator<T> : CSharpSyntaxVisitor<T>
    {
        private static readonly NameOfExpressionSyntaxEvaluator NameOfEvaluator = new NameOfExpressionSyntaxEvaluator();

        private readonly IDeclarationResolver resolver;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;

        /// <summary>
        /// setup instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="genericDeclaration"></param>
        public ConstantExpressionSyntaxEvaluator(IDeclarationResolver resolver, IGenericDeclaration<SyntaxNode> genericDeclaration)
        {
            this.resolver = resolver;
            this.genericDeclaration = genericDeclaration;
        }

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
            if (typeof(T) == typeof(object))
            {
                var type = node.Type.ToString();

                return ConvertToT(new TypeOfExpression(type));
            }
            else
            {
                return ConvertToT(node.Type.ToString());
            }
        }

        /// <inheritdoc/>
        public override T VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            return ConvertToT(node.Token.Value);
        }

        /// <inheritdoc/>
        public override T? VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            var strConstEval = new ConstantExpressionSyntaxEvaluator<string>(this.resolver, this.genericDeclaration);

            var str = string.Empty;
            foreach (var contentItem in node.Contents)
            {
                var word = strConstEval.Visit(contentItem);
                str += word;
            }

            return ConvertToT(str);
        }

        /// <inheritdoc/>
        public override T? VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
        {
            return ConvertToT(node.TextToken.Text);
        }

        /// <inheritdoc/>
        public override T? VisitInterpolation(InterpolationSyntax node)
        {
            var identifier = node.Expression.ToString();

            var constant = this.genericDeclaration.Constants.SingleOrDefault(c => c.Name == identifier);

            var value = Visit(constant.SyntaxNodeProvider.SyntaxNode.Initializer.Value);

            return ConvertToT(value);
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

        private T LoadFromArrayInitializerExpression(InitializerExpressionSyntax initializer)
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

        private T LoadArray<TItem>(InitializerExpressionSyntax initializer)
        {
            var size = initializer.Expressions.Count;
            var values = new TItem[size];

            var evaluator = new ConstantExpressionSyntaxEvaluator<TItem>(this.resolver, this.genericDeclaration);
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
