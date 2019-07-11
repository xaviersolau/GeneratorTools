// ----------------------------------------------------------------------
// <copyright file="ModelGeneratorExample.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Examples
{
    /// <summary>
    /// The model example generator.
    /// </summary>
    public class ModelGeneratorExample
    {
        private ILogger<ModelGeneratorExample> logger;
        private ICSharpWorkspace workspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGeneratorExample"/> class.
        /// </summary>
        /// <param name="logger">The logger to log the output messages.</param>
        /// <param name="workspace">The workspace to use to load the project data.</param>
        public ModelGeneratorExample(ILogger<ModelGeneratorExample> logger, ICSharpWorkspace workspace)
        {
            this.logger = logger;
            this.workspace = workspace;
        }

        /// <summary>
        /// Process the generator.
        /// </summary>
        /// <param name="projectFile">The project file to work from.</param>
        public void Generate(string projectFile)
        {
            this.logger.LogInformation($"Loading {Path.GetFileName(projectFile)}...");

            var projectFolder = Path.GetDirectoryName(projectFile);

            // First we need to register the project.
            var project = this.workspace.RegisterProject(projectFile);

            // Register the pattern interface.
            var patternInterfaceDeclaration = this.workspace.RegisterFile("./Patterns/Itf/IModelPattern.cs")
                .Declarations.Single() as IInterfaceDeclaration;

            // Register the pattern implementation.
            var patternImplementationDeclaration = this.workspace.RegisterFile("./Patterns/Impl/ModelPattern.cs")
                .Declarations.Single() as IGenericDeclaration<SyntaxNode>;

            // Load the project and its project dependencies. (Note that for now we only load the sources.
            // The binary assembly dependencies are not taken into account)
            var resolver = this.workspace.DeepLoad();

            // Get the base interface in order to find all extended interfaces that need to be implemented.
            var modelBaseInterface = resolver.Find("SoloX.GeneratorTools.Core.CSharp.Examples.Core.IModelBase").Single() as IGenericDeclaration<SyntaxNode>;

            // Setup a locator that will tell the location where the generated classes must be written.
            var locator = new RelativeLocator(projectFolder, project.RootNameSpace, suffix: "Impl");

            // Create the Implementation Generator with a file generator, the locator and the pattern interface/class.
            var generator = new ImplementationGenerator(
                new FileGenerator(".generated.cs"),
                locator,
                patternInterfaceDeclaration,
                patternImplementationDeclaration);

            // Loop on all interface extending the base interface.
            foreach (var modelInterface in modelBaseInterface.ExtendedBy.Where(d => d != patternInterfaceDeclaration))
            {
                this.logger.LogInformation(modelInterface.FullName);

                var implName = GeneratorHelper.ComputeClassName(modelInterface.Name);

                // Create the property writer what will use all properties from the model interface to generate
                // and write the corresponding code depending on the given patterns.
                var propertyWriter = new PropertyWriter(
                    patternInterfaceDeclaration.Properties.Single(),
                    modelInterface.Properties);

                // Setup some basic text replacement writer.
                var itfNameWriter = new StringReplaceWriter(patternInterfaceDeclaration.Name, modelInterface.Name);
                var implNameWriter = new StringReplaceWriter(patternImplementationDeclaration.Name, implName);

                // Create the writer selector.
                var writerSelector = new WriterSelector(propertyWriter, itfNameWriter, implNameWriter);

                // And generate the class implementation.
                generator.Generate(writerSelector, (IInterfaceDeclaration)modelInterface, implName);
            }
        }
    }
}