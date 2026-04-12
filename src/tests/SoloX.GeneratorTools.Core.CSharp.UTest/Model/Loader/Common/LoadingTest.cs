// ----------------------------------------------------------------------
// <copyright file="LoadingTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using NSubstitute;
using Shouldly;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common
{
    public class LoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public LoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public void AssertGenericTypeLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, Type? baseType, bool isRecord)
            where TSyntaxNode : SyntaxNode
        {
            Assert.Equal(
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                declaration.Name);

            declaration.IsRecordType.ShouldBe(isRecord);

            Assert.NotNull(declaration.SyntaxNodeProvider);
            Assert.NotNull(declaration.SyntaxNodeProvider.SyntaxNode);

            var classDeclaration = Assert.IsAssignableFrom<AGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Null(classDeclaration.GenericParameters);
            Assert.Null(classDeclaration.Extends);
            Assert.Null(classDeclaration.Members);

            var declarationResolver = SetupDeclarationResolver(declaration);

            classDeclaration.DeepLoad(declarationResolver);

            Assert.NotNull(classDeclaration.GenericParameters);
            Assert.NotNull(classDeclaration.Extends);
            Assert.NotNull(classDeclaration.Members);

            if (type.IsGenericTypeDefinition)
            {
                classDeclaration.GenericParameters.ShouldNotBeEmpty();

                var typeParams = type.GetTypeInfo().GenericTypeParameters;
                Assert.Equal(typeParams.Length, classDeclaration.GenericParameters.Count);

                Assert.Equal(typeParams[0].Name, classDeclaration.GenericParameters.First().Name);
                Assert.Equal(typeParams[0].IsValueType, classDeclaration.GenericParameters.First().IsValueType);
            }
            else
            {
                Assert.Empty(classDeclaration.GenericParameters);
            }

            if (baseType != null)
            {
                var baseClassName = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(baseType.Name);

                var extendNames = classDeclaration.Extends.Select(x => x.Declaration.Name);

                extendNames.Where(x => x == baseClassName).ShouldHaveSingleItem();
            }
        }

        public void AssertClassAttributeLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type attributeType)
            where TSyntaxNode : SyntaxNode
        {
            var attributeName = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(attributeType.Name);

            var declarationResolver = SetupDeclarationResolver(declaration);

            declaration.DeepLoad(declarationResolver);

            Assert.NotEmpty(declaration.Attributes);
            var attribute = Assert.Single(declaration.Attributes);

            Assert.Equal(attributeName, attribute.Name);

            Assert.NotNull(attribute.SyntaxNodeProvider);

            var node = attribute.SyntaxNodeProvider.SyntaxNode;
            Assert.NotNull(node);

            var attrText = node.ToString();
            Assert.Contains(attributeName.Replace(nameof(Attribute), string.Empty, StringComparison.Ordinal), attrText, StringComparison.OrdinalIgnoreCase);
        }

        public void AssertPropertyListLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, bool isArray, bool isNullable)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                typeof(SimpleClass));

            var nullableMock = Substitute.For<IGenericDeclaration<TSyntaxNode>>();
            nullableMock.Name.Returns("Nullable");

            declarationResolver
                .Resolve("System.Nullable", Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), declaration)
                .Returns(nullableMock);

            declarationResolver
                .Resolve(typeof(Nullable<int>))
                .Returns(nullableMock);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Empty(classDeclaration.GenericParameters);

            Assert.Empty(classDeclaration.Extends);

            classDeclaration.Members.ShouldNotBeEmpty();
            Assert.Equal(2, classDeclaration.Properties.Count);

            Assert.Empty(classDeclaration.Methods);

            var mClass = classDeclaration.NamedMembers
                .Where(m => m.Name == nameof(ClassWithProperties.PropertyClass))
                .ShouldHaveSingleItem();
            var pClass = Assert.IsType<PropertyDeclaration>(mClass);
            Assert.IsType<GenericDeclarationUse>(pClass.PropertyType);
            Assert.Equal(nameof(SimpleClass), pClass.PropertyType.Declaration.Name);

            var mInt = classDeclaration.NamedMembers
                .Where(m => m.Name == nameof(ClassWithProperties.PropertyInt))
                .ShouldHaveSingleItem();
            var pInt = Assert.IsType<PropertyDeclaration>(mInt);

            var propertyType = pInt.PropertyType;

            if (isNullable)
            {
                var genPropertyType = Assert.IsType<GenericDeclarationUse>(pInt.PropertyType);
                Assert.Equal("Nullable", genPropertyType.Declaration.Name);

                propertyType = genPropertyType.GenericParameters.Single();
            }

            Assert.IsType<PredefinedDeclarationUse>(propertyType);
            Assert.Equal("int", propertyType.Declaration.Name);

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

        public void AssertIndexerLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                typeof(SimpleClass));

            var nullableMock = Substitute.For<IGenericDeclaration<TSyntaxNode>>();
            nullableMock.Name.Returns("Nullable");

            declarationResolver
                .Resolve("System.Nullable", Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), declaration)
                .Returns(nullableMock);

            declarationResolver
                .Resolve(typeof(Nullable<int>))
                .Returns(nullableMock);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Empty(classDeclaration.GenericParameters);

            Assert.Empty(classDeclaration.Extends);

            classDeclaration.Members.ShouldNotBeEmpty();

            Assert.Empty(classDeclaration.Properties);

            var indexerDeclaration = Assert.Single(classDeclaration.Indexers);

            Assert.Single(indexerDeclaration.Parameters);

        }

        public void AssertRecordPropertyListLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type recordType)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                typeof(SimpleClass));

            var nullableMock = Substitute.For<IGenericDeclaration<TSyntaxNode>>();
            nullableMock.Name.Returns("Nullable");

            declarationResolver
                .Resolve("System.Nullable", Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), declaration)
                .Returns(nullableMock);

            declarationResolver
                .Resolve(typeof(Nullable<int>))
                .Returns(nullableMock);

            declaration.IsRecordType.ShouldBeTrue();

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Empty(classDeclaration.GenericParameters);

            classDeclaration.Extends.ShouldBeEmpty();

            classDeclaration.Members.ShouldNotBeEmpty();

            classDeclaration.Properties.Count.ShouldBe(recordType.GetProperties().Length);

            var props = new HashSet<string>(recordType.GetProperties().Select(p => p.Name));

            foreach (var property in classDeclaration.Properties)
            {
                props.Contains(property.Name).ShouldBeTrue();
            }
        }

        public void AssertConstantListLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, int nbConst, string nameOfConst)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(declaration);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Empty(classDeclaration.GenericParameters);
            Assert.Empty(classDeclaration.Extends);

            classDeclaration.Members.ShouldNotBeEmpty();
            Assert.Equal(nbConst, classDeclaration.Constants.Count);

            var constNameDecl = classDeclaration.Constants.SingleOrDefault(c => c.Name == nameOfConst);

            constNameDecl.ShouldNotBeNull();
            constNameDecl.SyntaxNodeProvider.ShouldNotBeNull();
            constNameDecl.SyntaxNodeProvider.SyntaxNode.ShouldNotBeNull();
        }

        public void AssertMethodLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string methodName)
            where TSyntaxNode : SyntaxNode
        {
            var classDeclaration = Assert.IsAssignableFrom<AGenericDeclaration<TSyntaxNode>>(declaration);

            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                typeof(SimpleClass));

            declarationResolver.Resolve(type).Returns(classDeclaration);

            declaration.DeepLoad(declarationResolver);

            Assert.Empty(classDeclaration.GenericParameters);
            Assert.Empty(classDeclaration.Extends);

            classDeclaration.Members.ShouldNotBeEmpty();

            var method = type.GetMethod(methodName);

            var memberDeclaration = classDeclaration.NamedMembers
                .Where(m => m.Name == methodName)
                .ShouldHaveSingleItem();
            var methodDeclaration = Assert.IsAssignableFrom<IMethodDeclaration>(memberDeclaration);

            if (method.ReturnType == typeof(int))
            {
                Assert.IsAssignableFrom<IPredefinedDeclarationUse>(methodDeclaration.ReturnType);
                Assert.Equal("int", methodDeclaration.ReturnType.Declaration.Name);
            }
            else
            {
                Assert.IsAssignableFrom<IGenericDeclarationUse>(methodDeclaration.ReturnType);
                Assert.Equal(method.ReturnType.Name, methodDeclaration.ReturnType.Declaration.Name);
            }

            if (method.GetParameters().Length > 0)
            {
                Assert.NotEmpty(methodDeclaration.Parameters);
                Assert.Equal(method.GetParameters().Length, methodDeclaration.Parameters.Count);
            }
            else
            {
                Assert.Empty(methodDeclaration.Parameters);
            }

            Assert.NotNull(methodDeclaration.GenericParameters);

            if (method.ContainsGenericParameters)
            {
                Assert.NotEmpty(methodDeclaration.GenericParameters);
            }
            else
            {
                Assert.Empty(methodDeclaration.GenericParameters);
            }
        }

        public void AssertGenericPropertyLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string propertyName)
            where TSyntaxNode : SyntaxNode
        {
            var classDeclaration = Assert.IsAssignableFrom<AGenericDeclaration<TSyntaxNode>>(declaration);

            var declarationResolver = this.SetupDeclarationResolver(
                declaration);

            foreach (var parameter in type.GetTypeInfo().GenericTypeParameters)
            {
                var parameterDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
                parameterDeclaration.Name.Returns(parameter.Name);

                declarationResolver
                    .Resolve(parameter)
                    .Returns(parameterDeclaration);
            }

            declarationResolver.Resolve(type).Returns(classDeclaration);

            declaration.DeepLoad(declarationResolver);

            classDeclaration.Properties.ShouldNotBeEmpty();

            var m = classDeclaration.NamedMembers
                .Where(m => m.Name == propertyName)
                .ShouldHaveSingleItem();
            var p = Assert.IsType<PropertyDeclaration>(m);

            Assert.IsType<GenericParameterDeclarationUse>(p.PropertyType);
            Assert.Equal("T", p.PropertyType.Declaration.Name);

            var property = type.GetProperty(propertyName);

            if (property.PropertyType.IsArray)
            {
                Assert.NotNull(p.PropertyType.ArraySpecification);
            }
            else
            {
                Assert.Null(p.PropertyType.ArraySpecification);
            }

        }

        public void AssertPropertyGetterSetterLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string propertyName)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            classDeclaration.Members.ShouldNotBeEmpty();

            classDeclaration.Properties.ShouldNotBeEmpty();

            var mDeclaration = classDeclaration.NamedMembers.Where(m => m.Name == propertyName).ShouldHaveSingleItem();
            var pDeclaration = mDeclaration.ShouldBeOfType<PropertyDeclaration>();

            var property = type.GetProperty(propertyName);

            Assert.Equal(property.CanRead, pDeclaration.HasGetter);
            Assert.Equal(property.CanWrite, pDeclaration.HasSetter);
        }

        public void AssertPropertyAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string propertyName, string descriptionArgument)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration);

            var attributeDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            attributeDeclaration.Name.Returns(nameof(DescriptionAttribute));

            declarationResolver
                .Resolve(nameof(DescriptionAttribute), Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns(attributeDeclaration);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = declaration.ShouldBeAssignableTo<IGenericDeclaration<TSyntaxNode>>();

            classDeclaration.Members.ShouldNotBeEmpty();

            classDeclaration.Properties.ShouldNotBeEmpty();

            var mDeclaration = classDeclaration.NamedMembers.Where(m => m.Name == propertyName).ShouldHaveSingleItem();
            var pDeclaration = mDeclaration.ShouldBeOfType<PropertyDeclaration>();

            pDeclaration.Attributes.ShouldNotBeNull();
            pDeclaration.Attributes.ShouldNotBeEmpty();

            var attributeUse = pDeclaration.Attributes.ShouldHaveSingleItem();

            attributeUse.Name.ShouldBe(nameof(DescriptionAttribute));

            attributeUse.ConstructorArguments.ShouldHaveSingleItem().ShouldBe(descriptionArgument);
        }

        public void AssertPropertyTestAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string propertyName, string typeName, bool named, string attributeName)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration);

            var attributeDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            attributeDeclaration.Name.Returns(nameof(DescriptionAttribute));

            declarationResolver
                .Resolve(nameof(DescriptionAttribute), Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns(attributeDeclaration);

            var objectDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            objectDeclaration.Name.Returns("object");

            declarationResolver
                .Resolve("System.Object", Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns(objectDeclaration);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = declaration.ShouldBeAssignableTo<IGenericDeclaration<TSyntaxNode>>();

            classDeclaration.Members.ShouldNotBeEmpty();

            classDeclaration.Properties.ShouldNotBeEmpty();

            var mDeclaration = classDeclaration.NamedMembers.Where(m => m.Name == propertyName).ShouldHaveSingleItem();
            var pDeclaration = mDeclaration.ShouldBeOfType<PropertyDeclaration>();

            pDeclaration.Attributes.ShouldNotBeNull();
            pDeclaration.Attributes.ShouldNotBeEmpty();

            var attributeUse = pDeclaration.Attributes.ShouldHaveSingleItem();

            attributeUse.Name.ShouldBe(attributeName);

            var arg = named
                ? attributeUse.NamedArguments.ShouldHaveSingleItem().Value
                : attributeUse.ConstructorArguments.ShouldHaveSingleItem();

            arg.ShouldNotBeNull();

            var use = arg.ShouldBeAssignableTo<IDeclarationUse<SyntaxNode>>();

            use.Declaration.Name.ShouldBeEquivalentTo(typeName);
        }

        public void AssertPropertyTestGenericAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string propertyName, string attributeName)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration);

            var attributeDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            attributeDeclaration.Name.Returns(nameof(DescriptionAttribute));

            declarationResolver
                .Resolve(nameof(DescriptionAttribute), Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns(attributeDeclaration);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            classDeclaration.Members.ShouldNotBeEmpty();

            classDeclaration.Properties.ShouldNotBeEmpty();

            var mDeclaration = classDeclaration.NamedMembers.Where(m => m.Name == propertyName).ShouldHaveSingleItem();
            var pDeclaration = mDeclaration.ShouldBeOfType<PropertyDeclaration>();

            pDeclaration.Attributes.ShouldNotBeNull();
            pDeclaration.Attributes.ShouldNotBeEmpty();

            var attributeUse = pDeclaration.Attributes.ShouldHaveSingleItem();

            attributeUse.Name.ShouldBe(attributeName);
        }

        public void AssertMethodAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string methodName, bool returnAttribute)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration);

            var attributeDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            attributeDeclaration.Name.Returns(nameof(DescriptionAttribute));

            declarationResolver
                .Resolve(nameof(DescriptionAttribute), Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns(attributeDeclaration);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            classDeclaration.Members.ShouldNotBeEmpty();

            classDeclaration.Methods.ShouldNotBeEmpty();

            var mDeclaration = classDeclaration.NamedMembers.Where(m => m.Name == methodName).ShouldHaveSingleItem();
            var methodDeclaration = mDeclaration.ShouldBeOfType<MethodDeclaration>();

            if (returnAttribute)
            {
                methodDeclaration.ReturnAttributes.ShouldNotBeNull();
                methodDeclaration.ReturnAttributes.ShouldNotBeEmpty();
                methodDeclaration.ReturnAttributes.ShouldHaveSingleItem();
            }
            else
            {
                methodDeclaration.Attributes.ShouldNotBeNull();
                methodDeclaration.Attributes.ShouldNotBeEmpty();
                methodDeclaration.Attributes.ShouldHaveSingleItem();
            }
        }

        public void AssertMethodArgumentAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string methodName, int argumentIndex)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration);

            var attributeDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            attributeDeclaration.Name.Returns(nameof(DescriptionAttribute));

            declarationResolver
                .Resolve(nameof(DescriptionAttribute), Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns(attributeDeclaration);

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            classDeclaration.Members.ShouldNotBeEmpty();

            classDeclaration.Methods.ShouldNotBeEmpty();

            var mDeclaration = classDeclaration.NamedMembers.Where(m => m.Name == methodName).ShouldHaveSingleItem();
            var methodDeclaration = mDeclaration.ShouldBeOfType<MethodDeclaration>();

            methodDeclaration.Parameters.Count.ShouldBeGreaterThan(argumentIndex);

            var methodParameter = methodDeclaration.Parameters.ElementAt(argumentIndex);
            methodParameter.Attributes.ShouldNotBeNull();
            methodParameter.Attributes.ShouldNotBeEmpty();
            methodParameter.Attributes.ShouldHaveSingleItem();
        }

        public void AssertEnumTypeLoaded(IEnumDeclaration enumDeclaration, Type type)
        {
            var className = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name);

            Assert.Equal(
                className,
                enumDeclaration.Name);

            Assert.NotNull(enumDeclaration.SyntaxNodeProvider);
            Assert.NotNull(enumDeclaration.SyntaxNodeProvider.SyntaxNode);

            var declarationResolver = this.SetupDeclarationResolver(
                enumDeclaration);

            enumDeclaration.DeepLoad(declarationResolver);

            var underlyingType = type.GetEnumUnderlyingType();

            if (underlyingType != typeof(int))
            {
                Assert.NotNull(enumDeclaration.UnderlyingType);
            }

            if (type.CustomAttributes.Any())
            {
                enumDeclaration.Attributes.ShouldNotBeNull();
                enumDeclaration.Attributes.ShouldNotBeEmpty();
            }
        }

        private IDeclarationResolver SetupDeclarationResolver(
            IDeclaration<SyntaxNode> contextDeclaration,
            params Type[] classes)
        {
            var declarationResolverMock = Substitute.For<IDeclarationResolver>();

            declarationResolverMock
                .Resolve(Arg.Any<string>(), Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns((IGenericDeclaration<SyntaxNode>?)null);

            declarationResolverMock
                .Resolve(Arg.Any<string>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns((IDeclaration<SyntaxNode>?)null);

            declarationResolverMock
                .Resolve(Arg.Any<Type>())
                .Returns((IGenericDeclaration<SyntaxNode>?)null);

            foreach (var classItem in classes)
            {
                var className = classItem.Name;

                var classFile = new CSharpFile(
                    className.ToBasicClassesPath(),
                    DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                    Substitute.For<IGlobalUsingDirectives>());
                classFile.Load(Substitute.For<ICSharpWorkspace>());
                var classDeclarationSingle = Assert.Single(classFile.Declarations);
                if (classDeclarationSingle is IGenericDeclaration<SyntaxNode> genericDeclaration)
                {
                    if (genericDeclaration.TypeParameterListSyntaxProvider.SyntaxNode != null)
                    {
                        declarationResolverMock
                            .Resolve(genericDeclaration.Name, Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), contextDeclaration)
                            .Returns(genericDeclaration);
                    }
                    else
                    {
                        declarationResolverMock
                            .Resolve(classDeclarationSingle.Name, contextDeclaration)
                            .Returns(classDeclarationSingle);
                        declarationResolverMock
                            .Resolve(classItem)
                            .Returns(genericDeclaration);
                    }
                }
                else
                {
                    declarationResolverMock
                            .Resolve(classDeclarationSingle.Name, contextDeclaration)
                            .Returns(classDeclarationSingle);
                }
            }

            return declarationResolverMock;
        }
    }
}
