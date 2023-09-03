// ----------------------------------------------------------------------
// <copyright file="AttributesWalker.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Exceptions;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker
{
    internal class AttributesWalker : CSharpSyntaxWalker
    {
        private readonly IDeclarationResolver resolver;
        private readonly List<IAttributeUse> attributesList;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;

        public AttributesWalker(
            IDeclarationResolver resolver,
            IGenericDeclaration<SyntaxNode> genericDeclaration,
            List<IAttributeUse> attributesList)
        {
            this.resolver = resolver;
            this.attributesList = attributesList;
            this.genericDeclaration = genericDeclaration;
        }

        public override void VisitAttribute(AttributeSyntax node)
        {
            var syntaxNodeProvider = new ParserSyntaxNodeProvider<AttributeSyntax>(node);

            var useWalker = new DeclarationUseWalker(this.resolver, this.genericDeclaration);
            var use = useWalker.Visit(node.Name);

            if (use == null)
            {
                throw new ParserException("Unable to load Declaration use.", node.Name);
            }

            this.attributesList.Add(new AttributeUse(
                use,
                syntaxNodeProvider,
                () =>
                {
                    var namedArguments = new Dictionary<string, object>();

                    if (node.ArgumentList != null)
                    {
                        foreach (var argumentNode in node.ArgumentList.Arguments)
                        {
                            var exp = argumentNode.Expression;

                            var constEvaluator = new ConstantExpressionSyntaxEvaluator<object>(this.resolver, this.genericDeclaration);
                            var value = constEvaluator.Visit(exp);

                            if (argumentNode.NameEquals != null)
                            {
                                var name = argumentNode.NameEquals.Name.Identifier.Text;

                                namedArguments.Add(name, value);
                            }
                            else if (argumentNode.NameColon != null)
                            {
                                var name = argumentNode.NameColon.Name.Identifier.Text;

                                namedArguments.Add(name, value);
                            }
                        }
                    }
                    return namedArguments;
                },
                () =>
                {
                    var constructorArguments = new List<object>();

                    if (node.ArgumentList != null)
                    {
                        foreach (var argumentNode in node.ArgumentList.Arguments)
                        {
                            var exp = argumentNode.Expression;

                            var constEvaluator = new ConstantExpressionSyntaxEvaluator<object>(this.resolver, this.genericDeclaration);
                            var value = constEvaluator.Visit(exp);

                            if (argumentNode.NameEquals == null && argumentNode.NameColon == null)
                            {
                                constructorArguments.Add(value);
                            }
                        }
                    }
                    return constructorArguments;
                }));
        }
    }
}
