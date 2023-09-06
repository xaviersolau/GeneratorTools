// ----------------------------------------------------------------------
// <copyright file="ConstantExpressionSyntaxEvaluatorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void SimpleIntegerConstEvaluatorAsObjectTest()
        {
            var resolver = Mock.Of<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var textValue = "123";
            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolver, genericDeclaration);
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
        [InlineData("new string[]")]
        [InlineData("new String[]")]
        [InlineData("new []")]
        public void StringArrayConstEvaluatorWithTypeProbeTest(string arrayDecl)
        {
            var resolver = Mock.Of<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var stringDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();
            Mock.Get(stringDeclaration).SetupGet(d => d.FullName).Returns(typeof(string).FullName);

            Mock.Get(resolver).Setup(r => r.Resolve("String", genericDeclaration)).Returns(stringDeclaration);

            var textValue1 = "textValue1";
            var textValue2 = "textValue2";
            var textExp = $@"{arrayDecl} {{ ""{textValue1}"", ""{textValue2}"" }}";

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExp);

            var arrayObject = walker.Visit(exp);

            Assert.NotNull(arrayObject);

            var array = arrayObject.Should().BeAssignableTo<IEnumerable<string>>().Subject;

            Assert.Equal(2, array.Count());
            Assert.Equal(textValue1, array.ElementAt(0));
            Assert.Equal(textValue2, array.ElementAt(1));
        }

        [Theory]
        [InlineData("new int[]")]
        [InlineData("new []")]
        public void IntArrayConstEvaluatorWithTypeProbeTest(string arrayDecl)
        {
            var resolver = Mock.Of<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var intValue1 = 10;
            var intValue2 = 20;
            var textExp = $@"{arrayDecl} {{ {intValue1}, {intValue2} }}";

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExp);

            var arrayObject = walker.Visit(exp);

            Assert.NotNull(arrayObject);

            var array = arrayObject.Should().BeAssignableTo<IEnumerable<int>>().Subject;

            Assert.Equal(2, array.Count());
            Assert.Equal(intValue1, array.ElementAt(0));
            Assert.Equal(intValue2, array.ElementAt(1));
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

        [Theory]
        [InlineData("$\"Name of is {nameof(Arg1)}\"", "Name of is Arg1")]
        public void InterpolationEvaluatorTest(string textExpression, string expectedValue)
        {
            var resolverMock = new Mock<IDeclarationResolver>();
            var genericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolverMock.Object, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExpression);

            var value = walker.Visit(exp);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("ConstField", 123)]
        public void ConstFieldEvaluatorTest(string textExpression, object expectedValue)
        {
            var resolverMock = new Mock<IDeclarationResolver>();
            var genericDeclarationMock = MockGenericDeclarationWithConstant(textExpression, expectedValue);

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolverMock.Object, genericDeclarationMock.Object);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExpression);

            var value = walker.Visit(exp);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("ClassName", "ConstField", 123)]
        public void ExternalClassConstFieldEvaluatorTest(string classExpression, string textExpression, object expectedValue)
        {
            var resolverMock = new Mock<IDeclarationResolver>();

            var genericDeclarationMock = MockGenericDeclarationWithConstant(textExpression, expectedValue);

            var currentGenericDeclaration = Mock.Of<IGenericDeclaration<SyntaxNode>>();

            resolverMock.Setup(r => r.Resolve(classExpression, currentGenericDeclaration)).Returns(genericDeclarationMock.Object);

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolverMock.Object, currentGenericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($"{classExpression}.{textExpression}");

            var value = walker.Visit(exp);

            Assert.Equal(expectedValue, value);
        }

        private static Mock<IGenericDeclaration<SyntaxNode>> MockGenericDeclarationWithConstant(string constantName, object constantValue)
        {
            var genericDeclarationMock = new Mock<IGenericDeclaration<SyntaxNode>>();
            var constantDeclarationMock = new Mock<IConstantDeclaration>();
            var constantDeclarationSyntaxProviderMock = new Mock<ISyntaxNodeProvider<VariableDeclaratorSyntax>>();

            constantDeclarationMock.SetupGet(c => c.Name).Returns(constantName);
            constantDeclarationMock.SetupGet(c => c.SyntaxNodeProvider).Returns(constantDeclarationSyntaxProviderMock.Object);

            var constExp = SyntaxTreeHelper.GetFieldSyntax("object", $"{constantName} = {constantValue}");

            constantDeclarationSyntaxProviderMock.SetupGet(p => p.SyntaxNode).Returns(constExp.Declaration.Variables.First());

            var constants = new IConstantDeclaration[] { constantDeclarationMock.Object };

            genericDeclarationMock.SetupGet(d => d.Constants).Returns(constants);
            return genericDeclarationMock;
        }
    }
}
