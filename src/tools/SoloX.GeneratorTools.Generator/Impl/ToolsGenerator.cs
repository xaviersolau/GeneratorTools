// ----------------------------------------------------------------------
// <copyright file="ToolsGenerator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Utils;
using SoloX.GeneratorTools.Generator.Patterns.Impl;
using SoloX.GeneratorTools.Generator.Patterns.Itf;

namespace SoloX.GeneratorTools.Generator.Impl
{
    /// <summary>
    /// Generator Tools implementation.
    /// </summary>
    public class ToolsGenerator : IToolsGenerator
    {
        private readonly IGeneratorLogger<ToolsGenerator> logger;
        private readonly ICSharpWorkspaceFactory workspaceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolsGenerator"/> class.
        /// </summary>
        /// <param name="logger">Logger that will be used for logs.</param>
        /// <param name="workspaceFactory">The workspace to use.</param>
        public ToolsGenerator(IGeneratorLogger<ToolsGenerator> logger, ICSharpWorkspaceFactory workspaceFactory)
        {
            this.logger = logger;
            this.workspaceFactory = workspaceFactory;
        }

        /// <inheritdoc/>
        public void Generate(string projectFile)
        {
            var projectFolder = Path.GetDirectoryName(projectFile);

            this.logger.LogInformation($"Loading {Path.GetFileName(projectFile)}...");

            var workspace = this.workspaceFactory.CreateWorkspace();

            var project = workspace.RegisterProject(projectFile);

            var locator = new RelativeLocator(projectFolder, project.RootNameSpace, suffix: "Impl");
            var fileGenerator = new FileWriter(".generated.cs");

            // Generate with a filter on current project interface declarations.
            this.Generate(
                workspace,
                locator,
                fileGenerator,
                project.Files);
        }

        internal void Generate(ICSharpWorkspace workspace, RelativeLocator locator, IWriter fileGenerator, IEnumerable<ICSharpFile> files)
        {
            workspace.RegisterFile(GetContentFile("./Patterns/Itf/IObjectPattern.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Itf/IFactoryPattern.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/FactoryPattern.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ObjectPattern.cs"));

            var resolver = workspace.DeepLoad();

            var generator1 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(IFactoryPattern),
                this.logger);

            var generatedItems1 = generator1.Generate(files);

            var generator2 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(FactoryPattern),
                this.logger);

            var generatedItems2 = generator2.Generate(files);

            var generator3 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(ObjectPattern),
                this.logger);

            var generatedItems3 = generator3.Generate(files);
        }

        private static string GetContentFile(string contentFile)
        {
            return Path.Combine(Path.GetDirectoryName(typeof(ToolsGenerator).Assembly.Location), contentFile);
        }
    }
}
