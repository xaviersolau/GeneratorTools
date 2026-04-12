// ----------------------------------------------------------------------
// <copyright file="InterfaceBasedOnSelectorTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using NSubstitute;
using Shouldly;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Selectors
{
    public class InterfaceBasedOnSelectorTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public InterfaceBasedOnSelectorTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(typeof(ISelectorTarget1), true)]
        [InlineData(typeof(ISelectorTarget2), true)]
        [InlineData(typeof(IOther), false)]
        public void ItShouldSelectTheRightInterface(Type type, bool selectionExpected)
        {
            var declarationFactory = DeclarationHelper.CreateReflectionDeclarationFactory(this.testOutputHelper);

            var declarationResolverMock = Substitute.For<IDeclarationResolver>();

            declarationResolverMock.Resolve(typeof(ITestSelector)).Returns((IGenericDeclaration<SyntaxNode>?)null);

            var declaration2 = declarationFactory
                .CreateDeclaration(type);

            declaration2.DeepLoad(declarationResolverMock);

            var fileMock = Substitute.For<ICSharpFile>();

            fileMock.Declarations.Returns(new[] { declaration2 });

            var selector = new InterfaceBasedOnSelector<ITestSelector>();

            var selected = selector.GetDeclarations(new[] { fileMock });
            if (selectionExpected)
            {
                var selectedItem = selected.ShouldHaveSingleItem();

                selectedItem.Name.ShouldBe(type.Name);
            }
            else
            {
                selected.ShouldBeEmpty();
            }
        }
    }

#pragma warning disable CA1040 // Avoid empty interfaces
    public interface ITestSelector
    {
    }
    public interface ISelectorTarget1 : ITestSelector
    {
    }
    public interface ISelectorTarget2 : ISelectorTarget1
    {
    }
    public interface IOther
    {
    }
#pragma warning restore CA1040 // Avoid empty interfaces
}
