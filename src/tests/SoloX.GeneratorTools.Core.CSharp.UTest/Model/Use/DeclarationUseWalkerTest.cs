// ----------------------------------------------------------------------
// <copyright file="DeclarationUseWalkerTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Use
{
    public class DeclarationUseWalkerTest
    {
        [Fact]
        public void GenericParameterDeclarationUseLoadingTest()
        {
            var declarationUseStatement = "TName";
            IGenericParameterDeclaration genericParameterDeclaration = null;

            var walker = SetupDeclarationUseWalker(ctx =>
            {
                genericParameterDeclaration = SetupGenericParameterDeclaration(ctx, declarationUseStatement);
            });

            var node = SyntaxTreeHelper.GetTypeSyntax(declarationUseStatement);

            var declarationUse = walker.Visit(node);

            Assert.NotNull(declarationUse);
            Assert.IsType<GenericParameterDeclarationUse>(declarationUse);
            Assert.Same(genericParameterDeclaration, declarationUse.Declaration);
        }

        [Fact]
        public void UnknownDeclarationUseLoadingTest()
        {
            var walker = SetupDeclarationUseWalker();

            var node = SyntaxTreeHelper.GetTypeSyntax("Unknown");

            var declarationUse = walker.Visit(node);

            Assert.NotNull(declarationUse);
            Assert.IsType<UnknownDeclarationUse>(declarationUse);
            Assert.NotNull(declarationUse.Declaration);
            Assert.IsType<UnknownDeclaration>(declarationUse.Declaration);
        }

        [Theory]
        [InlineData("AName", "[]", 1)]
        [InlineData("AName", "[][]", 2)]
        public void ArrayDeclarationUseLoadingTest(string declarationName, string arrayText, int dimCount)
        {
            var declaration = Mock.Of<IClassDeclaration>();

            var walker = SetupDeclarationUseWalker(resolverSetup: resolver =>
            {
                resolver
                    .Setup(r => r.Resolve(declarationName, It.IsAny<IDeclaration>()))
                    .Returns(declaration);
            });

            var node = SyntaxTreeHelper.GetTypeSyntax($"{declarationName}{arrayText}");

            var declarationUse = walker.Visit(node);

            Assert.NotNull(declarationUse);
            Assert.Same(declaration, declarationUse.Declaration);

            Assert.NotNull(declarationUse.ArraySpecification);
            Assert.Equal(dimCount, declarationUse.ArraySpecification.SyntaxNode.Count);
        }

        [Theory]
        [InlineData("AName", "", 0)]
        [InlineData("AName", "<int>", 1)]
        [InlineData("AName", "<int, double>", 2)]
        public void GenericDeclarationUseLoadingTest(string declarationName, string genericParametersText, int genericParams)
        {
            var declarationMock = new Mock<IClassDeclaration>();

            var walker = SetupDeclarationUseWalker(resolverSetup: resolver =>
            {
                if (genericParams == 0)
                {
                    resolver
                        .Setup(r => r.Resolve(declarationName, It.IsAny<IDeclaration>()))
                        .Returns(declarationMock.Object);
                    resolver
                        .Setup(r => r.Resolve(declarationName, Array.Empty<IDeclarationUse>(), It.IsAny<IDeclaration>()))
                        .Returns(declarationMock.Object);
                }
                else
                {
                    resolver
                        .Setup(r => r.Resolve(declarationName, It.IsAny<IReadOnlyList<IDeclarationUse>>(), It.IsAny<IDeclaration>()))
                        .Returns(declarationMock.Object);
                }
            });

            var node = SyntaxTreeHelper.GetTypeSyntax($"{declarationName}{genericParametersText}");

            var declarationUse = walker.Visit(node);

            Assert.NotNull(declarationUse);
            Assert.Same(declarationMock.Object, declarationUse.Declaration);

            var genericDeclarationUse = Assert.IsType<GenericDeclarationUse>(declarationUse);
            Assert.NotNull(genericDeclarationUse.GenericParameters);
            Assert.Equal(genericParams, genericDeclarationUse.GenericParameters.Count);

            foreach (var genericParameter in genericDeclarationUse.GenericParameters)
            {
                Assert.IsType<PredefinedDeclarationUse>(genericParameter);
            }
        }

        private static DeclarationUseWalker SetupDeclarationUseWalker(
            Action<Mock<IGenericDeclaration>> contextSetup = null,
            Action<Mock<IDeclarationResolver>> resolverSetup = null)
        {
            var resolverMock = new Mock<IDeclarationResolver>();
            resolverSetup?.Invoke(resolverMock);

            var declarationContextMock = new Mock<IGenericDeclaration>();
            if (contextSetup != null)
            {
                contextSetup(declarationContextMock);
            }
            else
            {
                SetupGenericParameterDeclaration(declarationContextMock, null);
            }

            return new DeclarationUseWalker(resolverMock.Object, declarationContextMock.Object);
        }

        /// <summary>
        /// Setup a IGenericParameterDeclaration on the given generic declaration mock.
        /// </summary>
        private static IGenericParameterDeclaration SetupGenericParameterDeclaration(Mock<IGenericDeclaration> genericDeclarationMock, string parameterName)
        {
            if (parameterName != null)
            {
                var genericParameterDeclarationMock = new Mock<IGenericParameterDeclaration>();
                genericParameterDeclarationMock
                    .SetupGet(d => d.Name)
                    .Returns(parameterName);

                genericDeclarationMock
                    .SetupGet(d => d.GenericParameters)
                    .Returns(new[] { genericParameterDeclarationMock.Object });
                return genericParameterDeclarationMock.Object;
            }

            genericDeclarationMock
                .SetupGet(d => d.GenericParameters)
                .Returns(Array.Empty<IGenericParameterDeclaration>());

            return null;
        }
    }
}
