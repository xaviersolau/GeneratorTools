// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Shouldly;
using SoloX.CodeQuality.Test.Helpers;
using SoloX.CodeQuality.Test.Helpers.Solution;
using SoloX.GeneratorTools.Core.Test.Helpers;

namespace SoloX.GeneratorTools.Core.CSharp.E2ETest
{
    public class CSharpWorkspaceTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public CSharpWorkspaceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ItShouldLoadMetadataAssembly()
        {
            var configurationName = ProjectBinProbe.GetConfiguration<CSharpWorkspaceTest>();
            var framework = ProjectBinProbe.GetFramework<CSharpWorkspaceTest>();

            var root = new RandomGenerator().RandomString(4);

            var solutionName = "GeneratorTest";
            var projectName = "SampleProject";

            IEnumerable<(string key, string value)> replaceTxt = [
                ("SoloX.GeneratorTools.Core.CSharp.Sample3", projectName),
                ];

            var solution = new SolutionBuilder(root, solutionName)
                .WithNugetConfig(@"PkgCache", configuration =>
                {
                    configuration
                        .UsePackageSources(src =>
                        {
                            src.Clear()
                                .AddNugetOrg();
                        });
                })
                .WithProject(projectName, "classlib", framework, configuration =>
                {
                    configuration
                        .UsePackageReference("Microsoft.Extensions.Localization.Abstractions")
                        .UseFiles(files =>
                        {
                            files
                                .Remove("Class1.cs")
                                .Add(@"../Resources/SampleUsingLocalizerAssembly.cs", "SampleUsingLocalizerAssembly.cs", replaceTxt);
                        });
                })
                .Build();

            try
            {
                var processResult = solution.Build(configuration: configurationName);

                processResult.ExitCode.ShouldBe(0);

                var assemblyFile = Path.Combine(root, solutionName, projectName, $"bin/{configurationName}/{framework}/{projectName}.dll");

                Test.Helpers.XUnit.MetadataAssemblyProbe.LoadMetadataAssemblyAndAssert(this.testOutputHelper, assemblyFile, decResolver =>
                {
                    var generatedDecl = decResolver.Find("SampleProject.SampleUsingLocalizerAssembly");

                    generatedDecl.ShouldNotBeNull();
                });
            }
            finally
            {
                Directory.Delete(root, true);
            }
        }
    }
}
