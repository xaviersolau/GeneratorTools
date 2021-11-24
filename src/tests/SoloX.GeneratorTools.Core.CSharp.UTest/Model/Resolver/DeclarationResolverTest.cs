// ----------------------------------------------------------------------
// <copyright file="DeclarationResolverTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Resolver
{
    public class DeclarationResolverTest
    {
        [Fact]
        public void FindDeclarationTest()
        {
            var declaration = DeclarationHelper.SetupDeclaration<IDeclaration<SyntaxNode>>("ns", "name");

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
            var declaration = DeclarationHelper.SetupDeclaration<IDeclaration<SyntaxNode>>(nameSpace, name);

            var contextDeclaration = DeclarationHelper.SetupDeclaration<IDeclaration<SyntaxNode>>(ctxNameSpace, "ctxName", m =>
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

        [Theory]
        [InlineData("ctxNameSpace", "name", "paramName", "ctxNameSpace", null, "name", true)]
        [InlineData("ctxNameSpace", "name", "paramName", "ctxNameSpace", null, "ctxNameSpace.name", true)]
        [InlineData("a.b.c", "name", "paramName", "ctxNameSpace", "a.b.c", "name", true)]
        [InlineData("a.b.c", "name", "paramName", "ctxNameSpace", "a.b", "c.name", true)]
        [InlineData("a.b.c", "name", "paramName", "ctxNameSpace", null, "a.b.c.name", true)]
        [InlineData("a.b.c", "name", "paramName", "ctxNameSpace", null, "name", false)]
        [InlineData("a.b.c", "name", "paramName", "ctxNameSpace", "a.b.c", "unknown", false)]
        [InlineData("ctxNameSpace", "name", "paramName", "ctxNameSpace.a", null, "name", true)]
        public void ResolveGenericDeclarationTest(
            string nameSpace, string name, string nameParam, string ctxNameSpace, string usingNameSpace, string nameToResolve, bool expectedMatch)
        {
            var genericParameter = Mock.Of<IGenericParameterDeclaration>();
            var genericDeclaration = DeclarationHelper.SetupDeclaration<IGenericDeclaration<SyntaxNode>>(nameSpace, name, mock =>
            {
                mock.SetupGet(x => x.GenericParameters).Returns(new[] { genericParameter });
            });

            var contextDeclaration = DeclarationHelper.SetupDeclaration<IDeclaration<SyntaxNode>>(ctxNameSpace, "ctxName", m =>
            {
                var usingDirectives = string.IsNullOrEmpty(usingNameSpace) ? Array.Empty<string>() : new[] { usingNameSpace };
                m.SetupGet(d => d.UsingDirectives).Returns(usingDirectives);
            });

            var declarationParam = DeclarationHelper.SetupDeclaration<IDeclaration<SyntaxNode>>(nameSpace, nameParam);
            var declarationParamUse1 = DeclarationHelper.SetupDeclarationUse<IDeclarationUse<SyntaxNode>>(declarationParam);
            var declarationParamUse2 = DeclarationHelper.SetupDeclarationUse<IDeclarationUse<SyntaxNode>>(declarationParam);

            var declResolver = new DeclarationResolver(new[] { genericDeclaration }, (r, d) => { });

            var resolvedGenericDecl1 = declResolver.Resolve(
                nameToResolve,
                new[] { declarationParamUse1 },
                contextDeclaration);
            var resolvedGenericDecl2 = declResolver.Resolve(
                nameToResolve,
                new[] { declarationParamUse1, declarationParamUse2 },
                contextDeclaration);

            var resolvedDecl = declResolver.Resolve(nameToResolve, contextDeclaration);

            Assert.Null(resolvedDecl);
            Assert.Null(resolvedGenericDecl2);

            if (expectedMatch)
            {
                Assert.NotNull(resolvedGenericDecl1);
                Assert.Same(genericDeclaration, resolvedGenericDecl1);
            }
            else
            {
                Assert.Null(resolvedGenericDecl1);
            }
        }
    }
}