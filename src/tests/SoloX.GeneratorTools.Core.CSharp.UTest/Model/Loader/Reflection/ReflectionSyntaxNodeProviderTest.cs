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

            var declaration = DeclarationFactory.CreateClassDeclaration(type);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);
            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.Load(declarationResolverMock.Object);

            Assert.NotNull(declaration.SyntaxNodeProvider);
            Assert.NotNull(declaration.SyntaxNodeProvider.SyntaxNode);
        }

        [Fact]
        public void SytaxNodeProviderClassWithPropertiesTest()
        {
            var type = typeof(ClassWithProperties);

            var declaration = DeclarationFactory.CreateClassDeclaration(type);

            var classDeclaration = Assert.IsType<ClassDeclaration>(declaration);
            var declarationResolverMock = new Mock<IDeclarationResolver>();
            classDeclaration.Load(declarationResolverMock.Object);

            foreach (var property in classDeclaration.Properties)
            {
                Assert.NotNull(property.SyntaxNodeProvider);
                Assert.NotNull(property.SyntaxNodeProvider.SyntaxNode);
                Assert.NotNull(property.PropertyType.SyntaxNodeProvider);
                Assert.NotNull(property.PropertyType.SyntaxNodeProvider.SyntaxNode);
            }
        }
    }
}
