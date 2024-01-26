﻿// ----------------------------------------------------------------------
// <copyright file="EnumLoadingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Utils;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Enums;
using SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Reflection
{
    public class EnumLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly LoadingTest loadingTest;

        public EnumLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;

            this.loadingTest = new LoadingTest(testOutputHelper);
        }

        [Theory]
        [InlineData(typeof(SimpleEnum))]
        [InlineData(typeof(EnumWithBaseType))]
        public void ItShouldLoadEnumType(Type type)
        {
            var enumDeclaration = LoadEnumDeclaration(type);

            this.loadingTest.AssertEnumTypeLoaded(enumDeclaration, type);
        }

        private IEnumDeclaration LoadEnumDeclaration(Type type)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var assemblyLoader = new CSharpAssembly(
                Mock.Of<IGeneratorLogger<CSharpAssembly>>(),
                DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper),
                type.Assembly);

            assemblyLoader.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(assemblyLoader.Declarations.Where(x => x.Name == className));

            var enumDeclaration = Assert.IsAssignableFrom<IEnumDeclaration>(declaration);
            return enumDeclaration;
        }
    }
}
