// ----------------------------------------------------------------------
// <copyright file="DeclarationHelper.cs" company="SoloX Software">
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
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

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
            where TDeclaration : class, IDeclaration
        {
            var declarationMock = new Mock<TDeclaration>();
            declarationMock.SetupGet(d => d.Name).Returns(name);
            declarationMock.SetupGet(d => d.DeclarationNameSpace).Returns(nameSpace);
            declarationMock.SetupGet(d => d.FullName).Returns(ADeclaration.GetFullName(nameSpace, name));

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
        public static TDeclarationUse SetupDeclarationUse<TDeclarationUse>(IDeclaration declaration, string typeSourceCode = null, Action<Mock<TDeclarationUse>> setup = null)
            where TDeclarationUse : class, IDeclarationUse
        {
            var propertyTypeMock = new Mock<TDeclarationUse>();
            propertyTypeMock.Setup(t => t.Declaration).Returns(declaration);
            if (typeSourceCode != null)
            {
                var typeNode = SyntaxTreeHelper.GetTypeSyntax(typeSourceCode);
                propertyTypeMock.Setup(t => t.SyntaxNode).Returns(typeNode);
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
            var propertyType = DeclarationHelper.SetupDeclarationUse<IDeclarationUse>(Mock.Of<IDeclaration>(), type);

            var itfDeclPropMock = new Mock<IPropertyDeclaration>();
            itfDeclPropMock.SetupGet(d => d.Name).Returns(name);
            itfDeclPropMock.SetupGet(d => d.PropertyType).Returns(propertyType);

            return itfDeclPropMock.Object;
        }
    }
}
