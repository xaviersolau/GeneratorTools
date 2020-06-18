// ----------------------------------------------------------------------
// <copyright file="ReflectionLoadingTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Reflection
{
    public class ReflectionLoadingTest
    {
        private ITestOutputHelper testOutputHelper;

        public ReflectionLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(typeof(SimpleClass))]
        [InlineData(typeof(SimpleClassWithBase))]
        [InlineData(typeof(SimpleClassWithGenericBase))]
        [InlineData(typeof(GenericClass<>))]
        [InlineData(typeof(GenericClassWithBase<>))]
        [InlineData(typeof(GenericClassWithGenericBase<>))]
        public void BasicReflectionLoadingTest(Type type)
        {
            var declaration = DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper)
                .CreateClassDeclaration(type);

            Assert.NotNull(declaration);
            Assert.Equal(
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                declaration.Name);

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

            if (type.IsGenericTypeDefinition)
            {
                Assert.NotEmpty(declaration.GenericParameters);

                var typeParams = type.GetTypeInfo().GenericTypeParameters;
                Assert.Equal(typeParams.Length, declaration.GenericParameters.Count);

                Assert.Equal(typeParams[0].Name, declaration.GenericParameters.First().Name);
            }
            else
            {
                Assert.Empty(declaration.GenericParameters);
            }
        }

        [Theory]
        [InlineData(typeof(GenericClassWithBase<>), nameof(SimpleClass))]
        [InlineData(typeof(GenericClassWithGenericBase<>), nameof(GenericClass<object>))]
        public void LoadExtendsTest(Type type, string baseClassName)
        {
            var declaration = DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper)
                .CreateClassDeclaration(type);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.Load(declarationResolverMock.Object);

            Assert.NotEmpty(classDeclaration.Extends);

            var baseClassDefinition = classDeclaration.Extends.First().Declaration;

            Assert.NotNull(baseClassDefinition);
            Assert.Equal(baseClassName, baseClassDefinition.Name);
        }

        [Theory]
        [InlineData(typeof(ClassWithProperties), false)]
        [InlineData(typeof(ClassWithArrayProperties), true)]
        public void LoadMemberListTest(Type type, bool isArray)
        {
            var declarationFactory = DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper);
            var declaration = declarationFactory.CreateClassDeclaration(type);
            var simpleClassDeclaration = declarationFactory.CreateClassDeclaration(typeof(SimpleClass));

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            declarationResolverMock.Setup(r => r.Resolve(typeof(SimpleClass))).Returns(simpleClassDeclaration);
            classDeclaration.Load(declarationResolverMock.Object);

            Assert.NotEmpty(classDeclaration.Properties);
            Assert.Equal(2, classDeclaration.Properties.Count);

            var mClass = Assert.Single(declaration.Members.Where(m => m.Name == nameof(ClassWithProperties.PropertyClass)));
            var pClass = Assert.IsType<PropertyDeclaration>(mClass);

            Assert.IsType<GenericDeclarationUse>(pClass.PropertyType);
            Assert.Equal(nameof(SimpleClass), pClass.PropertyType.Declaration.Name);

            var mInt = Assert.Single(declaration.Members.Where(m => m.Name == nameof(ClassWithProperties.PropertyInt)));
            var pInt = Assert.IsType<PropertyDeclaration>(mInt);

            Assert.IsType<PredefinedDeclarationUse>(pInt.PropertyType);
            Assert.Equal("int", pInt.PropertyType.Declaration.Name);

            if (isArray)
            {
                Assert.NotNull(pClass.PropertyType.ArraySpecification);
                Assert.NotNull(pInt.PropertyType.ArraySpecification);
            }
            else
            {
                Assert.Null(pClass.PropertyType.ArraySpecification);
                Assert.Null(pInt.PropertyType.ArraySpecification);
            }
        }

        [Fact]
        public void LoadGetterSetterPropertyTest()
        {
            var type = typeof(ClassWithGetterSetterProperties);

            var declarationFactory = DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper);
            var declaration = declarationFactory.CreateClassDeclaration(type);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.Load(declarationResolverMock.Object);

            Assert.NotEmpty(classDeclaration.Properties);
            Assert.Equal(3, classDeclaration.Properties.Count);

            var rwp = Assert.Single(classDeclaration.Properties.Where(p => p.Name == nameof(ClassWithGetterSetterProperties.ReadWriteProperty)));
            Assert.True(rwp.HasGetter);
            Assert.True(rwp.HasSetter);

            var rop = Assert.Single(classDeclaration.Properties.Where(p => p.Name == nameof(ClassWithGetterSetterProperties.ReadOnlyProperty)));
            Assert.True(rop.HasGetter);
            Assert.False(rop.HasSetter);

            var wop = Assert.Single(classDeclaration.Properties.Where(p => p.Name == nameof(ClassWithGetterSetterProperties.WriteOnlyProperty)));
            Assert.False(wop.HasGetter);
            Assert.True(wop.HasSetter);
        }

        [Fact]
        public void LoadClassAttributes()
        {
            var type = typeof(AttributedClass);
            var declaration = DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper)
                .CreateClassDeclaration(type);

            Assert.NotNull(declaration);
            Assert.Equal(
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                declaration.Name);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.Load(declarationResolverMock.Object);

            Assert.NotNull(declaration.Attributes);
            var attribute = Assert.Single(declaration.Attributes);

            Assert.Equal(nameof(PatternAttribute), attribute.Name);

            Assert.NotNull(attribute.SyntaxNodeProvider);

            var node = attribute.SyntaxNodeProvider.SyntaxNode;
            Assert.NotNull(node);

            var attrText = node.ToString();
            Assert.Contains(nameof(PatternAttribute), attrText, StringComparison.OrdinalIgnoreCase);
        }
    }
}