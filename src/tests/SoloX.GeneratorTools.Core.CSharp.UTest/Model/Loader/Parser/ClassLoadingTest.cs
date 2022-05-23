﻿// ----------------------------------------------------------------------
// <copyright file="ClassLoadingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Loader
{
    public class ClassLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ClassLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(nameof(SimpleClass), null, null)]
        [InlineData(nameof(SimpleClassWithBase), null, nameof(SimpleClass))]
        [InlineData(nameof(SimpleClassWithGenericBase), null, nameof(GenericClass<object>))]
        [InlineData(nameof(GenericClass<object>), "T", null)]
        [InlineData(nameof(GenericClassWithBase<object>), "T", nameof(SimpleClass))]
        [InlineData(nameof(GenericClassWithGenericBase<object>), "T", nameof(GenericClass<object>))]
        public void LoadCSharpClassTest(string className, string typeParameterName, string baseClassName)
        {
            var location = className.ToBasicPath();
            var csFile = new CSharpFile(
                location,
                DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));

            csFile.Load(Mock.Of<ICSharpWorkspace>());

            Assert.Single(csFile.Declarations);
            var declaration = csFile.Declarations.Single();

            Assert.Equal(location, declaration.Location);

            var classDecl = Assert.IsType<ClassDeclaration>(declaration);

            classDecl.DeepLoad(Mock.Of<IDeclarationResolver>());

            Assert.NotNull(classDecl.GenericParameters);
            Assert.NotNull(classDecl.Extends);

            if (string.IsNullOrEmpty(typeParameterName))
            {
                Assert.Empty(classDecl.GenericParameters);
            }
            else
            {
                var paramDecl = Assert.Single(classDecl.GenericParameters);
                Assert.Equal(typeParameterName, paramDecl.Name);
            }

            if (string.IsNullOrEmpty(baseClassName))
            {
                Assert.Empty(classDecl.Extends);
            }
            else
            {
                var baseClass = Assert.Single(classDecl.Extends);
                Assert.Equal(baseClassName, baseClass.Declaration.Name);
            }
        }

        [Theory]
        [InlineData(nameof(GenericClassWithBase<object>), nameof(SimpleClass))]
        [InlineData(nameof(GenericClassWithGenericBase<object>), nameof(GenericClass<object>))]
        public void LoadExtendsTest(string className, string baseClassName)
        {
            var csFile = new CSharpFile(
                className.ToBasicPath(),
                DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));
            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                baseClassName);

            var classDecl = Assert.IsType<ClassDeclaration>(declaration);

            classDecl.DeepLoad(declarationResolver);

            Assert.NotNull(classDecl.GenericParameters);
            Assert.NotNull(classDecl.Extends);

            var extendsSingle = Assert.Single(classDecl.Extends);

            var genDeclUse = Assert.IsType<GenericDeclarationUse>(extendsSingle);
            Assert.Equal(baseClassName, genDeclUse.Declaration.Name);
        }

        [Theory]
        [InlineData(nameof(PatternAttributedClass), nameof(PatternAttribute))]
        [InlineData(nameof(RepeatAttributedClass), nameof(RepeatAttribute))]
        public void LoadClassAttributes(string className, string attributeName)
        {
            var file = className.ToBasicPath();

            var csFile = new CSharpFile(
                file,
                DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));
            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            var declarationResolver = this.SetupDeclarationResolver(declaration);

            var decl = Assert.IsType<ClassDeclaration>(declaration);

            decl.DeepLoad(declarationResolver);

            Assert.NotEmpty(decl.Attributes);
            var attribute = Assert.Single(decl.Attributes);

            Assert.Equal(attributeName, attribute.Name);

            Assert.NotNull(attribute.SyntaxNodeProvider);
        }

        [Theory]
        [InlineData(nameof(ClassWithProperties), false)]
        [InlineData(nameof(ClassWithArrayProperties), true)]
        public void LoadPropertyListTest(string className, bool isArray)
        {
            var file = className.ToBasicPath();

            var csFile = new CSharpFile(
                file,
                DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));
            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                nameof(SimpleClass));

            var decl = Assert.IsType<ClassDeclaration>(declaration);

            decl.DeepLoad(declarationResolver);

            Assert.Empty(decl.GenericParameters);
            Assert.Empty(decl.Extends);

            Assert.NotEmpty(decl.Members);
            Assert.Equal(2, decl.Members.Count);

            var mClass = Assert.Single(decl.Members.Where(m => m.Name == nameof(ClassWithProperties.PropertyClass)));
            var pClass = Assert.IsType<PropertyDeclaration>(mClass);
            Assert.IsType<GenericDeclarationUse>(pClass.PropertyType);
            Assert.Equal(nameof(SimpleClass), pClass.PropertyType.Declaration.Name);

            var mInt = Assert.Single(decl.Members.Where(m => m.Name == nameof(ClassWithProperties.PropertyInt)));
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
        [InlineData(nameof(ClassWithMethods), false)]
        [InlineData(nameof(ClassWithGenericMethods), true)]
        public void LoadMethodListTest(string className, bool isGeneric)
        {
            var file = className.ToBasicPath();

            var csFile = new CSharpFile(
                file,
                DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));
            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                nameof(SimpleClass));

            var decl = Assert.IsType<ClassDeclaration>(declaration);

            decl.DeepLoad(declarationResolver);

            Assert.Empty(decl.GenericParameters);
            Assert.Empty(decl.Extends);

            Assert.NotEmpty(decl.Members);
            Assert.Equal(2, decl.Members.Count);

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
            var className = nameof(ClassWithGetterSetterProperties);

            var file = className.ToBasicPath();

            var csFile = new CSharpFile(
                file,
                DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));
            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            var declarationResolver = this.SetupDeclarationResolver(declaration);

            var decl = Assert.IsType<ClassDeclaration>(declaration);

            decl.DeepLoad(declarationResolver);

            Assert.NotEmpty(decl.Properties);
            Assert.Equal(3, decl.Properties.Count);

            var rwp = Assert.Single(decl.Properties.Where(p => p.Name == nameof(ClassWithGetterSetterProperties.ReadWriteProperty)));
            Assert.True(rwp.HasGetter);
            Assert.True(rwp.HasSetter);

            var rop = Assert.Single(decl.Properties.Where(p => p.Name == nameof(ClassWithGetterSetterProperties.ReadOnlyProperty)));
            Assert.True(rop.HasGetter);
            Assert.False(rop.HasSetter);

            var wop = Assert.Single(decl.Properties.Where(p => p.Name == nameof(ClassWithGetterSetterProperties.WriteOnlyProperty)));
            Assert.False(wop.HasGetter);
            Assert.True(wop.HasSetter);
        }

        private IDeclarationResolver SetupDeclarationResolver(IDeclaration<SyntaxNode> contextDeclaration, params string[] classNames)
        {
            var declarationResolverMock = new Mock<IDeclarationResolver>();
            foreach (var className in classNames)
            {
                var classFile = new CSharpFile(
                    className.ToBasicPath(),
                    DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper));
                classFile.Load(Mock.Of<ICSharpWorkspace>());
                var classDeclarationSingle = Assert.Single(classFile.Declarations);
                if (classDeclarationSingle is IGenericDeclaration<SyntaxNode> genericDeclaration
                    && genericDeclaration.TypeParameterListSyntaxProvider.SyntaxNode != null)
                {
                    declarationResolverMock
                        .Setup(dr => dr.Resolve(genericDeclaration.Name, It.IsAny<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), contextDeclaration))
                        .Returns(genericDeclaration);
                }
                else
                {
                    declarationResolverMock
                        .Setup(dr => dr.Resolve(classDeclarationSingle.Name, contextDeclaration))
                        .Returns(classDeclarationSingle);
                }
            }

            return declarationResolverMock.Object;
        }
    }
}
