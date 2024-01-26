// ----------------------------------------------------------------------
// <copyright file="DeclarationHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.Test.Helpers;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Utils
{
    /// <summary>
    /// Helper to generate declaration for tests.
    /// </summary>
    public static class DeclarationHelper
    {
        /// <summary>
        /// Generate an declaration mock form the given parameters.
        /// </summary>
        /// <param name="nameSpace">The declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <param name="setup">The action delegate to do additional setup on the mock.</param>
        /// <returns>The mocked declaration.</returns>
        public static TDeclaration SetupDeclaration<TDeclaration>(string nameSpace, string name, Action<Mock<TDeclaration>> setup = null)
            where TDeclaration : class, IDeclaration<SyntaxNode>
        {
            var declarationMock = new Mock<TDeclaration>();
            declarationMock.SetupGet(d => d.Name).Returns(name);
            declarationMock.SetupGet(d => d.DeclarationNameSpace).Returns(nameSpace);
            declarationMock.SetupGet(d => d.FullName).Returns(ADeclaration<SyntaxNode>.GetFullName(nameSpace, name));

            setup?.Invoke(declarationMock);

            return declarationMock.Object;
        }

        /// <summary>
        /// Setup using directives.
        /// </summary>
        /// <returns></returns>
        public static IUsingDirectives SetupUsingDirectives(IReadOnlyList<string> usings)
        {
            var usingDirectivesMock = new Mock<IUsingDirectives>();

            usingDirectivesMock.SetupGet(u => u.Usings).Returns(usings);

            return usingDirectivesMock.Object;
        }

        /// <summary>
        /// Generate a declaration use.
        /// </summary>
        /// <param name="declaration">The used declaration.</param>
        /// <param name="typeSourceCode">The type source code.</param>
        /// <param name="setup">The action delegate to do additional setup on the mock.</param>
        /// <returns>The mocked declaration use.</returns>
        public static TDeclarationUse SetupDeclarationUse<TDeclarationUse>(IDeclaration<SyntaxNode> declaration, string typeSourceCode = null, Action<Mock<TDeclarationUse>> setup = null)
            where TDeclarationUse : class, IDeclarationUse<SyntaxNode>
        {
            var propertyTypeMock = new Mock<TDeclarationUse>();
            propertyTypeMock.Setup(t => t.Declaration).Returns(declaration);
            if (typeSourceCode != null)
            {
                var typeNode = SyntaxTreeHelper.GetTypeSyntax(typeSourceCode);
                var nodeProvider = SyntaxTreeHelper.GetSyntaxNodeProvider(typeNode);
                propertyTypeMock.Setup(t => t.SyntaxNodeProvider).Returns(nodeProvider);
            }

            setup?.Invoke(propertyTypeMock);

            return propertyTypeMock.Object;
        }

        /// <summary>
        /// Generate a Property declaration.
        /// </summary>
        /// <param name="type">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>The mocked property declaration.</returns>
        public static IPropertyDeclaration SetupPropertyDeclaration(string type, string name)
        {
            var propertyType = DeclarationHelper.SetupDeclarationUse<IDeclarationUse<SyntaxNode>>(Mock.Of<IDeclaration<SyntaxNode>>(), type);

            var itfDeclPropMock = new Mock<IPropertyDeclaration>();
            itfDeclPropMock.SetupGet(d => d.Name).Returns(name);
            itfDeclPropMock.SetupGet(d => d.PropertyType).Returns(propertyType);

            return itfDeclPropMock.Object;
        }

        /// <summary>
        /// Create a Declaration factory setup with the test output logger.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <returns>The created declaration factory.</returns>
        public static IParserDeclarationFactory CreateParserDeclarationFactory(ITestOutputHelper testOutputHelper)
        {
            return new ParserDeclarationFactory(
                new ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>>(testOutputHelper)),
                new ParserGenericDeclarationLoader<ClassDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<ParserGenericDeclarationLoader<ClassDeclarationSyntax>>(testOutputHelper)),
                new ParserGenericDeclarationLoader<StructDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<ParserGenericDeclarationLoader<StructDeclarationSyntax>>(testOutputHelper)),
                new ParserGenericDeclarationLoader<RecordDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<ParserGenericDeclarationLoader<RecordDeclarationSyntax>>(testOutputHelper)),
                new ParserEnumDeclarationLoader(LoggerHelper.CreateGeneratorLogger<ParserEnumDeclarationLoader>(testOutputHelper)));
        }

        /// <summary>
        /// Create a Declaration factory setup with the test output logger.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <returns>The created declaration factory.</returns>
        public static IMetadataDeclarationFactory CreateMetadataDeclarationFactory(ITestOutputHelper testOutputHelper)
        {
            return new MetadataDeclarationFactory(
                new MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax>>(testOutputHelper)),
                new MetadataGenericDeclarationLoader<ClassDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<MetadataGenericDeclarationLoader<ClassDeclarationSyntax>>(testOutputHelper)),
                new MetadataGenericDeclarationLoader<StructDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<MetadataGenericDeclarationLoader<StructDeclarationSyntax>>(testOutputHelper)),
                new MetadataGenericDeclarationLoader<RecordDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<MetadataGenericDeclarationLoader<RecordDeclarationSyntax>>(testOutputHelper)),
                new MetadataEnumDeclarationLoader(LoggerHelper.CreateGeneratorLogger<MetadataEnumDeclarationLoader>(testOutputHelper)));
        }

        /// <summary>
        /// Create a Declaration factory setup with the test output logger.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <returns>The created declaration factory.</returns>
        public static IReflectionDeclarationFactory CreateReflectionDeclarationFactory(ITestOutputHelper testOutputHelper)
        {
            return new ReflectionDeclarationFactory(
                new ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>>(testOutputHelper)),
                new ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>>(testOutputHelper)),
                new ReflectionGenericDeclarationLoader<StructDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<ReflectionGenericDeclarationLoader<StructDeclarationSyntax>>(testOutputHelper)),
                new ReflectionGenericDeclarationLoader<RecordDeclarationSyntax>(
                    LoggerHelper.CreateGeneratorLogger<ReflectionGenericDeclarationLoader<RecordDeclarationSyntax>>(testOutputHelper)),
                new ReflectionEnumDeclarationLoader(LoggerHelper.CreateGeneratorLogger<ReflectionEnumDeclarationLoader>(testOutputHelper)));
        }
    }
}
