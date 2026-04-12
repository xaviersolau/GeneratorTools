// ----------------------------------------------------------------------
// <copyright file="CSharpProjectTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Linq;
using NSubstitute;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace
{
    public class CSharpProjectTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public CSharpProjectTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void LoadCSharpProjectTest()
        {
            var projectFile = @"../../../../SoloX.GeneratorTools.Core.CSharp.Sample2/SoloX.GeneratorTools.Core.CSharp.Sample2.csproj";

            var workspaceMock = Substitute.For<ICSharpWorkspace>();

            workspaceMock
                .RegisterProject(Arg.Any<string>())
                .Returns(ci => new CSharpProject(ci.Arg<string>()));

            workspaceMock
                .RegisterFile(Arg.Any<string>(), Arg.Any<IGlobalUsingDirectives>())
                .Returns(ci => new CSharpFile(
                    ci.Arg<string>(),
                    DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                    ci.Arg<IGlobalUsingDirectives>()));

            var project = new CSharpProject(projectFile);
            project.Load(workspaceMock);

            Assert.Single(project.ProjectReferences);

            var projectReference = project.ProjectReferences.Single();
            Assert.Equal("SoloX.GeneratorTools.Core.CSharp.Sample1.csproj", projectReference.ProjectFile);

            Assert.Single(project.Files);

            var csFile = project.Files.Single();
            Assert.Equal("Sample2Class1.cs", csFile.FileName);

            var ns = project.RootNameSpace;
            Assert.Equal("SoloX.GeneratorTools.Core.CSharp.Sample2", ns);
        }
    }
}
