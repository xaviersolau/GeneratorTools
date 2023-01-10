// ----------------------------------------------------------------------
// <copyright file="ImplementationGeneratorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Linq;
using Moq;
using SoloX.CodeQuality.Test.Helpers.XUnit;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Generator.Writer;
using SoloX.GeneratorTools.Core.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.Test.Helpers.Snapshot;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Implementation
{
    public class ImplementationGeneratorTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ImplementationGeneratorTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GenerateSimpleTest()
        {
            var patternInterfaceFile = @"Generator/Implementation/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Implementation/Patterns/Impl/SimplePattern.cs";
            var declarationInterfaceFile = @"Generator/Implementation/Samples/ISimpleSample.cs";
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
        [InlineData("Simple")]
        public void GenerateMethodTest(string method)
        {
            GenerateSimpleSample(method, "Method");
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

        private static IWriterSelector SetupWriterSelector(
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

        private void GenerateSimpleSample(string name, string group)
        {
            var code = $"{name}{group}";
            var patternInterfaceFile = @"Generator/Implementation/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = $@"Generator/Implementation/Patterns/Impl/{group}/{code}Pattern.cs";
            var declarationInterfaceFile = @"Generator/Implementation/Samples/ISimpleSample.cs";
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

        private void GenerateAndAssertSnapshot(
            string patternInterfaceFile,
            string patternImplementationFile,
            string declarationInterfaceFile,
            string targetNameSpace,
            string implName,
            string snapshotName)
        {
            //SnapshotHelper.IsOverwriteEnable = true;

            LoadWorkSpace(
                patternInterfaceFile,
                patternImplementationFile,
                declarationInterfaceFile,
                out var itfDeclaration,
                out var itfPatternDeclaration,
                out var implPatternDeclaration);

            var locator = new RelativeLocator(string.Empty, targetNameSpace);

            var snapshotGenerator = new SnapshotWriter();

            var implGenerator = new ImplementationGenerator(
                snapshotGenerator, locator, itfPatternDeclaration, implPatternDeclaration);

            var writerSelector = SetupWriterSelector(itfPatternDeclaration, implPatternDeclaration, itfDeclaration, implName);

            implGenerator.Generate(writerSelector, itfDeclaration, implName);

            var location = SnapshotHelper.GetLocationFromCallingCodeProjectRoot("Generator/Implementation");

            SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), snapshotName, location);
        }

        private IDeclarationResolver LoadWorkSpace(
            string patternInterfaceFile,
            string patternImplementationFile,
            string declarationInterfaceFile,
            out IInterfaceDeclaration itfDeclaration,
            out IInterfaceDeclaration itfPatternDeclaration,
            out IClassDeclaration implPatternDeclaration)
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

            return ws.DeepLoad();
        }
    }
}
