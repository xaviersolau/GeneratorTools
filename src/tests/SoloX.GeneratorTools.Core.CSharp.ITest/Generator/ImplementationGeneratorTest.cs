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
using SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.CSharp.ITest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Generator.Writer.Impl;
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

            var implName = "SimpleSample";

            var implGenerator = new ImplementationGenerator(snapshotGenerator, locator, itfPatternDeclaration, implPatternDeclaration);

            var propertyWriter = new PropertyWriter(
                itfPatternDeclaration.Properties.Single(),
                itfDeclaration.Properties.ToArray());

            var itfNameWriter = new StringReplaceWriter(itfPatternDeclaration.Name, itfDeclaration.Name);
            var implNameWriter = new StringReplaceWriter(implPatternDeclaration.Name, implName);

            var writerSelector = new WriterSelector(propertyWriter, itfNameWriter, implNameWriter);

            implGenerator.Generate(writerSelector, itfDeclaration, implName);

            SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), nameof(this.GenerateSimpleTest), "Generator");
        }
    }
}
