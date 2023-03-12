// ----------------------------------------------------------------------
// <copyright file="ClassLoadingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Utils;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Reflection
{
    public class ClassLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ClassLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(typeof(SimpleClass), null)]
        [InlineData(typeof(SimpleClassWithBase), typeof(SimpleClass))]
        [InlineData(typeof(SimpleClassWithGenericBase), typeof(GenericClass<>))]
        [InlineData(typeof(GenericClass<>), null)]
        [InlineData(typeof(GenericClassWithBase<>), typeof(SimpleClass))]
        [InlineData(typeof(GenericClassWithGenericBase<>), typeof(GenericClass<>))]
        public void ItShouldLoadClassType(Type type, Type baseType)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var assemblyLoader = new CSharpAssembly(
                Mock.Of<IGeneratorLogger<CSharpAssembly>>(),
                DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper),
                type.Assembly);

            assemblyLoader.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(assemblyLoader.Declarations.Where(x => x.Name == className));

            var classDeclaration = Assert.IsAssignableFrom<IClassDeclaration>(declaration);

            LoadingTest.AssertGenericTypeLoaded(classDeclaration, type, baseType);
        }

        [Theory]
        [InlineData(typeof(GenericClassWithBase<>), nameof(SimpleClass))]
        [InlineData(typeof(GenericClassWithGenericBase<>), nameof(GenericClass<object>))]
        public void LoadExtendsTest(Type type, string baseClassName)
        {
            var declaration = DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper)
                .CreateDeclaration(type);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.DeepLoad(declarationResolverMock.Object);

            Assert.NotEmpty(classDeclaration.Extends);

            var baseClassDefinition = classDeclaration.Extends.First().Declaration;

            Assert.NotNull(baseClassDefinition);
            Assert.Equal(baseClassName, baseClassDefinition.Name);
        }

        [Theory]
        [InlineData(typeof(ClassWithProperties), false)]
        [InlineData(typeof(ClassWithArrayProperties), true)]
        public void LoadPropertyListTest(Type type, bool isArray)
        {
            var declarationFactory = DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper);
            var declaration = declarationFactory.CreateDeclaration(type);
            var simpleDeclaration = declarationFactory.CreateDeclaration(typeof(SimpleClass));

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);
            var simpleClassDeclaration = Assert.IsType<ClassDeclaration>(simpleDeclaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            declarationResolverMock.Setup(r => r.Resolve(typeof(SimpleClass))).Returns(simpleClassDeclaration);
            classDeclaration.DeepLoad(declarationResolverMock.Object);

            Assert.NotEmpty(classDeclaration.Properties);
            Assert.Equal(2, classDeclaration.Properties.Count);

            var mClass = Assert.Single(classDeclaration.Members.Where(m => m.Name == nameof(ClassWithProperties.PropertyClass)));
            var pClass = Assert.IsType<PropertyDeclaration>(mClass);

            Assert.IsType<GenericDeclarationUse>(pClass.PropertyType);
            Assert.Equal(nameof(SimpleClass), pClass.PropertyType.Declaration.Name);

            var mInt = Assert.Single(classDeclaration.Members.Where(m => m.Name == nameof(ClassWithProperties.PropertyInt)));
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

        [Theory]
        [InlineData(typeof(ClassWithMethods), false)]
        [InlineData(typeof(ClassWithGenericMethods), true)]
        public void LoadMethodListTest(Type type, bool isGeneric)
        {
            var declarationFactory = DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper);
            var declaration = declarationFactory.CreateDeclaration(type);
            var simpleDeclaration = declarationFactory.CreateDeclaration(typeof(SimpleClass));

            var decl = Assert.IsType<ClassDeclaration>(declaration);
            var simpleClassDeclaration = Assert.IsType<ClassDeclaration>(simpleDeclaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            declarationResolverMock.Setup(r => r.Resolve(typeof(SimpleClass))).Returns(simpleClassDeclaration);
            decl.DeepLoad(declarationResolverMock.Object);

            Assert.NotEmpty(decl.Methods);
            Assert.Equal(2, decl.Methods.Count);

            var mClass = Assert.Single(decl.Members.Where(m => m.Name == nameof(ClassWithMethods.ThisIsABasicMethod)));
            var methodClass = Assert.IsType<MethodDeclaration>(mClass);
            Assert.IsType<GenericDeclarationUse>(methodClass.ReturnType);
            Assert.Equal(nameof(SimpleClass), methodClass.ReturnType.Declaration.Name);

            Assert.Empty(methodClass.Parameters);

            var mInt = Assert.Single(decl.Members.Where(m => m.Name == nameof(ClassWithMethods.ThisIsAMethodWithParameters)));
            var methodInt = Assert.IsType<MethodDeclaration>(mInt);
            Assert.IsType<PredefinedDeclarationUse>(methodInt.ReturnType);
            Assert.Equal("int", methodInt.ReturnType.Declaration.Name);

            Assert.NotEmpty(methodInt.Parameters);
            Assert.Equal(3, methodInt.Parameters.Count);

            if (isGeneric)
            {
                Assert.NotNull(methodClass.GenericParameters);
                Assert.NotNull(methodInt.GenericParameters);

                Assert.NotEmpty(methodClass.GenericParameters);
                Assert.NotEmpty(methodInt.GenericParameters);
            }
            else
            {
                Assert.NotNull(methodClass.GenericParameters);
                Assert.NotNull(methodInt.GenericParameters);
                Assert.Empty(methodClass.GenericParameters);
                Assert.Empty(methodInt.GenericParameters);
            }
        }

        [Fact]
        public void LoadGetterSetterPropertyTest()
        {
            var type = typeof(ClassWithGetterSetterProperties);

            var declarationFactory = DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper);
            var declaration = declarationFactory.CreateDeclaration(type);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.DeepLoad(declarationResolverMock.Object);

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

        [Theory]
        [InlineData(typeof(PatternAttributedClass), nameof(PatternAttribute))]
        [InlineData(typeof(RepeatAttributedClass), nameof(RepeatAttribute))]
        public void LoadClassAttributes(Type type, string attributeName)
        {
            var declaration = DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper)
                .CreateDeclaration(type);

            Assert.NotNull(declaration);
            Assert.Equal(
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                declaration.Name);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);

            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.DeepLoad(declarationResolverMock.Object);

            Assert.NotNull(declaration.Attributes);
            var attribute = Assert.Single(declaration.Attributes);

            Assert.Equal(attributeName, attribute.Name);

            Assert.NotNull(attribute.SyntaxNodeProvider);

            var node = attribute.SyntaxNodeProvider.SyntaxNode;
            Assert.NotNull(node);

            var attrText = node.ToString();
            Assert.Contains(attributeName, attrText, StringComparison.OrdinalIgnoreCase);
        }
    }
}