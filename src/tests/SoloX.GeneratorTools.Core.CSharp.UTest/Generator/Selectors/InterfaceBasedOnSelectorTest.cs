// ----------------------------------------------------------------------
// <copyright file="InterfaceBasedOnSelectorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using System;
using Xunit;
using Xunit.Abstractions;

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
            var declarationFactory = DeclarationHelper.CreateDeclarationFactory(this.testOutputHelper);

            var declarationResolverMock = new Mock<IDeclarationResolver>();

            var declaration2 = declarationFactory
                .CreateInterfaceDeclaration(type);

            declaration2.DeepLoad(declarationResolverMock.Object);

            var fileMock = new Mock<ICSharpFile>();

            fileMock.Setup(file => file.Declarations).Returns(new[] { declaration2 });

            var selector = new InterfaceBasedOnSelector<ITestSelector>();

            var selected = selector.GetDeclarations(new[] { fileMock.Object });

            if (selectionExpected)
            {
                var selectedItem = selected.Should().ContainSingle().Subject;

                selectedItem.Name.Should().Be(type.Name);
            }
            else
            {
                selected.Should().BeEmpty();
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
