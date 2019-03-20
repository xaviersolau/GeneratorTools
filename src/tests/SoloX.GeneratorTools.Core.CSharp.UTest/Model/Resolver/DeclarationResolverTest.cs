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
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Resolver
{
    public class DeclarationResolverTest
    {
        [Fact]
        public void FindDeclarationTest()
        {
            var declaration = DeclarationHelper.SetupDeclaration<IDeclaration>("ns", "name");

            var declResolver = new DeclarationResolver(new[] { declaration }, null);

            var fundDecl = Assert.Single(declResolver.Find(declaration.FullName));
            Assert.Same(declaration, fundDecl);

            Assert.Null(declResolver.Find("unknown"));
        }

        [Theory]
        [InlineData("ctxNameSpace", "name", "ctxNameSpace", null, "name", true)]
        [InlineData("ctxNameSpace", "name", "ctxNameSpace", null, "ctxNameSpace.name", true)]
        [InlineData("a.b.c", "name", "ctxNameSpace", "a.b.c", "name", true)]
        [InlineData("a.b.c", "name", "ctxNameSpace", "a.b", "c.name", true)]
        [InlineData("a.b.c", "name", "ctxNameSpace", null, "a.b.c.name", true)]
        [InlineData("a.b.c", "name", "ctxNameSpace", null, "name", false)]
        [InlineData("a.b.c", "name", "ctxNameSpace", "a.b.c", "unknown", false)]
        [InlineData("ctxNameSpace", "name", "ctxNameSpace.a", null, "name", true)]
        public void ResolveDeclarationTest(
            string nameSpace, string name, string ctxNameSpace, string usingNameSpace, string nameToResolve, bool expectedMatch)
        {
            var declaration = DeclarationHelper.SetupDeclaration<IDeclaration>(nameSpace, name);

            var contextDeclaration = DeclarationHelper.SetupDeclaration<IDeclaration>(ctxNameSpace, "ctxName", m =>
            {
                var usingDirectives = string.IsNullOrEmpty(usingNameSpace) ? Array.Empty<string>() : new[] { usingNameSpace };
                m.SetupGet(d => d.UsingDirectives).Returns(usingDirectives);
            });

            var declResolver = new DeclarationResolver(new[] { declaration }, (r, d) => { });

            var resolvedDecl = declResolver.Resolve(nameToResolve, contextDeclaration);

            if (expectedMatch)
            {
                Assert.NotNull(resolvedDecl);
                Assert.Same(declaration, resolvedDecl);
            }
            else
            {
                Assert.Null(resolvedDecl);
            }
        }
    }
}