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
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace
{
    public class CSharpFileTest
    {
        [Fact]
        public void BasicCSharpFileTest()
        {
            var file = "./Resources/Workspace/LoadClassTest.cs";

            var csFile = new CSharpFile(file);

            Assert.Equal(Path.GetFileName(file), csFile.FileName);
            Assert.Equal(Path.GetDirectoryName(file), csFile.FilePath);
        }

        [Theory]
        [InlineData("./Resources/Workspace/LoadClassTest.cs", typeof(ClassDeclaration))]
        [InlineData("./Resources/Workspace/LoadEnumTest.cs", typeof(EnumDeclaration))]
        [InlineData("./Resources/Workspace/LoadInterfaceTest.cs", typeof(InterfaceDeclaration))]
        [InlineData("./Resources/Workspace/LoadStructTest.cs", typeof(StructDeclaration))]
        public void LoadCSharpFileTest(string declarationFile, Type expectedDeclarationType)
        {
            var csFile = new CSharpFile(declarationFile);

            csFile.Load();

            Assert.Single(csFile.Declarations);

            Assert.Equal(expectedDeclarationType, csFile.Declarations.Single().GetType());
        }

        [Fact]
        public void LoadCSharpFileNameSpaceDeclarationTest()
        {
            var file = "./Resources/Workspace/LoadNameSapceTest.cs";

            var csFile = new CSharpFile(file);

            csFile.Load();

            Assert.Equal(3, csFile.Declarations.Count);

            Assert.Single(csFile.Declarations.Where(d => string.IsNullOrEmpty(d.DeclarationNameSpace)));
            Assert.Single(csFile.Declarations.Where(d => d.DeclarationNameSpace == "SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace"));
            Assert.Single(csFile.Declarations.Where(d => d.DeclarationNameSpace == "SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace.Test"));
        }
    }
}