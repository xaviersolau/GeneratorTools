// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;
using SoloX.CodeQuality.Test.Helpers.Logger;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Workspace
{
    public class CSharpWorkspaceTest : IDisposable
    {
        private ITestOutputHelper testOutputHelper;
        private ILoggerFactory testLoggerFactory;

        public CSharpWorkspaceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.testLoggerFactory = new TestLoggerFactory(this.testOutputHelper);
        }

        [Fact]
        public void SimpleProjectLoadTest()
        {
            var projectFile = @"../../../../SoloX.GeneratorTools.Core.CSharp.Sample1/SoloX.GeneratorTools.Core.CSharp.Sample1.csproj";

            var resolver = this.LoadAndGetResolver(projectFile, 1);

            var sample1Class1Decl = Assert.Single(resolver.Find("SoloX.GeneratorTools.Core.CSharp.Sample1.Sample1Class1"));

            Assert.NotNull(sample1Class1Decl.Name);
            Assert.IsType<ClassDeclaration>(sample1Class1Decl);
        }

        [Fact]
        public void ProjectDependencyAssemblyLoadTest()
        {
            var projectFile = @"../../../../SoloX.GeneratorTools.Core.CSharp.Sample1/SoloX.GeneratorTools.Core.CSharp.Sample1.csproj";

            var resolver = this.LoadAndGetResolver(projectFile, 1);

            var jsonDecl = resolver.Find("Newtonsoft.Json.IArrayPool");

            Assert.NotNull(jsonDecl);
            Assert.NotEmpty(jsonDecl);
        }

        [Fact]
        public void ProjectLoadWithDependenciesTest()
        {
            var projectFile = @"../../../../SoloX.GeneratorTools.Core.CSharp.Sample2/SoloX.GeneratorTools.Core.CSharp.Sample2.csproj";

            var resolver = this.LoadAndGetResolver(projectFile, 2);

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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            this.testLoggerFactory.Dispose();
        }

        private IDeclarationResolver LoadAndGetResolver(string projectFile, int expectedProjectCount)
        {
            using (var ws = new CSharpWorkspace(
                new TestLogger<CSharpWorkspace>(this.testOutputHelper),
                new CSharpFactory(
                    this.testLoggerFactory,
                    DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper)),
                new CSharpLoader()))
            {
                ws.RegisterProject(projectFile);

                Assert.Equal(expectedProjectCount, ws.Projects.Count);

                Assert.NotEmpty(ws.Files);

                return ws.DeepLoad();
            }
        }
    }
}
