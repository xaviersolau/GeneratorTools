// ----------------------------------------------------------------------
// <copyright file="ConstantExpressionSyntaxEvaluatorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Walker
{
    public class ConstantExpressionSyntaxEvaluatorTest
    {
        [Fact]
        public void SimpleStringConstEvaluatorTest()
        {
            var resolver = Mock.Of<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var textValue = "MyValue";
            var walker = new ConstantExpressionSyntaxEvaluator<string>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($@"""{textValue}""");

            var value = walker.Visit(exp);

            Assert.Equal(textValue, value);
        }

        [Fact]
        public void SimpleIntegerConstEvaluatorTest()
        {
            var resolver = Mock.Of<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var textValue = "123";
            var walker = new ConstantExpressionSyntaxEvaluator<int>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($@"{textValue}");

            var value = walker.Visit(exp);

            Assert.Equal(123, value);
        }

        [Theory]
        [InlineData("new string[]")]
        [InlineData("new []")]
        public void StringArrayConstEvaluatorTest(string arrayDecl)
        {
            var resolver = Mock.Of<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var textValue1 = "textValue1";
            var textValue2 = "textValue2";
            var textExp = $@"{arrayDecl} {{ ""{textValue1}"", ""{textValue2}"" }}";

            var walker = new ConstantExpressionSyntaxEvaluator<string[]>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExp);

            var array = walker.Visit(exp);

            Assert.NotNull(array);

            Assert.Equal(2, array.Length);
            Assert.Equal(textValue1, array[0]);
            Assert.Equal(textValue2, array[1]);
        }

        [Theory]
        [InlineData("MyValue", "MyValue")]
        [InlineData("SomeClass.MyValue", "MyValue")]
        public void NameOfConstEvaluatorTest(string textExpression, string expectedValue)
        {
            var resolver = Mock.Of<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var walker = new ConstantExpressionSyntaxEvaluator<string>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($"nameof({textExpression})");

            var value = walker.Visit(exp);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("object")]
        [InlineData("string")]
        public void TypeOfConstEvaluatorTest(string textExpression)
        {
            var resolverMock = new Mock<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolverMock.Object, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($"typeof({textExpression})");

            var value = walker.Visit(exp);

            var typeDeclarationUse = value.Should().BeAssignableTo<IDeclarationUse<SyntaxNode>>().Subject;
            typeDeclarationUse.Declaration.Name.Should().Be(textExpression);
        }
    }
}
