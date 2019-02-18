// ----------------------------------------------------------------------
// <copyright file="CSharpProjectTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace
{
    public class CSharpProjectTest
    {
        [Fact]
        public void LoadCSharpProjectTest()
        {
            var projectFile = @"..\..\..\..\SoloX.GeneratorTools.Core.CSharp.Sample2\SoloX.GeneratorTools.Core.CSharp.Sample2.csproj";

            var workspaceMock = new Mock<ICSharpWorkspace>();

            workspaceMock
                .Setup(ws => ws.RegisterProject(It.IsAny<string>()))
                .Returns<string>(p => new CSharpProject(p));

            workspaceMock
                .Setup(ws => ws.RegisterFile(It.IsAny<string>()))
                .Returns<string>(p => new CSharpFile(p));

            var project = new CSharpProject(projectFile);
            project.Load(workspaceMock.Object);

            Assert.Single(project.ProjectReferences);

            var projectReference = project.ProjectReferences.Single();
            Assert.Equal("SoloX.GeneratorTools.Core.CSharp.Sample1.csproj", projectReference.ProjectFile);

            Assert.Single(project.Files);

            var csFile = project.Files.Single();
            Assert.Equal("Class1.cs", csFile.FileName);
        }
    }
}
