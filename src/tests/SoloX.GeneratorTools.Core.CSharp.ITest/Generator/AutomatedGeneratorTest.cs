// ----------------------------------------------------------------------
// <copyright file="AutomatedGeneratorTest.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers.Snapshot;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator
{
    public class AutomatedGeneratorTest
    {
        private ITestOutputHelper testOutputHelper;

        public AutomatedGeneratorTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void AutomatedSimpleTest()
        {
            var patternInterfaceFile = @"Generator/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Patterns/Impl/SimplePattern.cs";
            var declarationInterfaceFile = @"Generator/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            this.GenerateAndAssertSnapshot(
                typeof(SimplePattern),
                patternInterfaceFile,
                patternImplementationFile,
                declarationInterfaceFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleTest));
        }

        private void GenerateAndAssertSnapshot(
            Type patternType,
            string patternInterfaceFile,
            string patternImplementationFile,
            string declarationInterfaceFile,
            string targetNameSpace,
            string snapshotName)
        {
            var resolver = this.LoadWorkSpace(
                patternInterfaceFile,
                patternImplementationFile,
                declarationInterfaceFile,
                out var itfDeclaration,
                out var itfPatternDeclaration,
                out var implPatternDeclaration,
                out var files);

            var locator = new RelativeLocator(string.Empty, targetNameSpace);

            var snapshotGenerator = new SnapshotWriter();

            var implGenerator = new AutomatedGenerator(
                snapshotGenerator, locator, resolver, patternType);

            implGenerator.Generate(files);

            string location = SnapshotHelper.GetLocationFromCallingProjectRoot("Generator");

            SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), snapshotName, location);
        }

        private IDeclarationResolver LoadWorkSpace(
            string patternInterfaceFile,
            string patternImplementationFile,
            string declarationInterfaceFile,
            out IInterfaceDeclaration itfDeclaration,
            out IInterfaceDeclaration itfPatternDeclaration,
            out IClassDeclaration implPatternDeclaration,
            out IEnumerable<ICSharpFile> files)
        {
            using (var ws = new CSharpWorkspace(
                Mock.Of<ILogger<CSharpWorkspace>>(),
                new CSharpFactory(
                    Mock.Of<ILoggerFactory>(),
                    DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper)),
                new CSharpLoader()))
            {
                itfDeclaration = ws.RegisterFile(declarationInterfaceFile)
                    .Declarations.First() as IInterfaceDeclaration;
                itfPatternDeclaration = ws.RegisterFile(patternInterfaceFile)
                    .Declarations.First() as IInterfaceDeclaration;
                implPatternDeclaration = ws.RegisterFile(patternImplementationFile)
                    .Declarations.First() as IClassDeclaration;

                files = ws.Files;

                return ws.DeepLoad();
            }
        }
    }
}
