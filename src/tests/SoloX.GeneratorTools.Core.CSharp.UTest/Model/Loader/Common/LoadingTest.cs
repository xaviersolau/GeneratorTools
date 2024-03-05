// ----------------------------------------------------------------------
// <copyright file="LoadingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using Xunit;
using System;
using System.Reflection;
using System.Linq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using System.Collections.Generic;
using Xunit.Abstractions;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes;
using FluentAssertions;
using System.ComponentModel;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common
{
    public class LoadingTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public LoadingTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public static void AssertGenericTypeLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, Type? baseType, bool isRecord)
            where TSyntaxNode : SyntaxNode
        {
            Assert.Equal(
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                declaration.Name);

            declaration.IsRecordType.Should().Be(isRecord);

            Assert.NotNull(declaration.SyntaxNodeProvider);
            Assert.NotNull(declaration.SyntaxNodeProvider.SyntaxNode);

            var classDeclaration = Assert.IsAssignableFrom<AGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Null(classDeclaration.GenericParameters);
            Assert.Null(classDeclaration.Extends);
            Assert.Null(classDeclaration.Members);

            classDeclaration.DeepLoad(Mock.Of<IDeclarationResolver>());

            Assert.NotNull(classDeclaration.GenericParameters);
            Assert.NotNull(classDeclaration.Extends);
            Assert.NotNull(classDeclaration.Members);

            if (type.IsGenericTypeDefinition)
            {
                Assert.NotEmpty(classDeclaration.GenericParameters);

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

                Assert.Single(extendNames.Where(x => x == baseClassName));
            }
        }

        public void AssertClassAttributeLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type attributeType)
            where TSyntaxNode : SyntaxNode
        {
            var attributeName = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(attributeType.Name);

            var declarationResolver = SetupDeclarationResolver(declaration,
                mock =>
                {
                    var attributeMock = new Mock<IGenericDeclaration<TSyntaxNode>>();
                    attributeMock.SetupGet(x => x.Name).Returns(attributeName);

                    mock
                        .Setup(dr => dr.Resolve(attributeName, It.IsAny<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), declaration))
                        .Returns(attributeMock.Object);

                    mock
                        .Setup(dr => dr.Resolve(attributeType))
                        .Returns(attributeMock.Object);
                });

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
                (mock) =>
                {
                    var nullableMock = new Mock<IGenericDeclaration<TSyntaxNode>>();
                    nullableMock.Setup(d => d.Name).Returns("Nullable");

                    mock
                        .Setup(dr => dr.Resolve("System.Nullable", It.IsAny<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), declaration))
                        .Returns(nullableMock.Object);

                    mock
                        .Setup(dr => dr.Resolve(typeof(Nullable<int>)))
                        .Returns(nullableMock.Object);
                },
                typeof(SimpleClass));

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Empty(classDeclaration.GenericParameters);

            Assert.Empty(classDeclaration.Extends);

            Assert.NotEmpty(classDeclaration.Members);
            Assert.Equal(2, classDeclaration.Properties.Count);

            Assert.Empty(classDeclaration.Methods);

            var mClass = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == nameof(ClassWithProperties.PropertyClass)));
            var pClass = Assert.IsType<PropertyDeclaration>(mClass);
            Assert.IsType<GenericDeclarationUse>(pClass.PropertyType);
            Assert.Equal(nameof(SimpleClass), pClass.PropertyType.Declaration.Name);

            var mInt = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == nameof(ClassWithProperties.PropertyInt)));
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
                (mock) =>
                {
                    var nullableMock = new Mock<IGenericDeclaration<TSyntaxNode>>();
                    nullableMock.Setup(d => d.Name).Returns("Nullable");

                    mock
                        .Setup(dr => dr.Resolve("System.Nullable", It.IsAny<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), declaration))
                        .Returns(nullableMock.Object);

                    mock
                        .Setup(dr => dr.Resolve(typeof(Nullable<int>)))
                        .Returns(nullableMock.Object);
                },
                typeof(SimpleClass));

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Empty(classDeclaration.GenericParameters);

            Assert.Empty(classDeclaration.Extends);

            Assert.NotEmpty(classDeclaration.Members);

            Assert.Empty(classDeclaration.Properties);

            var indexerDeclaration = Assert.Single(classDeclaration.Indexers);

            Assert.Single(indexerDeclaration.Parameters);

        }

        public void AssertRecordPropertyListLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type recordType)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                (mock) =>
                {
                    var nullableMock = new Mock<IGenericDeclaration<TSyntaxNode>>();
                    nullableMock.Setup(d => d.Name).Returns("Nullable");

                    mock
                        .Setup(dr => dr.Resolve("System.Nullable", It.IsAny<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), declaration))
                        .Returns(nullableMock.Object);

                    mock
                        .Setup(dr => dr.Resolve(typeof(Nullable<int>)))
                        .Returns(nullableMock.Object);
                },
                typeof(SimpleClass));

            declaration.IsRecordType.Should().BeTrue();

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Empty(classDeclaration.GenericParameters);

            classDeclaration.Extends.Should().BeEmpty();

            Assert.NotEmpty(classDeclaration.Members);

            classDeclaration.Properties.Should().HaveCount(recordType.GetProperties().Length);

            var props = new HashSet<string>(recordType.GetProperties().Select(p => p.Name));

            foreach (var property in classDeclaration.Properties)
            {
                props.Contains(property.Name).Should().BeTrue();
            }
        }

        public void AssertConstantListLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, int nbConst, string nameOfConst)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                (mock) =>
                {
                    // nothing
                });

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Empty(classDeclaration.GenericParameters);
            Assert.Empty(classDeclaration.Extends);

            Assert.NotEmpty(classDeclaration.Members);
            Assert.Equal(nbConst, classDeclaration.Constants.Count);

            var constNameDecl = classDeclaration.Constants.SingleOrDefault(c => c.Name == nameOfConst);

            constNameDecl.Should().NotBeNull();
            constNameDecl.SyntaxNodeProvider.Should().NotBeNull();
            constNameDecl.SyntaxNodeProvider.SyntaxNode.Should().NotBeNull();
        }

        public void AssertMethodLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string methodName)
            where TSyntaxNode : SyntaxNode
        {
            var classDeclaration = Assert.IsAssignableFrom<AGenericDeclaration<TSyntaxNode>>(declaration);

            var declarationResolver = this.SetupDeclarationResolver(
                declaration,
                mock =>
                {
                    mock.Setup(r => r.Resolve(type)).Returns(classDeclaration);
                },
                typeof(SimpleClass));

            declaration.DeepLoad(declarationResolver);

            Assert.Empty(classDeclaration.GenericParameters);
            Assert.Empty(classDeclaration.Extends);

            Assert.NotEmpty(classDeclaration.Members);

            var method = type.GetMethod(methodName);

            var memberDeclaration = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == methodName));
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
                declaration,
                mock =>
                {
                    foreach (var parameter in type.GetTypeInfo().GenericTypeParameters)
                    {
                        var parameterDeclaration = new Mock<IGenericDeclaration<SyntaxNode>>();
                        parameterDeclaration.Setup(d => d.Name).Returns(parameter.Name);

                        mock
                            .Setup(dr => dr.Resolve(parameter))
                            .Returns(parameterDeclaration.Object);
                    }

                    mock.Setup(r => r.Resolve(type)).Returns(classDeclaration);
                });

            declaration.DeepLoad(declarationResolver);

            Assert.NotEmpty(classDeclaration.Properties);

            var m = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == propertyName));
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

            Assert.NotEmpty(classDeclaration.Members);

            Assert.NotEmpty(classDeclaration.Properties);

            var mDeclaration = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == propertyName));
            var pDeclaration = Assert.IsType<PropertyDeclaration>(mDeclaration);

            var property = type.GetProperty(propertyName);

            Assert.Equal(property.CanRead, pDeclaration.HasGetter);
            Assert.Equal(property.CanWrite, pDeclaration.HasSetter);
        }

        public void AssertPropertyAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string propertyName, string descriptionArgument)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration, resolver =>
                {
                    var attributeDeclaration = new Mock<IGenericDeclaration<SyntaxNode>>();
                    attributeDeclaration.Setup(d => d.Name).Returns(nameof(DescriptionAttribute));

                    resolver
                        .Setup(r => r.Resolve(nameof(DescriptionAttribute), Array.Empty<IDeclarationUse<SyntaxNode>>(), It.IsAny<IDeclaration<SyntaxNode>>()))
                        .Returns(attributeDeclaration.Object);
                });

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.NotEmpty(classDeclaration.Members);

            Assert.NotEmpty(classDeclaration.Properties);

            var mDeclaration = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == propertyName));
            var pDeclaration = Assert.IsType<PropertyDeclaration>(mDeclaration);

            pDeclaration.Attributes.Should().NotBeNullOrEmpty();

            var attributeUse = pDeclaration.Attributes.Should().ContainSingle().Subject;

            attributeUse.Name.Should().Be(nameof(DescriptionAttribute));

            attributeUse.ConstructorArguments.Should().ContainSingle().Subject.Should().Be(descriptionArgument);
        }

        public void AssertPropertyTestAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string propertyName, string typeName, bool named, string attributeName)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration, resolver =>
                {
                    var attributeDeclaration = new Mock<IGenericDeclaration<SyntaxNode>>();
                    attributeDeclaration.Setup(d => d.Name).Returns(nameof(DescriptionAttribute));

                    resolver
                        .Setup(r => r.Resolve(nameof(DescriptionAttribute), Array.Empty<IDeclarationUse<SyntaxNode>>(), It.IsAny<IDeclaration<SyntaxNode>>()))
                        .Returns(attributeDeclaration.Object);
                });

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.NotEmpty(classDeclaration.Members);

            Assert.NotEmpty(classDeclaration.Properties);

            var mDeclaration = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == propertyName));
            var pDeclaration = Assert.IsType<PropertyDeclaration>(mDeclaration);

            pDeclaration.Attributes.Should().NotBeNullOrEmpty();

            var attributeUse = pDeclaration.Attributes.Should().ContainSingle().Subject;

            attributeUse.Name.Should().Be(attributeName);

            var arg = named
                ? attributeUse.NamedArguments.Should().ContainSingle().Subject.Value
                : attributeUse.ConstructorArguments.Should().ContainSingle().Subject;

            arg.Should().NotBeNull();

            var use = arg.Should().BeAssignableTo<IDeclarationUse<SyntaxNode>>().Subject;

            use.Declaration.Name.Should().BeEquivalentTo(typeName);
        }

        public void AssertPropertyTestGenericAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string propertyName, string attributeName)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration, resolver =>
                {
                    var attributeDeclaration = new Mock<IGenericDeclaration<SyntaxNode>>();
                    attributeDeclaration.Setup(d => d.Name).Returns(nameof(DescriptionAttribute));

                    resolver
                        .Setup(r => r.Resolve(nameof(DescriptionAttribute), Array.Empty<IDeclarationUse<SyntaxNode>>(), It.IsAny<IDeclaration<SyntaxNode>>()))
                        .Returns(attributeDeclaration.Object);
                });

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.NotEmpty(classDeclaration.Members);

            Assert.NotEmpty(classDeclaration.Properties);

            var mDeclaration = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == propertyName));
            var pDeclaration = Assert.IsType<PropertyDeclaration>(mDeclaration);

            pDeclaration.Attributes.Should().NotBeNullOrEmpty();

            var attributeUse = pDeclaration.Attributes.Should().ContainSingle().Subject;

            attributeUse.Name.Should().Be(attributeName);
        }

        public void AssertMethodAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string methodName, bool returnAttribute)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration, resolver =>
                {
                    var attributeDeclaration = new Mock<IGenericDeclaration<SyntaxNode>>();
                    attributeDeclaration.Setup(d => d.Name).Returns(nameof(DescriptionAttribute));

                    resolver
                        .Setup(r => r.Resolve(nameof(DescriptionAttribute), Array.Empty<IDeclarationUse<SyntaxNode>>(), It.IsAny<IDeclaration<SyntaxNode>>()))
                        .Returns(attributeDeclaration.Object);
                });

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.NotEmpty(classDeclaration.Members);

            Assert.NotEmpty(classDeclaration.Methods);

            var mDeclaration = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == methodName));
            var methodDeclaration = Assert.IsType<MethodDeclaration>(mDeclaration);

            if (returnAttribute)
            {
                methodDeclaration.ReturnAttributes.Should().NotBeNullOrEmpty();
                methodDeclaration.ReturnAttributes.Should().ContainSingle();
            }
            else
            {
                methodDeclaration.Attributes.Should().NotBeNullOrEmpty();
                methodDeclaration.Attributes.Should().ContainSingle();
            }
        }

        public void AssertMethodArgumentAttributesLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, string methodName, int argumentIndex)
            where TSyntaxNode : SyntaxNode
        {
            var declarationResolver = this.SetupDeclarationResolver(
                declaration, resolver =>
                {
                    var attributeDeclaration = new Mock<IGenericDeclaration<SyntaxNode>>();
                    attributeDeclaration.Setup(d => d.Name).Returns(nameof(DescriptionAttribute));

                    resolver
                        .Setup(r => r.Resolve(nameof(DescriptionAttribute), Array.Empty<IDeclarationUse<SyntaxNode>>(), It.IsAny<IDeclaration<SyntaxNode>>()))
                        .Returns(attributeDeclaration.Object);
                });

            declaration.DeepLoad(declarationResolver);

            var classDeclaration = Assert.IsAssignableFrom<IGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.NotEmpty(classDeclaration.Members);

            Assert.NotEmpty(classDeclaration.Methods);

            var mDeclaration = Assert.Single(classDeclaration.NamedMembers.Where(m => m.Name == methodName));
            var methodDeclaration = Assert.IsType<MethodDeclaration>(mDeclaration);

            methodDeclaration.Parameters.Count.Should().BeGreaterThan(argumentIndex);

            var methodParameter = methodDeclaration.Parameters.ElementAt(argumentIndex);
            methodParameter.Attributes.Should().NotBeNullOrEmpty();
            methodParameter.Attributes.Should().ContainSingle();
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
        }

        private IDeclarationResolver SetupDeclarationResolver(
            IDeclaration<SyntaxNode> contextDeclaration,
            params Type[] classes)
        {
            return SetupDeclarationResolver(contextDeclaration, (_) => { }, classes);
        }

        private IDeclarationResolver SetupDeclarationResolver(
            IDeclaration<SyntaxNode> contextDeclaration,
            Action<Mock<IDeclarationResolver>> setup,
            params Type[] classes)
        {
            var declarationResolverMock = new Mock<IDeclarationResolver>();
            foreach (var classItem in classes)
            {
                var className = classItem.Name;

                var classFile = new CSharpFile(
                    className.ToBasicClassesPath(),
                    DeclarationHelper.CreateParserDeclarationFactory(this.testOutputHelper),
                    Mock.Of<IGlobalUsingDirectives>());
                classFile.Load(Mock.Of<ICSharpWorkspace>());
                var classDeclarationSingle = Assert.Single(classFile.Declarations);
                if (classDeclarationSingle is IGenericDeclaration<SyntaxNode> genericDeclaration)
                {
                    if (genericDeclaration.TypeParameterListSyntaxProvider.SyntaxNode != null)
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
                        declarationResolverMock
                            .Setup(dr => dr.Resolve(classItem))
                            .Returns(genericDeclaration);
                    }
                }
                else
                {
                    declarationResolverMock
                            .Setup(dr => dr.Resolve(classDeclarationSingle.Name, contextDeclaration))
                            .Returns(classDeclarationSingle);
                }
            }

            setup(declarationResolverMock);

            return declarationResolverMock.Object;
        }
    }
}
