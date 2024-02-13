// ----------------------------------------------------------------------
// <copyright file="RecordStructLoadingTest.cs" company="Xavier Solau">
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
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.RecordStructs;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Parser
{
    public class RecordStructLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public RecordStructLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(typeof(SimpleRecordStruct), null)]
        public void ItShouldLoadRecordStructType(Type type, Type baseType)
        {
            var recordDeclaration = LoadRecordStructDeclaration(type);

            LoadingTest.AssertGenericTypeLoaded(recordDeclaration, type, baseType, true);
        }

        private IRecordStructDeclaration LoadRecordStructDeclaration(Type type)
        {
            var recordName = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var location = recordName.ToBasicRecordStructsPath();
            var csFile = new CSharpFile(
                location,
                DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                Mock.Of<IGlobalUsingDirectives>());

            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            Assert.Equal(location, declaration.Location);

            var recordDeclaration = Assert.IsAssignableFrom<IRecordStructDeclaration>(declaration);
            return recordDeclaration;
        }
    }
}
