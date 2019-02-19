// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace
{
    public class CSharpWorkspaceTest
    {
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

            var workspace = new CSharpWorkspace(factoryMock.Object, loaderMock.Object);

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
}