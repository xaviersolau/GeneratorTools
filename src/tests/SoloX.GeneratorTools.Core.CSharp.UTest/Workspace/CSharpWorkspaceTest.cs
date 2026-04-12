// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.IO;
using Microsoft.CodeAnalysis;
using NSubstitute;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers.XUnit;
using Xunit;

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

            var projectLoader1 = Substitute.For<ICSharpWorkspaceItemLoader<ICSharpProject>>();
            var projectLoader2 = Substitute.For<ICSharpWorkspaceItemLoader<ICSharpProject>>();

            var factoryMock = Substitute.For<ICSharpWorkspaceItemFactory>();
            factoryMock.CreateProject(Path.GetFullPath(projectName1))
                .Returns(projectLoader1);
            factoryMock.CreateProject(Path.GetFullPath(projectName2))
                .Returns(projectLoader2);

            projectLoader1.WorkspaceItem.Returns(Substitute.For<ICSharpProject>());
            projectLoader2.WorkspaceItem.Returns(Substitute.For<ICSharpProject>());

            var workspace = new CSharpWorkspace(
                LoggerHelper.CreateGeneratorLogger<CSharpWorkspace>(this.testOutputHelper),
                factoryMock);

            var project1 = workspace.RegisterProject(projectName1);
            var project2 = workspace.RegisterProject(projectName1);

            Assert.Same(project1, project2);

            var project3 = workspace.RegisterProject(projectName2);

            Assert.NotSame(project1, project3);

            factoryMock.Received().CreateProject(Path.Combine(workingDir, projectName1));
            factoryMock.Received().CreateProject(Path.Combine(workingDir, projectName2));

            projectLoader1.Received().Load(workspace);
            projectLoader2.Received().Load(workspace);
        }

        [Fact]
        public void DeepLoadTest()
        {
            var factoryMock = Substitute.For<ICSharpWorkspaceItemFactory>();

            var workspace = new CSharpWorkspace(
                LoggerHelper.CreateGeneratorLogger<CSharpWorkspace>(this.testOutputHelper),
                factoryMock);

            var fileloaderMock = Substitute.For<ICSharpWorkspaceItemLoader<ICSharpFile>>();
            var fileMock = Substitute.For<ICSharpFile>();

            fileloaderMock.WorkspaceItem.Returns(fileMock);

            var declaration = DeclarationHelper.SetupDeclaration<IDeclaration<SyntaxNode>>("nameSpace", "name");

            fileMock.Declarations.Returns(new IDeclaration<SyntaxNode>[] { declaration });
            factoryMock.CreateFile(Arg.Any<string>(), Arg.Any<IGlobalUsingDirectives>())
                .Returns(fileloaderMock);

            workspace.RegisterFile("Test", null);

            workspace.DeepLoad();

            declaration.Received().DeepLoad(Arg.Any<IDeclarationResolver>());
        }
    }
}