// ----------------------------------------------------------------------
// <copyright file="LoadingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using Xunit;
using System;
using System.Reflection;
using System.Linq;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model.Loader.Common
{
    public static class LoadingTest
    {
        public static void AssertGenericTypeLoaded<TSyntaxNode>(IDeclaration<TSyntaxNode> declaration, Type type, Type? baseType)
            where TSyntaxNode : SyntaxNode
        {
            Assert.Equal(
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                declaration.Name);

            Assert.NotNull(declaration.SyntaxNodeProvider);
            Assert.NotNull(declaration.SyntaxNodeProvider.SyntaxNode);

            var classDeclaration = Assert.IsAssignableFrom<AGenericDeclaration<TSyntaxNode>>(declaration);

            Assert.Null(classDeclaration.GenericParameters);
            Assert.Null(classDeclaration.Extends);
            Assert.Null(classDeclaration.Members);

            classDeclaration.DeepLoad(Mock.Of<IDeclarationResolver>());

            Assert.NotNull(classDeclaration.GenericParameters);
            Assert.NotNull(classDeclaration.Extends);
            Assert.NotNull(classDeclaration.Members);

            if (type.IsGenericTypeDefinition)
            {
                Assert.NotEmpty(classDeclaration.GenericParameters);

                var typeParams = type.GetTypeInfo().GenericTypeParameters;
                Assert.Equal(typeParams.Length, classDeclaration.GenericParameters.Count);

                Assert.Equal(typeParams[0].Name, classDeclaration.GenericParameters.First().Name);
                Assert.Equal(typeParams[0].IsValueType, classDeclaration.GenericParameters.First().IsValueType);
            }
            else
            {
                Assert.Empty(classDeclaration.GenericParameters);
            }

            if (baseType != null)
            {
                var baseClassName = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(baseType.Name);

                var extendNames = classDeclaration.Extends.Select(x => x.Declaration.Name);

                Assert.Single(extendNames.Where(x => x == baseClassName));
            }
        }
    }
}
