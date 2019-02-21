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
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Use
{
    public class DeclarationUseWalkerTest
    {
        [Theory]
        [InlineData("TName")]
        public void GenericParameterDeclarationUseLoadingTest(string declarationUseStatement)
        {
            var resolverMock = new Mock<IDeclarationResolver>();
            var declarationContextMock = new Mock<IGenericDeclaration>();

            var genericParameterDeclarationMock = new Mock<IGenericParameterDeclaration>();
            genericParameterDeclarationMock
                .SetupGet(d => d.Name)
                .Returns(declarationUseStatement);

            declarationContextMock
                .SetupGet(d => d.GenericParameters)
                .Returns(new[] { genericParameterDeclarationMock.Object });

            var walker = new DeclarationUseWalker(resolverMock.Object, declarationContextMock.Object);

            var node = SyntaxTreeHelper.GetTypeSyntax(declarationUseStatement);

            var declarationUse = walker.Visit(node);

            Assert.NotNull(declarationUse);
            var gpdu = Assert.IsType<GenericParameterDeclarationUse>(declarationUse);
            Assert.Same(genericParameterDeclarationMock.Object, gpdu.Declaration);
        }
    }
}
