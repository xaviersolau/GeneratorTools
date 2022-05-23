// ----------------------------------------------------------------------
// <copyright file="ImplementationGeneratorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.CodeQuality.Test.Helpers.XUnit;
using SoloX.GeneratorTools.Core.CSharp.Extentions;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers.Snapshot;
using SoloX.GeneratorTools.Generator.Impl;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Generator.ITest.Generator
{
    public class ImplementationGeneratorTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ImplementationGeneratorTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(@"Resources/Data/ISimpleObject.cs")]
        public void GenerateSimpleTest(string interfaceFile)
        {
            var snapshotName = nameof(this.GenerateSimpleTest)
                + Path.GetFileNameWithoutExtension(interfaceFile);

            this.GenerateSnapshot(snapshotName, interfaceFile);
        }

        private void GenerateSnapshot(string snapshotName, params string[] files)
        {
            var sc = new ServiceCollection();
            sc.AddTestLogging(this.testOutputHelper);
            sc.AddCSharpToolsGenerator();

            using (var sp = sc.BuildServiceProvider())
            {
                var workspaceFactory = sp.GetService<ICSharpWorkspaceFactory>();

                var workspace = workspaceFactory.CreateWorkspace();

                foreach (var file in files)
                {
                    workspace.RegisterFile(file);
                }

                var generator = new ToolsGenerator(
                    sp.GetService<ILogger<ToolsGenerator>>(),
                    workspace);

                var inputs = new HashSet<string>();
                var locator = new RelativeLocator(string.Empty, "target.name.space");

                var snapshotGenerator = new SnapshotWriter();

                generator.Generate(locator, snapshotGenerator, workspace.Files);

                var location = SnapshotHelper.GetLocationFromCallingCodeProjectRoot(null);
                SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), snapshotName, location);
            }
        }
    }
}
