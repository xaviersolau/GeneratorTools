// ----------------------------------------------------------------------
// <copyright file="TypeGenericDeclarationUseTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Use
{
    public class TypeGenericDeclarationUseTest
    {
        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(double))]
        [InlineData(typeof(string))]
        [InlineData(typeof(object))]
        public void PredefinedTypeDeclarationUseTest(Type type)
        {
            var resolverMock = new Mock<IDeclarationResolver>();

            var use = ReflectionGenericDeclarationLoader<SyntaxNode>.GetDeclarationUseFrom(type, resolverMock.Object);

            Assert.NotNull(use);
            Assert.NotNull(use.Declaration);

            var pdu = Assert.IsType<PredefinedDeclarationUse>(use);
            Assert.Equal(type.Name, pdu.Declaration.Name);
        }

        [Fact]
        public void UnknownTypeDeclarationUseTest()
        {
            var type = typeof(List<object>);
            var resolverMock = new Mock<IDeclarationResolver>();

            var use = ReflectionGenericDeclarationLoader<SyntaxNode>.GetDeclarationUseFrom(type, resolverMock.Object);

            Assert.NotNull(use);
            Assert.NotNull(use.Declaration);

            var udu = Assert.IsType<UnknownDeclarationUse>(use);
            Assert.Equal(type.Name, udu.Declaration.Name);
        }

        [Fact]
        public void ResolvedTypeDeclarationUseTest()
        {
            var type = typeof(TypeGenericDeclarationUseTest);
            var resolvedDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();
            var resolverMock = new Mock<IDeclarationResolver>();
            resolverMock.Setup(r => r.Resolve(type)).Returns(resolvedDeclaration);

            var use = ReflectionGenericDeclarationLoader<SyntaxNode>.GetDeclarationUseFrom(type, resolverMock.Object);

            Assert.NotNull(use);
            Assert.NotNull(use.Declaration);

            var gdu = Assert.IsType<GenericDeclarationUse>(use);
            Assert.Same(resolvedDeclaration, gdu.Declaration);
        }
    }
}
