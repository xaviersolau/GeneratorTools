﻿// ----------------------------------------------------------------------
// <copyright file="ImplementationGeneratorTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.CSharp.ITest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Generator.Writer;
using SoloX.GeneratorTools.Core.Generator.Writer.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator
{
    public class ImplementationGeneratorTest
    {
        [Fact]
        public void GenerateSimpleTest()
        {
            var patternInterfaceFile = @"Generator/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Patterns/Impl/SimplePattern.cs";
            var declarationInterfaceFile = @"Generator/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";
            var implName = "SimpleSample";

            GenerateAndAssertSnapshot(
                patternInterfaceFile,
                patternImplementationFile,
                declarationInterfaceFile,
                targetNameSpace,
                implName,
                nameof(this.GenerateSimpleTest));
        }

        [Theory]
        [InlineData("Condition")]
        public void GenerateExpressionTest(string expression)
        {
            GenerateSimpleSample(expression, "Expression");
        }

        [Theory]
        [InlineData("If")]
        [InlineData("PackedIf")]
        [InlineData("ForEach")]
        [InlineData("For")]
        [InlineData("PackedForEach")]
        [InlineData("Throw")]
        public void GenerateStatementTest(string statement)
        {
            GenerateSimpleSample(statement, "Statement");
        }

        private static void GenerateSimpleSample(string name, string group)
        {
            var code = $"{name}{group}";
            var patternInterfaceFile = @"Generator/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = $@"Generator/Patterns/Impl/{group}/{code}Pattern.cs";
            var declarationInterfaceFile = @"Generator/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";
            var implName = $"{code}Sample";

            GenerateAndAssertSnapshot(
                patternInterfaceFile,
                patternImplementationFile,
                declarationInterfaceFile,
                targetNameSpace,
                implName,
                $"Generate{code}Test");
        }

        private static void GenerateAndAssertSnapshot(
            string patternInterfaceFile,
            string patternImplementationFile,
            string declarationInterfaceFile,
            string targetNameSpace,
            string implName,
            string snapshotName)
        {
            LoadWorkSpace(
                patternInterfaceFile,
                patternImplementationFile,
                declarationInterfaceFile,
                out var itfDeclaration,
                out var itfPatternDeclaration,
                out var implPatternDeclaration);

            var locator = new RelativeLocator(string.Empty, targetNameSpace);

            var snapshotGenerator = new SnapshotGenerator();

            var implGenerator = new ImplementationGenerator(
                snapshotGenerator, locator, itfPatternDeclaration, implPatternDeclaration);

            var writerSelector = SetupWriterSolector(itfPatternDeclaration, implPatternDeclaration, itfDeclaration, implName);

            implGenerator.Generate(writerSelector, itfDeclaration, implName);

            SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), snapshotName, "Generator");
        }

        private static IWriterSelector SetupWriterSolector(
            IInterfaceDeclaration itfPatternDeclaration,
            IClassDeclaration implPatternDeclaration,
            IInterfaceDeclaration itfDeclaration,
            string implName)
        {
            var propertyWriter = new PropertyWriter(
                itfPatternDeclaration.Properties.Single(),
                itfDeclaration.Properties.ToArray());

            var itfNameWriter = new StringReplaceWriter(itfPatternDeclaration.Name, itfDeclaration.Name);
            var implNameWriter = new StringReplaceWriter(implPatternDeclaration.Name, implName);

            return new WriterSelector(propertyWriter, itfNameWriter, implNameWriter);
        }

        private static IDeclarationResolver LoadWorkSpace(
            string patternInterfaceFile,
            string patternImplementationFile,
            string declarationInterfaceFile,
            out IInterfaceDeclaration itfDeclaration,
            out IInterfaceDeclaration itfPatternDeclaration,
            out IClassDeclaration implPatternDeclaration)
        {
            var ws = new CSharpWorkspace(
                Mock.Of<ILogger<CSharpWorkspace>>(),
                new CSharpFactory(Mock.Of<ILoggerFactory>()),
                new CSharpLoader());
            itfDeclaration = ws.RegisterFile(declarationInterfaceFile)
                .Declarations.First() as IInterfaceDeclaration;
            itfPatternDeclaration = ws.RegisterFile(patternInterfaceFile)
                .Declarations.First() as IInterfaceDeclaration;
            implPatternDeclaration = ws.RegisterFile(patternImplementationFile)
                .Declarations.First() as IClassDeclaration;

            return ws.DeepLoad();
        }
    }
}
