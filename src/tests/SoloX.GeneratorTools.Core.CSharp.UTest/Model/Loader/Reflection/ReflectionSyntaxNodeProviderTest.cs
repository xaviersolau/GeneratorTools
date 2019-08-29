// ----------------------------------------------------------------------
// <copyright file="ReflectionSyntaxNodeProviderTest.cs" company="SoloX Software">
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

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Reflection
{
    public class ReflectionSyntaxNodeProviderTest
    {
        [Fact]
        public void SytaxNodeProviderSimpleClassTest()
        {
            var type = typeof(SimpleClass);
            var classDeclaration = LoadClassDeclarationFrom(type);

            Assert.NotNull(classDeclaration.SyntaxNodeProvider);
            Assert.NotNull(classDeclaration.SyntaxNodeProvider.SyntaxNode);
        }

        [Fact]
        public void SytaxNodeProviderClassWithPropertiesTest()
        {
            var type = typeof(ClassWithProperties);
            var classDeclaration = LoadClassDeclarationFrom(type);

            foreach (var property in classDeclaration.Properties)
            {
                Assert.NotNull(property.SyntaxNodeProvider);
                Assert.NotNull(property.SyntaxNodeProvider.SyntaxNode);
                Assert.Contains(property.Name, property.SyntaxNodeProvider.SyntaxNode.ToString(), StringComparison.InvariantCulture);
                Assert.NotNull(property.PropertyType.SyntaxNodeProvider);
                Assert.NotNull(property.PropertyType.SyntaxNodeProvider.SyntaxNode);
                Assert.Equal(property.PropertyType.Declaration.Name, property.PropertyType.SyntaxNodeProvider.SyntaxNode.ToString());
            }
        }

        [Fact]
        public void SytaxNodeProviderClassWithArrayPropertiesTest()
        {
            var type = typeof(ClassWithArrayProperties);
            var classDeclaration = LoadClassDeclarationFrom(type);

            foreach (var property in classDeclaration.Properties)
            {
                Assert.NotNull(property.PropertyType.ArraySpecification);
                Assert.NotNull(property.PropertyType.ArraySpecification.SyntaxNodeProvider);

                var code = property.PropertyType.ArraySpecification.SyntaxNodeProvider.SyntaxNode.ToString();

                Assert.Contains(
                    property.PropertyType.Declaration.Name,
                    code,
                    StringComparison.InvariantCulture);

                Assert.Contains(
                    "[]",
                    code,
                    StringComparison.InvariantCulture);
            }
        }

        private static ClassDeclaration LoadClassDeclarationFrom(Type type)
        {
            var declaration = DeclarationFactory.CreateClassDeclaration(type);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);
            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.Load(declarationResolverMock.Object);
            return classDeclaration;
        }
    }
}
