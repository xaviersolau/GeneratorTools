// ----------------------------------------------------------------------
// <copyright file="CSharpAssemblyTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using NSubstitute;
using Shouldly;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers.XUnit;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace
{
    public class CSharpAssemblyTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public CSharpAssemblyTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void LoadCSharpAssemblyTest()
        {
            var assembly = typeof(CSharpAssemblyTest).Assembly;
            var csAssembly = new CSharpAssembly(
                LoggerHelper.CreateGeneratorLogger<CSharpAssembly>(this.testOutputHelper),
                DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper),
                assembly);

            csAssembly.Load(Substitute.For<ICSharpWorkspace>());

            Assert.Same(assembly, csAssembly.Assembly);
            var decl = csAssembly.Declarations.Where(d => d.Name == nameof(IBasicInterface)).ShouldHaveSingleItem();

            var typeItfDecl = Assert.IsType<InterfaceDeclaration>(decl);

            Assert.Same(typeof(IBasicInterface), typeItfDecl.GetData<Type>());
        }
    }
}
