// ----------------------------------------------------------------------
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
using System;
using Xunit;
using Xunit.Abstractions;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Enums;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Parser
{
    public class EnumLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public EnumLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(typeof(SimpleEnum))]
        public void ItShouldLoadEnumType(Type type)
        {
            var enumDeclaration = LoadEnumDeclaration(type);

            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            Assert.Equal(
                className,
                enumDeclaration.Name);

            Assert.NotNull(enumDeclaration.SyntaxNodeProvider);
            Assert.NotNull(enumDeclaration.SyntaxNodeProvider.SyntaxNode);
        }

        private IEnumDeclaration LoadEnumDeclaration(Type type)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var location = className.ToBasicEnumsPath();
            var csFile = new CSharpFile(
                location,
                DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                Mock.Of<IGlobalUsingDirectives>());

            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            Assert.Equal(location, declaration.Location);

            var enumDeclaration = Assert.IsAssignableFrom<IEnumDeclaration>(declaration);
            return enumDeclaration;
        }
    }
}
