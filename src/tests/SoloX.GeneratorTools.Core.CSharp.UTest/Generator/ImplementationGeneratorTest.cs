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
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Patterns.Simple.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Patterns.Simple.Itf;
using SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Samples.Simple;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Generator
{
    public class ImplementationGeneratorTest
    {
        private const string SimpleSampleNS = "SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Samples.Simple";
        private const string SimplePatternItfNS = "SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Patterns.Simple.Itf";
        private const string SimplePatternImplNS = "SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Patterns.Simple.Impl";

        [Fact]
        public void GenerateSimpleTest()
        {
            var itfSyntaxTree = CSharpFileReader.Parse(@"Generator/Samples/Simple/ISimpleSample.cs")
                .GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>().First();
            var patternItfSyntaxTree = CSharpFileReader.Parse(@"Generator/Patterns/Simple/Itf/ISimplePattern.cs")
                .GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>().First();
            var patternImplSyntaxTree = CSharpFileReader.Parse(@"Generator/Patterns/Simple/Impl/SimplePattern.cs")
                .GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();

            var itfDeclaration = new InterfaceDeclaration(SimpleSampleNS, itfSyntaxTree, Array.Empty<string>(), string.Empty);
            var itfPatternDeclaration = new InterfaceDeclaration(SimplePatternItfNS, patternItfSyntaxTree, Array.Empty<string>(), string.Empty);
            var implPatternDeclaration = new ClassDeclaration(SimplePatternImplNS, patternImplSyntaxTree, new string[] { SimplePatternItfNS }, string.Empty);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            declarationResolverMock.Setup(dr => dr.Resolve(nameof(ISimplePattern), implPatternDeclaration)).Returns(itfPatternDeclaration);
            var declarationResolver = declarationResolverMock.Object;

            itfDeclaration.Load(declarationResolver);
            itfPatternDeclaration.Load(declarationResolver);
            implPatternDeclaration.Load(declarationResolver);

            var snapshotGenerator = new SnapshotGenerator();
            var implGenerator = new ImplementationGenerator(snapshotGenerator, "Target.NameSpace", itfPatternDeclaration, implPatternDeclaration);

            implGenerator.Generate(itfDeclaration);

            SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), nameof(this.GenerateSimpleTest), "Generator");
        }
    }
}
