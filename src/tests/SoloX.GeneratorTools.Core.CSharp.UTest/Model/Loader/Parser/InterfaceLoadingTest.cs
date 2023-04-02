// ----------------------------------------------------------------------
// <copyright file="InterfaceLoadingTest.cs" company="Xavier Solau">
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
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Interfaces;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Parser
{
    public class InterfaceLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly LoadingTest loadingTest;

        public InterfaceLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.loadingTest = new LoadingTest(testOutputHelper);
        }

        [Theory]
        [InlineData(typeof(SimpleInterface), null)]
        [InlineData(typeof(SimpleInterfaceWithBase), typeof(SimpleInterface))]
        [InlineData(typeof(SimpleInterfaceWithGenericBase), typeof(GenericInterface<>))]
        [InlineData(typeof(GenericInterface<>), null)]
        public void ItShouldLoadInterfaceType(Type type, Type baseType)
        {
            var declaration = LoadInterfaceDeclaration(type);

            LoadingTest.AssertGenericTypeLoaded(declaration, type, baseType);
        }

        [Theory]
        [InlineData(typeof(PatternAttributedInterface), typeof(PatternAttribute))]
        [InlineData(typeof(RepeatAttributedInterface), typeof(RepeatAttribute))]
        public void ItShouldLoadInterfaceAttributes(Type type, Type attributeType)
        {
            var classDeclaration = LoadInterfaceDeclaration(type);

            this.loadingTest.AssertClassAttributeLoaded(classDeclaration, attributeType);
        }

        private IInterfaceDeclaration LoadInterfaceDeclaration(Type type)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var location = className.ToBasicInterfacesPath();
            var csFile = new CSharpFile(
                location,
                DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                Mock.Of<IGlobalUsingDirectives>());

            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            Assert.Equal(location, declaration.Location);

            var interfaceDeclaration = Assert.IsAssignableFrom<IInterfaceDeclaration>(declaration);
            return interfaceDeclaration;
        }
    }
}
