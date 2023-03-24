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
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using System;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Parser
{
    public class InterfaceLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public InterfaceLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(typeof(SimpleInterface), null)]
        [InlineData(typeof(SimpleInterfaceWithBase), typeof(SimpleInterface))]
        public void ItShouldLoadInterfaceType(Type type, Type baseType)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var location = className.ToBasicPath();
            var csFile = new CSharpFile(
                location,
                DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                Mock.Of<IGlobalUsingDirectives>());

            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            Assert.Equal(location, declaration.Location);

            var classDeclaration = Assert.IsAssignableFrom<IInterfaceDeclaration>(declaration);

            LoadingTest.AssertGenericTypeLoaded(classDeclaration, type, baseType);
        }
    }
}
