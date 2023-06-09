// ----------------------------------------------------------------------
// <copyright file="ClassLoadingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Parser
{
    public class ClassLoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly LoadingTest loadingTest;

        public ClassLoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.loadingTest = new LoadingTest(testOutputHelper);
        }

        [Theory]
        [InlineData(typeof(SimpleClass), null)]
        [InlineData(typeof(SimpleClassWithBase), typeof(SimpleClass))]
        [InlineData(typeof(GenericClassWithStructConstraint<>), null)]
        [InlineData(typeof(GenericClassWithBase<>), typeof(SimpleClass))]
        [InlineData(typeof(GenericClassWithGenericBase<>), typeof(GenericClass<>))]
        public void ItShouldLoadClassType(Type type, Type baseType)
        {
            var classDeclaration = LoadClassDeclaration(type);

            LoadingTest.AssertGenericTypeLoaded(classDeclaration, type, baseType);
        }

        [Theory]
        [InlineData(typeof(PatternAttributedClass), typeof(PatternAttribute))]
        [InlineData(typeof(RepeatAttributedClass), typeof(RepeatAttribute))]
        public void ItShouldLoadClassAttributes(Type type, Type attributeType)
        {
            var classDeclaration = LoadClassDeclaration(type);

            this.loadingTest.AssertClassAttributeLoaded(classDeclaration, attributeType);
        }

        [Theory]
        [InlineData(typeof(ClassWithProperties), false, false)]
        [InlineData(typeof(ClassWithNulableProperties), false, true)]
        [InlineData(typeof(ClassWithArrayProperties), true, false)]
        public void ItShouldLoadPropertyList(Type type, bool isArray, bool isNullable)
        {
            var classDeclaration = LoadClassDeclaration(type);

            this.loadingTest.AssertPropertyListLoaded(classDeclaration, isArray, isNullable);
        }

        [Theory]
        [InlineData(typeof(GenericClassWithProperties<>), nameof(GenericClassWithProperties<object>.Property))]
        [InlineData(typeof(GenericClassWithArrayProperties<>), nameof(GenericClassWithArrayProperties<object>.Property))]
        [InlineData(typeof(GenericClassWithArrayProperties2<>), nameof(GenericClassWithArrayProperties2<object>.Property))]
        public void ItShouldLoadGenericProperty(Type type, string propertyName)
        {
            var classDeclaration = LoadClassDeclaration(type);

            this.loadingTest.AssertGenericPropertyLoaded(classDeclaration, type, propertyName);
        }

        [Theory]
        [InlineData(typeof(ClassWithMethods), nameof(ClassWithMethods.ThisIsABasicMethod))]
        [InlineData(typeof(ClassWithMethods), nameof(ClassWithMethods.ThisIsAMethodWithParameters))]
        [InlineData(typeof(ClassWithGenericMethods), nameof(ClassWithGenericMethods.ThisIsABasicMethod))]
        [InlineData(typeof(ClassWithGenericMethods), nameof(ClassWithGenericMethods.ThisIsAMethodWithParameters))]
        public void ItShouldLoadMethod(Type type, string methodName)
        {
            var classDeclaration = LoadClassDeclaration(type);

            this.loadingTest.AssertMethodLoaded(classDeclaration, type, methodName);
        }

        [Theory]
        [InlineData(typeof(ClassWithGetterSetterProperties), nameof(ClassWithGetterSetterProperties.ReadWriteProperty))]
        [InlineData(typeof(ClassWithGetterSetterProperties), nameof(ClassWithGetterSetterProperties.WriteOnlyProperty))]
        [InlineData(typeof(ClassWithGetterSetterProperties), nameof(ClassWithGetterSetterProperties.ReadOnlyProperty))]
        public void IsShouldLoadPropertyWithGetterSetter(Type type, string propertyName)
        {
            var classDeclaration = LoadClassDeclaration(type);

            this.loadingTest.AssertPropertyGetterSetterLoaded(classDeclaration, type, propertyName);
        }

        [Theory]
        [InlineData(typeof(ClassWithPropertyAttributes), nameof(ClassWithPropertyAttributes.PropertyWithAttribute))]
        public void IsShouldLoadPropertyWithAttributes(Type type, string propertyName)
        {
            var classDeclaration = LoadClassDeclaration(type);

            this.loadingTest.AssertPropertyAttributesLoaded(classDeclaration, type, propertyName);
        }

        [Theory]
        [InlineData(typeof(ClassWithMethodAttributes), nameof(ClassWithMethodAttributes.MethodWithAttribute1), false)]
        [InlineData(typeof(ClassWithMethodAttributes), nameof(ClassWithMethodAttributes.MethodWithAttribute2), false)]
        [InlineData(typeof(ClassWithMethodAttributes), nameof(ClassWithMethodAttributes.MethodWithAttribute2), true)]
        public void IsShouldLoadMethodWithAttributes(Type type, string methodName, bool returnAttribute)
        {
            var classDeclaration = LoadClassDeclaration(type);

            this.loadingTest.AssertMethodAttributesLoaded(classDeclaration, type, methodName, returnAttribute);
        }

        [Theory]
        [InlineData(typeof(ClassWithMethodAttributes), nameof(ClassWithMethodAttributes.MethodWithAttribute1), 0)]
        [InlineData(typeof(ClassWithMethodAttributes), nameof(ClassWithMethodAttributes.MethodWithAttribute2), 0)]
        [InlineData(typeof(ClassWithMethodAttributes), nameof(ClassWithMethodAttributes.MethodWithAttribute3), 1)]
        public void IsShouldLoadMethodArgumentWithAttributes(Type type, string methodName, int argumentIndex)
        {
            var classDeclaration = LoadClassDeclaration(type);

            this.loadingTest.AssertMethodArgumentAttributesLoaded(classDeclaration, type, methodName, argumentIndex);
        }

        private IClassDeclaration LoadClassDeclaration(Type type)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            var location = className.ToBasicClassesPath();
            var csFile = new CSharpFile(
                location,
                DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                Mock.Of<IGlobalUsingDirectives>());

            csFile.Load(Mock.Of<ICSharpWorkspace>());

            var declaration = Assert.Single(csFile.Declarations);

            Assert.Equal(location, declaration.Location);

            var classDeclaration = Assert.IsAssignableFrom<IClassDeclaration>(declaration);
            return classDeclaration;
        }
    }
}
