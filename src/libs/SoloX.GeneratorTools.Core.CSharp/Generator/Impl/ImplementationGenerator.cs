// ----------------------------------------------------------------------
// <copyright file="ImplementationGenerator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.Generator;
using SoloX.GeneratorTools.Core.Generator.Writer.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl
{
    /// <summary>
    /// Entity implementation generator example.
    /// </summary>
    public class ImplementationGenerator : IImplementationGenerator
    {
        private readonly IGenerator generator;
        private readonly IInterfaceDeclaration itfPattern;
        private readonly IGenericDeclaration implPattern;
        private readonly ILocator locator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementationGenerator"/> class.
        /// </summary>
        /// <param name="generator">The generator to use to generate the output.</param>
        /// <param name="locator">Code generation locator.</param>
        /// <param name="itfPattern">The interface pattern to use for the generator.</param>
        /// <param name="implPattern">The implementation pattern to use for the generator.</param>
        public ImplementationGenerator(IGenerator generator, ILocator locator, IInterfaceDeclaration itfPattern, IGenericDeclaration implPattern)
        {
            this.generator = generator;
            this.itfPattern = itfPattern;
            this.implPattern = implPattern;
            this.locator = locator;
        }

        /// <inheritdoc/>
        public void Generate(IInterfaceDeclaration itfDeclaration)
        {
            var propertyWriter = new PropertyWriter(
                this.itfPattern.Properties.Single(),
                itfDeclaration.Properties);

            var implName = GetEntityName(itfDeclaration.Name);

            var (location, nameSpace) = this.locator.ComputeTargetLocation(itfDeclaration.DeclarationNameSpace);

            this.generator.Generate(location, implName, writer =>
            {
                var generatorWalker = new ImplementationGeneratorWalker(
                    writer,
                    this.itfPattern,
                    this.implPattern,
                    itfDeclaration,
                    implName,
                    nameSpace,
                    new WriterSelector(propertyWriter));

                generatorWalker.Visit(this.implPattern.SyntaxNode.SyntaxTree.GetRoot());
            });
        }

        private static string GetEntityName(string name)
        {
            if (name.Length > 1 && name[0] == 'I' && char.IsUpper(name[1]))
            {
                return name.Substring(1);
            }
            else
            {
                return $"{name}Entity";
            }
        }
    }
}
