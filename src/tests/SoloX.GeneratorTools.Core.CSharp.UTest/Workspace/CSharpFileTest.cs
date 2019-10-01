// ----------------------------------------------------------------------
// <copyright file="CSharpFileTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace
{
    public class CSharpFileTest
    {
        private ITestOutputHelper testOutputHelper;

        public CSharpFileTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void BasicCSharpFileTest()
        {
            var file = "./Resources/Workspace/BasicClass.cs";

            var csFile = new CSharpFile(file, DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));

            Assert.Equal(Path.GetFileName(file), csFile.FileName);
            Assert.Equal(Path.GetDirectoryName(file), csFile.FilePath);
        }

        [Theory]
        [InlineData("./Resources/Workspace/BasicClass.cs", nameof(BasicClass), typeof(ClassDeclaration))]
        [InlineData("./Resources/Workspace/BasicEnum.cs", nameof(BasicEnum), typeof(EnumDeclaration))]
        [InlineData("./Resources/Workspace/BasicInterface.cs", nameof(BasicInterface), typeof(InterfaceDeclaration))]
        [InlineData("./Resources/Workspace/BasicStruct.cs", nameof(BasicStruct), typeof(StructDeclaration))]
        public void LoadCSharpFileTest(string declarationFile, string name, Type expectedDeclarationType)
        {
            var csFile = new CSharpFile(
                declarationFile,
                DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));

            csFile.Load();

            Assert.Single(csFile.Declarations);
            var decl = csFile.Declarations.Single();

            Assert.Equal(expectedDeclarationType, decl.GetType());
            Assert.Equal(name, decl.Name);
        }

        [Fact]
        public void LoadCSharpFileNameSpaceDeclarationTest()
        {
            var file = "./Resources/Workspace/MultiNameSapces.cs";

            var csFile = new CSharpFile(file, DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));

            csFile.Load();

            Assert.Equal(3, csFile.Declarations.Count);

            Assert.Single(csFile.Declarations.Where(d => string.IsNullOrEmpty(d.DeclarationNameSpace)));
            Assert.Single(csFile.Declarations.Where(d => d.DeclarationNameSpace == "SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace"));
            Assert.Single(csFile.Declarations.Where(d => d.DeclarationNameSpace == "SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace.Test"));
        }
    }
}