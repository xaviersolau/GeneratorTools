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
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;
using System;
using SoloX.GeneratorTools.Core.CSharp.Exceptions;

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
            var useWalker = new DeclarationUseWalker(this.resolver, this.genericDeclaration);
            var use = useWalker.Visit(node.Type);

            if (use == null)
            {
                throw new ParserException("Unable to load Declaration use.", node.Type);
            }

            if (typeof(T) == typeof(string))
            {
                return ConvertToT(use.Declaration.FullName);
            }
            else
            {
                return ConvertToT(use);
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
            if (node.Type.ElementType.Kind() == SyntaxKind.PredefinedType)
            {
                return LoadFromArrayInitializerExpression(node.Initializer, node.Type.ElementType.ToString());
            }
            else
            {
                var typeIdentifier = node.Type.ElementType.ToString();
                var decl = this.resolver.Resolve(typeIdentifier, this.genericDeclaration);

                if (decl != null)
                {
                    return LoadFromArrayInitializerExpression(node.Initializer, decl.FullName);
                }
                else
                {
                    return LoadFromArrayInitializerExpression(node.Initializer);
                }
            }
        }

        /// <inheritdoc/>
        public override T VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            return LoadFromArrayInitializerExpression(node.Initializer);
        }
#pragma warning restore CA1062 // Valider les arguments de méthodes publiques

        private static readonly IReadOnlyDictionary<string, Type> PredefinedTypeMap =
            new Dictionary<string, Type>
            {
                ["string"] = typeof(string),
                ["System.String"] = typeof(string),
                ["char"] = typeof(char),
                ["int"] = typeof(int),
                ["double"] = typeof(double),
                ["decimal"] = typeof(decimal),
                ["object"] = typeof(object),
            };

        private static readonly IReadOnlyDictionary<Type, Func<ConstantExpressionSyntaxEvaluator<T>, InitializerExpressionSyntax, T>> TypeToLoaderMap =
            new Dictionary<Type, Func<ConstantExpressionSyntaxEvaluator<T>, InitializerExpressionSyntax, T>>
            {
                [typeof(string)] = (l, i) => l.LoadArray<string>(i),
                [typeof(char)] = (l, i) => l.LoadArray<char>(i),
                [typeof(int)] = (l, i) => l.LoadArray<int>(i),
                [typeof(double)] = (l, i) => l.LoadArray<double>(i),
                [typeof(decimal)] = (l, i) => l.LoadArray<decimal>(i),
                [typeof(object)] = (l, i) => l.LoadArray<object>(i),
            };

        private T LoadFromArrayInitializerExpression(InitializerExpressionSyntax initializer, string typeName)
        {
            if (PredefinedTypeMap.TryGetValue(typeName, out var type))
            {
                return LoadArray(initializer, type);
            }

            return default;
        }

        private T LoadFromArrayInitializerExpression(InitializerExpressionSyntax initializer)
        {
            if (typeof(T) == typeof(object))
            {
                // Prob
                var size = initializer.Expressions.Count;
                if (size == 0)
                {
                    return ConvertToT(Array.Empty<object>());
                }
                else
                {
                    var first = Visit(initializer.Expressions[0]);

                    if (first != null)
                    {
                        return LoadArray(initializer, first.GetType());
                    }
                }
            }

            foreach (var typeItem in TypeToLoaderMap)
            {
                if (IsTArrayOfType(typeItem.Key))
                {
                    return typeItem.Value(this, initializer);
                }
            }

            return default;
        }

        private T LoadArray(InitializerExpressionSyntax initializer, Type eltType)
        {
            if (TypeToLoaderMap.TryGetValue(eltType, out var func))
            {
                return func(this, initializer);
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

        private static bool IsTArrayOfType(Type typeItem)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(typeItem);
            return enumerableType.IsAssignableFrom(typeof(T));
        }
    }
}
