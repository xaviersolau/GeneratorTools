// ----------------------------------------------------------------------
// <copyright file="AutomatedGeneratorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SoloX.CodeQuality.Test.Helpers.XUnit;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers.Snapshot;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated
{
    public class AutomatedGeneratorTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public AutomatedGeneratorTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void AutomatedSimpleTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimplePattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(SimplePattern),
                patternInterfaceFile,
                patternImplementationFile,
                declarationInterfaceFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleTest));
        }

        [Theory]
        [InlineData("Simple")]
        public void GenerateMethodTest(string method)
        {
            GenerateSimpleSample(method, "Method");
        }

        private void GenerateSimpleSample(string name, string group)
        {
            var code = $"{name}{group}";
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = $@"Generator/Automated/Patterns/Impl/{group}/{code}Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            var patternImplementationTypeName = $"SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.{group}.{code}Pattern";
            var patternImplementationType = Type.GetType(patternImplementationTypeName);

            GenerateAndAssertSnapshot(
                patternImplementationType,
                patternInterfaceFile,
                patternImplementationFile,
                declarationInterfaceFile,
                targetNameSpace,
                $"Automated{code}Test");
        }

        private void GenerateAndAssertSnapshot(
            Type patternType,
            string patternInterfaceFile,
            string patternImplementationFile,
            string declarationInterfaceFile,
            string targetNameSpace,
            string snapshotName)
        {
            //SnapshotHelper.IsOverwriteEnable = true;

            var resolver = LoadWorkSpace(
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
                snapshotGenerator, locator, resolver, patternType, null, Mock.Of<IGeneratorLogger>());

            implGenerator.Generate(files);

            var location = SnapshotHelper.GetLocationFromCallingCodeProjectRoot("Generator/Automated");

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
            var ws = new CSharpWorkspace(
                Mock.Of<IGeneratorLogger<CSharpWorkspace>>(),
                new CSharpWorkspaceItemFactory(
                    Mock.Of<IGeneratorLoggerFactory>(),
                    DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper)));

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
