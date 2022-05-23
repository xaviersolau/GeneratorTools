// ----------------------------------------------------------------------
// <copyright file="CSharpAssemblyTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers;
using Xunit;
using Xunit.Abstractions;

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
                DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper),
                assembly);

            csAssembly.Load(Mock.Of<ICSharpWorkspace>());

            Assert.Same(assembly, csAssembly.Assembly);
            var decl = Assert.Single(csAssembly.Declarations.Where(d => d.Name == nameof(IBasicInterface)));

            var typeItfDecl = Assert.IsType<InterfaceDeclaration>(decl);

            Assert.Same(typeof(IBasicInterface), typeItfDecl.GetData<Type>());
        }
    }
}
