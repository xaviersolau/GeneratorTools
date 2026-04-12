// ----------------------------------------------------------------------
// <copyright file="AutomatedGeneratorTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using SoloX.CodeQuality.Test.Helpers.Snapshot;
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
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;
using System.ComponentModel;
using System.Threading.Tasks;

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
        public async Task AutomatedSimpleTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimplePattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimplePattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleTest),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleMethod1Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod1Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimpleMethod1Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod1Test),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleMethodWithDateTime1Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod1Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodWithDateTimeSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimpleMethod1Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethodWithDateTime1Test),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleMethod2Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod2Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimpleMethod2Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod2Test),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleMethod3Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod3Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimpleMethod3Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod3Test),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleMethod4Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod4Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimpleMethod4Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod4Test),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleMethod5Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod5Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimpleMethod5Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod5Test),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleMethod6Test()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimpleMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimpleMethod6Pattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimpleMethod6Pattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleMethod6Test),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedAsyncMethodTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/IAsyncMethodPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/AsyncMethodPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/IAsyncMethodSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(AsyncMethodPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedAsyncMethodTest),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleWithCallTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimplePatternWithCall.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimplePatternWithCall),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleWithCallTest),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleWithConstructorTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimplePatternWithConstructor.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimplePatternWithConstructor),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleWithConstructorTest),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedSimpleWithCreateTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/SimplePatternWithCreate.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(SimplePatternWithCreate),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedSimpleWithCreateTest),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedRepeatStatementInProperty()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/RepeatStatementInPropertyPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(RepeatStatementInPropertyPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedRepeatStatementInProperty),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedRepeatStatementInConstructor()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/RepeatStatementInConstructorPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(RepeatStatementInConstructorPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedRepeatStatementInConstructor),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedConstTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/IConstPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/ConstPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/IConstSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(ConstPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedConstTest),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedAttributeSelectorTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/IAttributeSelectorPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/AttributeSelectorPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/IAttributeSelectorSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(AttributeSelectorPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedAttributeSelectorTest),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedBothRepeatAndRepeatStatementTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/IRepeatPattern.cs";
            var patternImplementationFile = @"Generator/Automated/Patterns/Impl/RepeatPattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/IRepeatSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
                typeof(RepeatPattern),
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                nameof(this.AutomatedBothRepeatAndRepeatStatementTest),
                declarationInterfaceFile);
        }

        [Fact]
        public async Task AutomatedIMethodPatternTest()
        {
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";

            var patternImplementationFile = @"Generator/Automated/Patterns/Itf/IMethodPattern.cs";

            var declarationInterfaceFile1 = @"Generator/Automated/Samples/ISimpleSample.cs";
            var declarationInterfaceFile2 = @"Generator/Automated/Samples/IOtherSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            await GenerateAndAssertSnapshot(
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
        public async Task GenerateMethodTest(string method)
        {
            await GenerateSimpleSample(method, "Method");
        }

        [Theory]
        [InlineData("If")]
        [InlineData("Try")]
        [InlineData("Await")]
        [InlineData("PackedIf")]
        [InlineData("ForEach")]
        [InlineData("For")]
        [InlineData("PackedForEach")]
        [InlineData("Throw")]
        [InlineData("Lambda")]
        [InlineData("Lambda2")]
        public async Task GenerateStatementTest(string statement)
        {
            await GenerateSimpleSample(statement, "Statement");
        }

        private Task GenerateSimpleSample(string name, string group)
        {
            var code = $"{name}{group}";
            var patternInterfaceFile = @"Generator/Automated/Patterns/Itf/ISimplePattern.cs";
            var patternImplementationFile = $@"Generator/Automated/Patterns/Impl/{group}/{code}SimplePattern.cs";
            var declarationInterfaceFile = @"Generator/Automated/Samples/ISimpleSample.cs";
            var targetNameSpace = "SoloX.GeneratorTools.Core.CSharp.ITest";

            var patternImplementationTypeName = $"SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.{group}.{code}SimplePattern";
            var patternImplementationType = Type.GetType(patternImplementationTypeName);

            return GenerateAndAssertSnapshot(
                patternImplementationType,
                patternInterfaceFile,
                patternImplementationFile,
                targetNameSpace,
                $"Automated{code}Test",
                declarationInterfaceFile);
        }

        private async Task GenerateAndAssertSnapshot(
            Type patternType,
            string patternInterfaceFile,
            string patternImplementationFile,
            string targetNameSpace,
            string snapshotName,
            params string[] declarationInterfaceFiles)
        {
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
                snapshotGenerator, locator, resolver, patternType, Substitute.For<IGeneratorLogger>(), selectorResolver: selectorResolver);

            implGenerator.Generate(files);

            var snapshotTest = SnapshotTestBuilder
                .Create()
                .WithThisFilePathLocation()
                .WithTextStrategy()
                .Build();

            await snapshotTest.CompareSnapshotAsync(snapshotName, snapshotGenerator.GetAllGenerated(), forceReplaceSnapshot: false).ConfigureAwait(false);
        }

        private static DefaultSelectorResolver CreateSelectorResolver()
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
                Substitute.For<IGeneratorLogger<CSharpWorkspace>>(),
                new CSharpWorkspaceItemFactory(
                    Substitute.For<IGeneratorLoggerFactory>(),
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
