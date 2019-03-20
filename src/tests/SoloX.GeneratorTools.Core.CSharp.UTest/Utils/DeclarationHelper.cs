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
    }
}
