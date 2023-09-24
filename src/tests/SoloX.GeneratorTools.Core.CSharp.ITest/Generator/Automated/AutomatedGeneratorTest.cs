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
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using System.ComponentModel;

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
                targetNameSpace,
                nameof(this.AutomatedSimpleTest),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedSimpleMethod1Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod1Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(SimpleMethod1Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod1Test),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedSimpleMethod2Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod2Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(SimpleMethod2Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod2Test),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedSimpleMethod3Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod3Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(SimpleMethod3Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod3Test),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedSimpleMethod4Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod4Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(SimpleMethod4Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod4Test),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedSimpleMethod5Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod5Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(SimpleMethod5Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod5Test),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedSimpleWithCallTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimplePatternWithCall.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(SimplePatternWithCall),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleWithCallTest),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedSimpleWithCreateTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimplePatternWithCreate.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(SimplePatternWithCreate),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleWithCreateTest),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedRepeatStatementInProperty()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/RepeatStatementInPropertyPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(RepeatStatementInPropertyPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedRepeatStatementInProperty),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedRepeatStatementInConstructor()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/RepeatStatementInConstructorPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(RepeatStatementInConstructorPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedRepeatStatementInConstructor),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedConstTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/IConstPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/ConstPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/IConstSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(ConstPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedConstTest),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedAttributeSelectorTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/IAttributeSelectorPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/AttributeSelectorPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/IAttributeSelectorSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(AttributeSelectorPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedAttributeSelectorTest),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedBothRepeatAndRepeatStatementTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/IRepeatPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/RepeatPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/IRepeatSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(RepeatPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedBothRepeatAndRepeatStatementTest),
                declarationInterfaceFile);
        }

        [Fact]
        public void AutomatedIMethodPatternTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";

            var patternImplementationFile = @"Generator/Automated/Patterns/Itf/IMethodPattern.cs";

            var declarationInterfaceFile1 = @"Generator/Automated/Samples/ISimpleSample.cs";
            var declarationInterfaceFile2 = @"Generator/Automated/Samples/IOtherSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            GenerateAndAssertSnapshot(
                typeof(IMethodPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedIMethodPatternTest),
                declarationInterfaceFile1,
                declarationInterfaceFile2);
        }

        [Theory]
        [InlineData("Simple")]
        [InlineData("TypeNameReplace")]
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

        private void GenerateSimpleSample(string name, string group)
        {
            var code = $"{name}{group}";
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = $@"Generator/Automated/Patterns/Impl/{group}/{code}SimplePattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            var patternImplementationTypeName = $"SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.{group}.{code}SimplePattern";
            var patternImplementationType = Type.GetType(patternImplementationTypeName);

            GenerateAndAssertSnapshot(
                patternImplementationType,
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                $"Automated{code}Test",
                declarationInterfaceFile);
        }

        private void GenerateAndAssertSnapshot(
            Type patternType,
            string patternInterfaceFile,
            string patternImplementationFile,
            string targetNameSpace,
            string snapshotName,
            params string[] declarationInterfaceFiles)
        {
            SnapshotHelper.IsOverwriteEnable = true;

            var resolver = LoadWorkSpace(
                patternInterfaceFile,
                patternImplementationFile,
                out var itfPatternDeclaration,
                out var implPatternDeclaration,
                out var itfDeclarations,
                out var files,
                declarationInterfaceFiles);

            var locator = new RelativeLocator(string.Empty, targetNameSpace);

            var snapshotGenerator = new SnapshotWriter();

            var selectorResolver = CreateSelectorResolver();

            var implGenerator = new AutomatedGenerator(
                snapshotGenerator, locator, resolver, patternType, selectorResolver, Mock.Of<IGeneratorLogger>());

            implGenerator.Generate(files);

            var location = SnapshotHelper.GetLocationFromCallingCodeProjectRoot("Generator/Automated");

            SnapshotHelper.AssertSnapshot(snapshotGenerator.GetAllGenerated(), snapshotName, location);
        }

        private static ISelectorResolver CreateSelectorResolver()
        {
            return new DefaultSelectorResolver(typeof(DescriptionAttribute));
        }

        private IDeclarationResolver LoadWorkSpace(
            string patternInterfaceFile,
            string patternImplementationFile,
            out IInterfaceDeclaration itfPatternDeclaration,
            out IClassDeclaration implPatternDeclaration,
            out IEnumerable<IInterfaceDeclaration> itfDeclarations,
            out IEnumerable<ICSharpFile> files,
            params string[] declarationInterfaceFiles)
        {
            var ws = new CSharpWorkspace(
                Mock.Of<IGeneratorLogger<CSharpWorkspace>>(),
                new CSharpWorkspaceItemFactory(
                    Mock.Of<IGeneratorLoggerFactory>(),
                    DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                    DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper),
                    DeclarationHelper.CreateMetadataDeclarationFactory(this.testOutputHelper)));

            var itfDeclarationsList = new List<IInterfaceDeclaration>();
            itfDeclarations = itfDeclarationsList;

            foreach (var declarationInterfaceFile in declarationInterfaceFiles)
            {
                var itfDeclaration = ws.RegisterFile(declarationInterfaceFile, null)
                    .Declarations.First() as IInterfaceDeclaration;

                itfDeclarationsList.Add(itfDeclaration);
            }

            itfPatternDeclaration = ws.RegisterFile(patternInterfaceFile, null)
                .Declarations.First() as IInterfaceDeclaration;
            implPatternDeclaration = ws.RegisterFile(patternImplementationFile, null)
                .Declarations.First() as IClassDeclaration;

            files = ws.Files;

            return ws.DeepLoad();
        }
    }
}
