// ----------------------------------------------------------------------
// <copyright file="StructLoadingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using System;
using Xunit;
using Xunit.Abstractions;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Structs;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Parser
{
    public class StructLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public StructLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(typeof(SimpleStruct), null)]
        public void ItShouldLoadStructType(Type type, Type baseType)
        {
            var structDeclaration = LoadStructDeclaration(type);

            LoadingTest.AssertGenericTypeLoaded(structDeclaration, type, baseType);
        }

        private IStructDeclaration LoadStructDeclaration(Type type)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var location = className.ToBasicStructsPath();
            var csFile = new CSharpFile(
                location,
                DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                Mock.Of<IGlobalUsingDirectives>());

            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            Assert.Equal(location, declaration.Location);

            var structDeclaration = Assert.IsAssignableFrom<IStructDeclaration>(declaration);
            return structDeclaration;
        }
    }
}
