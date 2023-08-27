// ----------------------------------------------------------------------
// <copyright file="EntityGeneratorExample.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Impl;
using SoloX.GeneratorTools.Core.CSharp.Extensions.Utils;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator;
using SoloX.GeneratorTools.Core.Generator.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Examples
{
    /// <summary>
    /// Entity generator example class to show a simple use case of a pattern based class generator.
    /// </summary>
    public class EntityGeneratorExample
    {
        private readonly ILogger<EntityGeneratorExample> logger;
        private readonly ICSharpWorkspaceFactory workspaceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityGeneratorExample"/> class.
        /// </summary>
        /// <param name="logger">The logger to log the output messages.</param>
        /// <param name="workspaceFactory">The factory to use to create a workspace to load the project data.</param>
        public EntityGeneratorExample(ILogger<EntityGeneratorExample> logger, ICSharpWorkspaceFactory workspaceFactory)
        {
            this.logger = logger;
            this.workspaceFactory = workspaceFactory;
        }

        /// <summary>
        /// Process the generator.
        /// </summary>
        /// <param name="projectFile">The project file to work from.</param>
        public void Generate(string projectFile)
        {
            this.logger.LogInformation($"Loading {Path.GetFileName(projectFile)}...");

            var projectFolder = Path.GetDirectoryName(projectFile);

            // Create a Workspace
            var workspace = this.workspaceFactory.CreateWorkspace();

            // First we need to register the project.
            var project = workspace.RegisterProject(projectFile);

            // Register the pattern interface.
            var patternInterfaceDeclaration = workspace.RegisterFile("./Patterns/Itf/IEntityPattern.cs")
                .Declarations.Single() as IInterfaceDeclaration;

            // Register the pattern implementation.
            var patternImplementationDeclaration = workspace.RegisterFile("./Patterns/Impl/EntityPattern.cs")
                .Declarations.Single() as IGenericDeclaration<SyntaxNode>;

            // Load the project and its project dependencies. (Note that for now we only load the sources.
            // The binary assembly dependencies are not taken into account)
            var resolver = workspace.DeepLoad();

            // Setup a locator that will tell the location where the generated classes must be written.
            var locator = new RelativeLocator(projectFolder, project.RootNameSpace, suffix: "Impl");

            // Setup a selector resolver.
            var selectorResolver = new DefaultSelectorResolver();

            // Create the automated generator.
            var generator = new AutomatedGenerator(
                new FileWriter(".generated.cs"),
                locator,
                resolver,
                typeof(EntityPattern),
                selectorResolver,
                new GeneratorLogger<EntityGeneratorExample>(this.logger));

            // Generate the files.
            generator.Generate(project.Files);
        }
    }
}
