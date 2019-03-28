// ----------------------------------------------------------------------
// <copyright file="ImplementationGeneratorTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.ITest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Generator.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator
{
    public class ImplementationGeneratorTest
    {
        [Fact]
        public void GenerateSimpleTest()
        {
            var ws = new CSharpWorkspace(new CSharpFactory(), new CSharpLoader());
            var itfDeclaration = ws.RegisterFile(@"Generator/Samples/Simple/ISimpleSample.cs")
                .Declarations.First() as IInterfaceDeclaration;
            var itfPatternDeclaration = ws.RegisterFile(@"Generator/Patterns/Simple/Itf/ISimplePattern.cs")
                .Declarations.First() as IInterfaceDeclaration;
            var implPatternDeclaration = ws.RegisterFile(@"Generator/Patterns/Simple/Impl/SimplePattern.cs")
                .Declarations.First() as IClassDeclaration;

            var declarationResolver = ws.DeepLoad();
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";
            var locator = new RelativeLocator(string.Empty, targetNameSpace);

            var snapshotGenerator = new SnapshotGenerator();
            var implGenerator = new ImplementationGenerator(snapshotGenerator, locator, itfPatternDeclaration, implPatternDeclaration);

            implGenerator.Generate(itfDeclaration);

            SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), nameof(this.GenerateSimpleTest), "Generator");
        }
    }
}
