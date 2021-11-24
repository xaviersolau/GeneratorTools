// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.CodeQuality.Test.Helpers.XUnit.Logger;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace
{
    public class CSharpWorkspaceTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public CSharpWorkspaceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RegisterProjectTest()
        {
            var projectName1 = "Test1";
            var projectName2 = "Test2";

            var workingDir = Path.GetFullPath(Environment.CurrentDirectory);

            var factoryMock = new Mock<ICSharpFactory>();
            factoryMock.Setup(f => f.CreateProject(It.IsAny<string>()))
                .Returns<string>(f => Mock.Of<ICSharpProject>());

            var loaderMock = new Mock<ICSharpLoader>();

            using (var workspace = new CSharpWorkspace(
                new TestLogger<CSharpWorkspace>(this.testOutputHelper),
                factoryMock.Object,
                loaderMock.Object))
            {
                var project1 = workspace.RegisterProject(projectName1);
                var project2 = workspace.RegisterProject(projectName1);

                Assert.Same(project1, project2);

                var project3 = workspace.RegisterProject(projectName2);

                Assert.NotSame(project1, project3);

                factoryMock.Verify(f => f.CreateProject(Path.Combine(workingDir, projectName1)));
                factoryMock.Verify(f => f.CreateProject(Path.Combine(workingDir, projectName2)));

                loaderMock.Verify(l => l.Load(workspace, project1));
                loaderMock.Verify(l => l.Load(workspace, project3));
            }
        }

        [Fact]
        public void DeepLoadTest()
        {
            var factoryMock = new Mock<ICSharpFactory>();
            var loaderMock = new Mock<ICSharpLoader>();

            using (var workspace = new CSharpWorkspace(
                new TestLogger<CSharpWorkspace>(this.testOutputHelper),
                factoryMock.Object,
                loaderMock.Object))
            {
                var fileMock = new Mock<ICSharpFile>();

                var declaration = DeclarationHelper.SetupDeclaration<IDeclaration<SyntaxNode>>("nameSpace", "name");

                fileMock.SetupGet(f => f.Declarations).Returns(new IDeclaration<SyntaxNode>[] { declaration });

                factoryMock.Setup(f => f.CreateFile(It.IsAny<string>()))
                    .Returns<string>(f => fileMock.Object);

                workspace.RegisterFile("Test");

                workspace.DeepLoad();

                loaderMock.Verify(l => l.Load(It.IsAny<IDeclarationResolver>(), declaration));
            }
        }
    }
}