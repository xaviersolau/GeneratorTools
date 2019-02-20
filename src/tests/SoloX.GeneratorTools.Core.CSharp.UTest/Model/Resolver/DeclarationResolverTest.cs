// ----------------------------------------------------------------------
// <copyright file="DeclarationResolverTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Resolver
{
    public class DeclarationResolverTest
    {
        [Fact]
        public void FindDeclarationTest()
        {
            var ns = "ns";
            var name = "name";
            var fullName = $"{ns}.{name}";

            var declarationMock = new Mock<IDeclaration>();
            declarationMock.SetupGet(d => d.Name).Returns(name);
            declarationMock.SetupGet(d => d.DeclarationNameSpace).Returns(ns);

            var declResolver = new DeclarationResolver(new[] { declarationMock.Object }, null);

            var fundDecl = Assert.Single(declResolver.Find(fullName));
            Assert.Same(declarationMock.Object, fundDecl);

            Assert.Null(declResolver.Find("unknown"));
        }
    }
}