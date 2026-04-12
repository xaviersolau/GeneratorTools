// ----------------------------------------------------------------------
// <copyright file="DeclarationUseWalkerTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using NSubstitute;
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
            var declaration = Substitute.For<IClassDeclaration>();

            var walker = SetupDeclarationUseWalker(resolverSetup: resolver =>
            {
                resolver
                    .Resolve(declarationName, Arg.Any<IDeclaration<SyntaxNode>>())
                    .Returns(declaration);
            });

            var node = SyntaxTreeHelper.GetTypeSyntax($"{declarationName}{arrayText}");

            var declarationUse = walker.Visit(node);

            Assert.NotNull(declarationUse);
            Assert.Same(declaration, declarationUse.Declaration);

            Assert.NotNull(declarationUse.ArraySpecification);
            Assert.NotNull(declarationUse.ArraySpecification.SyntaxNodeProvider);

            Assert.Equal(dimCount, declarationUse.ArraySpecification.ArrayCount);
        }

        [Theory]
        [InlineData("AName", "", 0)]
        [InlineData("AName", "<int>", 1)]
        [InlineData("AName", "<int, double>", 2)]
        public void GenericDeclarationUseLoadingTest(string declarationName, string genericParametersText, int genericParams)
        {
            var declarationMock = Substitute.For<IClassDeclaration>();

            var walker = SetupDeclarationUseWalker(resolverSetup: resolver =>
            {
                if (genericParams == 0)
                {
                    resolver
                        .Resolve(declarationName, Arg.Any<IDeclaration<SyntaxNode>>())
                        .Returns(declarationMock);
                    resolver
                        .Resolve(declarationName, Array.Empty<IDeclarationUse<SyntaxNode>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                        .Returns(declarationMock);
                }
                else
                {
                    resolver
                        .Resolve(declarationName, Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                        .Returns(declarationMock);
                }
            });

            var node = SyntaxTreeHelper.GetTypeSyntax($"{declarationName}{genericParametersText}");

            var declarationUse = walker.Visit(node);

            Assert.NotNull(declarationUse);
            Assert.Same(declarationMock, declarationUse.Declaration);

            var genericDeclarationUse = Assert.IsType<GenericDeclarationUse>(declarationUse);
            Assert.NotNull(genericDeclarationUse.GenericParameters);
            Assert.Equal(genericParams, genericDeclarationUse.GenericParameters.Count);

            foreach (var genericParameter in genericDeclarationUse.GenericParameters)
            {
                Assert.IsType<PredefinedDeclarationUse>(genericParameter);
            }
        }

        private static DeclarationUseWalker SetupDeclarationUseWalker(
            Action<IGenericDeclaration<SyntaxNode>> contextSetup = null,
            Action<IDeclarationResolver> resolverSetup = null)
        {
            var resolverMock = Substitute.For<IDeclarationResolver>();

            resolverMock
                .Resolve(Arg.Any<string>(), Arg.Any<IReadOnlyList<IDeclarationUse<SyntaxNode>>>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns((IGenericDeclaration<SyntaxNode>?)null);

            resolverMock
                .Resolve(Arg.Any<string>(), Arg.Any<IDeclaration<SyntaxNode>>())
                .Returns((IDeclaration<SyntaxNode>?)null);

            resolverMock
                .Resolve(Arg.Any<Type>())
                .Returns((IGenericDeclaration<SyntaxNode>?)null);

            resolverSetup?.Invoke(resolverMock);

            var declarationContextMock = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            if (contextSetup != null)
            {
                contextSetup(declarationContextMock);
            }
            else
            {
                SetupGenericParameterDeclaration(declarationContextMock, null);
            }

            return new DeclarationUseWalker(resolverMock, declarationContextMock);
        }

        /// <summary>
        /// Setup a IGenericParameterDeclaration on the given generic declaration mock.
        /// </summary>
        private static IGenericParameterDeclaration? SetupGenericParameterDeclaration(IGenericDeclaration<SyntaxNode> genericDeclarationMock, string parameterName)
        {
            if (parameterName != null)
            {
                var genericParameterDeclarationMock = Substitute.For<IGenericParameterDeclaration>();
                genericParameterDeclarationMock
                    .Name
                    .Returns(parameterName);

                genericDeclarationMock
                    .GenericParameters
                    .Returns(new[] { genericParameterDeclarationMock });
                return genericParameterDeclarationMock;
            }

            genericDeclarationMock
                .GenericParameters
                .Returns(Array.Empty<IGenericParameterDeclaration>());

            return null;
        }
    }
}
