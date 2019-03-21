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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Simple.Impl;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Simple.Itf;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Samples.Simple;
using SoloX.GeneratorTools.Core.CSharp.ITest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
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

            var snapshotGenerator = new SnapshotGenerator();
            var implGenerator = new ImplementationGenerator(snapshotGenerator, "Target.NameSpace", itfPatternDeclaration, implPatternDeclaration);

            implGenerator.Generate(itfDeclaration);

            SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), nameof(this.GenerateSimpleTest), "Generator");
        }
    }
}
