// ----------------------------------------------------------------------
// <copyright file="TypeGenericDeclarationUseTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using NSubstitute;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Use
{
    public class TypeGenericDeclarationUseTest
    {
        [Theory]
        [InlineData(typeof(byte), "byte")]
        [InlineData(typeof(short), "short")]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(float), "float")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(decimal), "decimal")]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(char), "char")]
        public void PredefinedTypeDeclarationUseTest(Type type, string typeName)
        {
            var resolverMock = Substitute.For<IDeclarationResolver>();

            var use = ReflectionGenericDeclarationLoader<SyntaxNode>.GetDeclarationUseFrom(type, resolverMock, null);

            Assert.NotNull(use);
            Assert.NotNull(use.Declaration);

            var pdu = Assert.IsType<PredefinedDeclarationUse>(use);
            Assert.Equal(typeName, pdu.Declaration.Name);
        }

        [Fact]
        public void UnknownTypeDeclarationUseTest()
        {
            var type = typeof(List<object>);
            var resolverMock = Substitute.For<IDeclarationResolver>();

            resolverMock.Resolve(Arg.Any<Type>()).Returns((IGenericDeclaration<SyntaxNode>?)null);

            var use = ReflectionGenericDeclarationLoader<SyntaxNode>.GetDeclarationUseFrom(type, resolverMock, null);

            Assert.NotNull(use);
            Assert.NotNull(use.Declaration);

            var udu = Assert.IsType<UnknownDeclarationUse>(use);
            Assert.Equal(ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name), udu.Declaration.Name);
        }

        [Fact]
        public void ResolvedTypeDeclarationUseTest()
        {
            var type = typeof(TypeGenericDeclarationUseTest);
            var resolvedDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            var resolverMock = Substitute.For<IDeclarationResolver>();
            resolverMock.Resolve(type).Returns(resolvedDeclaration);

            var use = ReflectionGenericDeclarationLoader<SyntaxNode>.GetDeclarationUseFrom(type, resolverMock, null);

            Assert.NotNull(use);
            Assert.NotNull(use.Declaration);

            var gdu = Assert.IsType<GenericDeclarationUse>(use);
            Assert.Same(resolvedDeclaration, gdu.Declaration);
        }
    }
}
