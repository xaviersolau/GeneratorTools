// ----------------------------------------------------------------------
// <copyright file="DeclarationHelper.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using SoloX.CodeQuality.Test.Helpers.Logger;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
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
        public static IDeclarationFactory CreateDeclarationFactory(ITestOutputHelper testOutputHelper)
        {
            return new DeclarationFactory(
                new ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>(
                    new TestLogger<ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>>(testOutputHelper)),
                new ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>(
                    new TestLogger<ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>>(testOutputHelper)),
                new ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>(),
                new ParserGenericDeclarationLoader<ClassDeclarationSyntax>(),
                new ParserGenericDeclarationLoader<StructDeclarationSyntax>());
        }
    }
}
