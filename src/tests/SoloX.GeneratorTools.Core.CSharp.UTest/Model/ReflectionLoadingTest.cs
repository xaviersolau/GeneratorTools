// ----------------------------------------------------------------------
// <copyright file="ReflectionLoadingTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model
{
    public class ReflectionLoadingTest
    {
        [Fact]
        public void BasicReflectionLoadingTest()
        {
            var type = typeof(SimpleClass);

            var declaration = DeclarationFactory.CreateClassDeclaration(type);

            Assert.NotNull(declaration);
            Assert.Equal(nameof(SimpleClass), declaration.Name);

            Assert.NotNull(declaration.SyntaxNodeProvider);
            Assert.NotNull(declaration.SyntaxNodeProvider.SyntaxNode);

            Assert.Null(declaration.GenericParameters);
            Assert.Null(declaration.Extends);
            Assert.Null(declaration.Members);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.Load(declarationResolverMock.Object);

            Assert.NotNull(declaration.GenericParameters);
            Assert.NotNull(declaration.Extends);
            Assert.NotNull(declaration.Members);
        }
    }
}
