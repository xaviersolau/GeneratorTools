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
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers;
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

            var projectLoader1 = new Mock<ICSharpWorkspaceItemLoader<ICSharpProject>>();
            var projectLoader2 = new Mock<ICSharpWorkspaceItemLoader<ICSharpProject>>();

            var factoryMock = new Mock<ICSharpWorkspaceItemFactory>();
            factoryMock.Setup(f => f.CreateProject(Path.GetFullPath(projectName1)))
                .Returns(projectLoader1.Object);
            factoryMock.Setup(f => f.CreateProject(Path.GetFullPath(projectName2)))
                .Returns(projectLoader2.Object);

            projectLoader1.SetupGet(x => x.WorkspaceItem).Returns(Mock.Of<ICSharpProject>());
            projectLoader2.SetupGet(x => x.WorkspaceItem).Returns(Mock.Of<ICSharpProject>());

            var workspace = new CSharpWorkspace(
                LoggerHelper.CreateGeneratorLogger<CSharpWorkspace>(this.testOutputHelper),
                factoryMock.Object);

            var project1 = workspace.RegisterProject(projectName1);
            var project2 = workspace.RegisterProject(projectName1);

            Assert.Same(project1, project2);

            var project3 = workspace.RegisterProject(projectName2);

            Assert.NotSame(project1, project3);

            factoryMock.Verify(f => f.CreateProject(Path.Combine(workingDir, projectName1)));
            factoryMock.Verify(f => f.CreateProject(Path.Combine(workingDir, projectName2)));

            projectLoader1.Verify(l => l.Load(workspace));
            projectLoader2.Verify(l => l.Load(workspace));
        }

        [Fact]
        public void DeepLoadTest()
        {
            var factoryMock = new Mock<ICSharpWorkspaceItemFactory>();

            var workspace = new CSharpWorkspace(
                LoggerHelper.CreateGeneratorLogger<CSharpWorkspace>(this.testOutputHelper),
                factoryMock.Object);

            var fileloaderMock = new Mock<ICSharpWorkspaceItemLoader<ICSharpFile>>();
            var fileMock = new Mock<ICSharpFile>();

            fileloaderMock.SetupGet(x => x.WorkspaceItem).Returns(fileMock.Object);

            var declaration = DeclarationHelper.SetupDeclaration<IDeclaration<SyntaxNode>>("nameSpace", "name");

            fileMock.SetupGet(f => f.Declarations).Returns(new IDeclaration<SyntaxNode>[] { declaration });

            factoryMock.Setup(f => f.CreateFile(It.IsAny<string>(), It.IsAny<IGlobalUsingDirectives>()))
                .Returns(fileloaderMock.Object);

            workspace.RegisterFile("Test", null);

            workspace.DeepLoad();

            Mock.Get(declaration).Verify(declaration => declaration.DeepLoad(It.IsAny<IDeclarationResolver>()));
        }
    }
}