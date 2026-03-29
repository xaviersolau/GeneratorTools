// ----------------------------------------------------------------------
// <copyright file="ImplementationGeneratorTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SoloX.CodeQuality.Test.Helpers.Snapshot;
using SoloX.CodeQuality.Test.Helpers.XUnit.V3;
using SoloX.GeneratorTools.Core.CSharp.Extensions;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers.Snapshot;
using SoloX.GeneratorTools.Core.Utils;
using SoloX.GeneratorTools.Generator.Impl;
using Xunit;

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
        public async Task GenerateSimpleTest(string interfaceFile)
        {
            var snapshotName = nameof(this.GenerateSimpleTest)
                + Path.GetFileNameWithoutExtension(interfaceFile);

            await GenerateSnapshot(snapshotName, interfaceFile);
        }

        private async Task GenerateSnapshot(string snapshotName, params string[] files)
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
                workspace.RegisterAssemblyTypes(typeof(AllSelector).Assembly,
                    new Type[] {
                        typeof(AllSelector), typeof(InterfaceBasedOnSelector<>), typeof(ReadOnlyPropertySelector), typeof(ReadWritePropertySelector)
                    });

                var generator = new ToolsGenerator(
                    sp.GetService<IGeneratorLogger<ToolsGenerator>>(),
                    workspaceFactory);

                var inputs = new HashSet<string>();
                var locator = new RelativeLocator(string.Empty, "target.name.space");

                var snapshotGenerator = new SnapshotWriter();

                generator.Generate(workspace, locator, snapshotGenerator, workspace.Files);

                var snapshotTest = SnapshotTestBuilder
                    .Create()
                    .WithThisFilePathLocation()
                    .WithTextStrategy()
                    .Build();

                await snapshotTest.CompareSnapshotAsync(snapshotName, snapshotGenerator.GetAllGenerated(), forceReplaceSnapshot: false).ConfigureAwait(false);
            }
        }
    }
}
