// ----------------------------------------------------------------------
// <copyright file="RecordStructLoadingTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using System;
using Xunit;
using SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common;
using Microsoft.CodeAnalysis;
using NSubstitute;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Utils;
using System.Linq;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.RecordStructs;
using Shouldly;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Metadata
{
    public class RecordStructLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly LoadingTest loadingTest;

        public RecordStructLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.loadingTest = new LoadingTest(testOutputHelper);
        }

        [Theory]
        [InlineData(typeof(SimpleRecordStruct), null)]
        public void ItShouldLoadRecordStructType(Type type, Type? baseType)
        {
            var recordDeclaration = LoadRecordStructDeclaration(type);

            this.loadingTest.AssertGenericTypeLoaded(recordDeclaration, type, baseType, true);
        }

        private IRecordStructDeclaration LoadRecordStructDeclaration(Type type)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var assemblyPath = type.Assembly.Location;

            var assemblyLoader = new CSharpMetadataAssembly(
                Substitute.For<IGeneratorLogger<CSharpMetadataAssembly>>(),
                DeclarationHelper.CreateMetadataDeclarationFactory(this.testOutputHelper),
                assemblyPath);

            assemblyLoader.Load(Substitute.For<ICSharpWorkspace>());

            var declaration = assemblyLoader.Declarations.Where(x => x.Name == className).ShouldHaveSingleItem();

            var recordDeclaration = Assert.IsAssignableFrom<IRecordStructDeclaration>(declaration);
            return recordDeclaration;
        }
    }
}
