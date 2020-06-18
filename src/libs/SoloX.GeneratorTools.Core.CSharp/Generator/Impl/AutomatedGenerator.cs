﻿// ----------------------------------------------------------------------
// <copyright file="AutomatedGenerator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator;
using SoloX.GeneratorTools.Core.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl
{
    /// <summary>
    /// Automated generator that will generate a code from the given pattern.
    /// </summary>
    public class AutomatedGenerator : IAutomatedGenerator
    {
        private readonly IGenerator generator;
        private readonly ILocator locator;
        private readonly IDeclarationResolver resolver;
        private readonly Type patternType;
        private readonly PatternAttribute patternAttribute;
        private readonly IDeclaration<SyntaxNode> pattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomatedGenerator"/> class.
        /// </summary>
        /// <param name="generator">The generator to use to generate the output.</param>
        /// <param name="locator">Code generation locator.</param>
        /// <param name="resolver">The resolver to resolve workspace symbols.</param>
        /// <param name="patternType">The pattern type to use.</param>
        public AutomatedGenerator(IGenerator generator, ILocator locator, IDeclarationResolver resolver, Type patternType)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            if (locator == null)
            {
                throw new ArgumentNullException(nameof(locator));
            }

            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            if (patternType == null)
            {
                throw new ArgumentNullException(nameof(patternType));
            }

            this.generator = generator;
            this.resolver = resolver;
            this.locator = locator;
            this.patternType = patternType;

            this.patternAttribute = FindAttribute<PatternAttribute>(this.patternType);

            // Get the pattern as source declaration from the given resolver.
            var factoryPatternItfDecl = resolver
                .Find(patternType.FullName)
                .Single();

            this.pattern = factoryPatternItfDecl;
        }

        /// <inheritdoc/>
        public IEnumerable<IGeneratedItem> Generate(IEnumerable<ICSharpFile> files)
        {
            var generatedItems = new List<IGeneratedItem>();
            var selector = this.patternAttribute.Selector;

            var declarations = selector.GetDeclarations(files);

            var repeatAttribute = FindAttribute<RepeatAttribute>(this.patternType);
            if (repeatAttribute != null)
            {
                var patternPattern = this.resolver.Resolve(repeatAttribute.RepeatPattern, this.pattern);

                foreach (var declaration in declarations)
                {
                    var (location, nameSpace) = this.locator.ComputeTargetLocation(declaration.DeclarationNameSpace);

                    var strategy = new AutomatedGenericStrategy(
                        (IGenericDeclaration<SyntaxNode>)patternPattern,
                        (IGenericDeclaration<SyntaxNode>)declaration,
                        this.resolver);

                    var implName = strategy.ComputeTargetName();

                    this.generator.Generate(location, implName, writer =>
                    {
                        var generatorWalker = new AutomatedWalker(
                            writer,
                            this.pattern,
                            strategy);

                        generatorWalker.Visit(this.pattern.SyntaxNodeProvider.SyntaxNode.SyntaxTree.GetRoot());
                    });
                }
            }
            else
            {
                var (location, nameSpace) = this.locator.ComputeTargetLocation(string.Empty);

                var strategy = new AutomatedDeclarationsStrategy(
                    this.pattern,
                    nameSpace,
                    this.pattern.Name.Replace("Pattern", string.Empty),
                    declarations,
                    this.resolver);

                this.generator.Generate(location, strategy.ComputeTargetName(), writer =>
                {
                    var generatorWalker = new AutomatedWalker(
                        writer,
                        this.pattern,
                        strategy);

                    generatorWalker.Visit(this.pattern.SyntaxNodeProvider.SyntaxNode.SyntaxTree.GetRoot());
                });
            }

            return generatedItems;
        }

        private static TAttribute FindAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), false);
            return (TAttribute)attributes.FirstOrDefault();
        }
    }
}
