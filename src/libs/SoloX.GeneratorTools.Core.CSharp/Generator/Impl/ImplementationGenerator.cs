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
using SoloX.GeneratorTools.Core.Generator.Writer;
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
        public void Generate(IWriterSelector writerSelector, IInterfaceDeclaration itfDeclaration, string implName)
        {
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
                    writerSelector);

                generatorWalker.Visit(this.implPattern.SyntaxNode.SyntaxTree.GetRoot());
            });
        }
    }
}
