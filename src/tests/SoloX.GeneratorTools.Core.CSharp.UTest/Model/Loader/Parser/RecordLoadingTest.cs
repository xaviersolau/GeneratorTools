// ----------------------------------------------------------------------
// <copyright file="RecordLoadingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Records;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Parser
{
    public class RecordLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly LoadingTest loadingTest;

        public RecordLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.loadingTest = new LoadingTest(testOutputHelper);
        }

        [Theory]
        [InlineData(typeof(SimpleRecord), null)]
        [InlineData(typeof(SimpleRecordWithBase), typeof(SimpleRecord))]
        public void ItShouldLoadRecordType(Type type, Type baseType)
        {
            var recordDeclaration = LoadRecordDeclaration(type);

            LoadingTest.AssertGenericTypeLoaded(recordDeclaration, type, baseType, true);
        }

        [Theory]
        [InlineData(typeof(ConstructorRecord))]
        public void ItShouldLoadRecordWithPrimaryConstructorType(Type type)
        {
            var recordDeclaration = LoadRecordDeclaration(type);

            this.loadingTest.AssertRecordPropertyListLoaded(recordDeclaration, type);
        }

        private IRecordDeclaration LoadRecordDeclaration(Type type)
        {
            var recordName = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var location = recordName.ToBasicRecordsPath();
            var csFile = new CSharpFile(
                location,
                DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                Mock.Of<IGlobalUsingDirectives>());

            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            Assert.Equal(location, declaration.Location);

            var recordDeclaration = Assert.IsAssignableFrom<IRecordDeclaration>(declaration);
            return recordDeclaration;
        }
    }
}
