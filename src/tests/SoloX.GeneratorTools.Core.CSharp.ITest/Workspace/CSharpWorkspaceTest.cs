// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Workspace
{
    public class CSharpWorkspaceTest
    {
        [Fact]
        public void ProjectLoadTest()
        {
            var projectFile = @"..\..\..\..\SoloX.GeneratorTools.Core.CSharp.Sample1\SoloX.GeneratorTools.Core.CSharp.Sample1.csproj";

            var ws = new CSharpWorkspace(new CSharpFactory(), new CSharpLoader());
            ws.RegisterProject(projectFile);

            Assert.Equal(1, ws.Projects.Count);

            Assert.NotEmpty(ws.Files);

            var resolver = ws.DeepLoad();

            var sample1Class1Decl = Assert.Single(resolver.Find("SoloX.GeneratorTools.Core.CSharp.Sample1.Sample1Class1"));

            Assert.NotNull(sample1Class1Decl.Name);
            Assert.IsType<ClassDeclaration>(sample1Class1Decl);
        }

        [Fact]
        public void ProjectLoadWithDependenciesTest()
        {
            var projectFile = @"..\..\..\..\SoloX.GeneratorTools.Core.CSharp.Sample2\SoloX.GeneratorTools.Core.CSharp.Sample2.csproj";

            var ws = new CSharpWorkspace(new CSharpFactory(), new CSharpLoader());
            ws.RegisterProject(projectFile);

            Assert.Equal(2, ws.Projects.Count);

            Assert.NotEmpty(ws.Files);

            var resolver = ws.DeepLoad();

            var sample2Class1Decl = Assert.Single(resolver.Find("SoloX.GeneratorTools.Core.CSharp.Sample2.Sample2Class1"));
            var sample1Class1Decl = Assert.Single(resolver.Find("SoloX.GeneratorTools.Core.CSharp.Sample1.Sample1Class1"));

            Assert.NotNull(sample2Class1Decl.Name);
            Assert.NotNull(sample1Class1Decl.Name);

            var sample2Class1 = Assert.IsType<ClassDeclaration>(sample2Class1Decl);
            var sample1Class1 = Assert.IsType<ClassDeclaration>(sample1Class1Decl);

            var extendedClassUse = Assert.Single(sample2Class1.Extends);
            Assert.Same(sample1Class1Decl, extendedClassUse.Declaration);

            var extendedBy = Assert.Single(sample1Class1.ExtendedBy);
            Assert.Same(sample2Class1Decl, extendedBy);
        }
    }
}
