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
using SoloX.GeneratorTools.Core.Utils;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Structs;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Metadata
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

            LoadingTest.AssertGenericTypeLoaded(structDeclaration, type, baseType, false);
        }

        private IStructDeclaration LoadStructDeclaration(Type type)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var assemblyPath = type.Assembly.Location;

            var assemblyLoader = new CSharpMetadataAssembly(
                Mock.Of<IGeneratorLogger<CSharpMetadataAssembly>>(),
                DeclarationHelper.CreateMetadataDeclarationFactory(this.testOutputHelper),
                assemblyPath);

            assemblyLoader.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(assemblyLoader.Declarations.Where(x => x.Name == className));

            var structDeclaration = Assert.IsAssignableFrom<IStructDeclaration>(declaration);
            return structDeclaration;
        }
    }
}
