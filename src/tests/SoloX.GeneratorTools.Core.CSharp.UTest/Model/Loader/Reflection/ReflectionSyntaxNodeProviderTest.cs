// ----------------------------------------------------------------------
// <copyright file="ReflectionSyntaxNodeProviderTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Reflection
{
    public class ReflectionSyntaxNodeProviderTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ReflectionSyntaxNodeProviderTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void SytaxNodeProviderSimpleClassTest()
        {
            var type = typeof(SimpleClass);
            var classDeclaration = this.LoadClassDeclarationFrom(type);

            Assert.NotNull(classDeclaration.SyntaxNodeProvider);
            Assert.NotNull(classDeclaration.SyntaxNodeProvider.SyntaxNode);
        }

        [Fact]
        public void SytaxNodeProviderClassWithPropertiesTest()
        {
            var type = typeof(ClassWithProperties);
            var classDeclaration = this.LoadClassDeclarationFrom(type);

            foreach (var property in classDeclaration.Properties)
            {
                Assert.NotNull(property.SyntaxNodeProvider);
                Assert.NotNull(property.SyntaxNodeProvider.SyntaxNode);
                Assert.Contains(property.Name, property.SyntaxNodeProvider.SyntaxNode.ToString(), StringComparison.InvariantCulture);
                Assert.NotNull(property.PropertyType.SyntaxNodeProvider);
                Assert.NotNull(property.PropertyType.SyntaxNodeProvider.SyntaxNode);
                Assert.Equal(property.PropertyType.Declaration.FullName, property.PropertyType.SyntaxNodeProvider.SyntaxNode.ToString());
            }
        }

        [Fact]
        public void SytaxNodeProviderClassWithArrayPropertiesTest()
        {
            var type = typeof(ClassWithArrayProperties);
            var classDeclaration = this.LoadClassDeclarationFrom(type);

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

        [Theory]
        [InlineData(typeof(byte), "byte")]
        [InlineData(typeof(short), "short")]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(float), "float")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(decimal), "decimal")]
        [InlineData(typeof(char), "char")]
        public void PredefinedSyntaxNodeProviderTest(Type type, string code)
        {
            var provider = new ReflectionPredefinedSyntaxNodeProvider(type);

            var syntaxNode = provider.SyntaxNode;

            Assert.NotNull(syntaxNode);
            Assert.IsType<PredefinedTypeSyntax>(syntaxNode);
            Assert.Equal(code, syntaxNode.ToString());
        }

        private ClassDeclaration LoadClassDeclarationFrom(Type type)
        {
            var declaration = DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper)
                .CreateClassDeclaration(type);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);
            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.DeepLoad(declarationResolverMock.Object);
            return classDeclaration;
        }
    }
}
