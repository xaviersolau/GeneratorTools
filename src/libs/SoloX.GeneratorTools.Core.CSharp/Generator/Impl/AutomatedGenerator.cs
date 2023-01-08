// ----------------------------------------------------------------------
// <copyright file="AutomatedGenerator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
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
        private readonly IWriter writer;
        private readonly ILocator locator;
        private readonly IDeclarationResolver resolver;
        private readonly Type patternType;
        private readonly ISelectorResolver selectorResolver;
        private readonly IGeneratorLogger logger;
        private readonly PatternAttribute patternAttribute;
        private readonly IDeclaration<SyntaxNode> pattern;
        private readonly List<string> ignoreUsingList = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomatedGenerator"/> class.
        /// </summary>
        /// <param name="writer">The writer to use to generate the output.</param>
        /// <param name="locator">Code generation locator.</param>
        /// <param name="resolver">The resolver to resolve workspace symbols.</param>
        /// <param name="patternType">The pattern type to use.</param>
        /// <param name="selectorResolver">Selector resolver or null to use the default one.</param>
        /// <param name="logger">Logger instance.</param>
        public AutomatedGenerator(IWriter writer, ILocator locator, IDeclarationResolver resolver, Type patternType, ISelectorResolver selectorResolver, IGeneratorLogger logger)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
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

            this.writer = writer;
            this.resolver = resolver;
            this.locator = locator;
            this.patternType = patternType;
            this.selectorResolver = selectorResolver;
            this.logger = logger;
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

            this.logger.LogDebug($"Selected declarations: {string.Join(", ", declarations.Select(d => d.Name))}");

            var replacePatternHandlerAttributes = FindAttributes<ReplacePatternAttribute>(this.patternType);
            var replacePatternHandlerFactories = replacePatternHandlerAttributes.Select(a => a.ReplacePatternHandlerFactory);

            var repeatAttribute = FindAttribute<RepeatAttribute>(this.patternType);
            if (repeatAttribute != null)
            {
                this.logger.LogDebug($"Repeat attribute detected: {repeatAttribute.RepeatPattern}");

                var patternPattern = this.resolver.Resolve(repeatAttribute.RepeatPattern, this.pattern);

                foreach (var declaration in declarations)
                {
                    this.logger.LogDebug($"Repeat item: {declaration.Name}");

                    var (location, nameSpace) = this.locator.ComputeTargetLocation(declaration.DeclarationNameSpace);

                    var strategy = new AutomatedGenericStrategy(
                        (IGenericDeclaration<SyntaxNode>)patternPattern,
                        (IGenericDeclaration<SyntaxNode>)declaration,
                        this.resolver,
                        replacePatternHandlerFactories,
                        this.ignoreUsingList,
                        this.selectorResolver);

                    var implName = strategy.ApplyPatternReplace(this.pattern.Name);

                    this.writer.Generate(location, implName, writer =>
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
                this.logger.LogDebug($"No Repeat attribute detected");

                var (location, nameSpace) = this.locator.ComputeTargetLocation(string.Empty);

                var strategy = new AutomatedDeclarationsStrategy(
                    this.pattern,
                    nameSpace,
                    this.pattern.Name.Replace("Pattern", string.Empty),
                    declarations,
                    this.resolver,
                    replacePatternHandlerFactories,
                    this.ignoreUsingList,
                    this.selectorResolver);

                this.writer.Generate(location, strategy.ComputeTargetName(), writer =>
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

        /// <inheritdoc/>
        public void AddIgnoreUsing(params string[] usingToIgnore)
        {
            this.ignoreUsingList.AddRange(usingToIgnore);
        }

        private static TAttribute FindAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), false);
            return (TAttribute)attributes.FirstOrDefault();
        }

        private static IEnumerable<TAttribute> FindAttributes<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), false);
            return attributes.Cast<TAttribute>();
        }
    }
}
