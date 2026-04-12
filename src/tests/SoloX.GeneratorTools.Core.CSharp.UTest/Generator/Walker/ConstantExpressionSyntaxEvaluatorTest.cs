// ----------------------------------------------------------------------
// <copyright file="ConstantExpressionSyntaxEvaluatorTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Shouldly;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using System.Collections.Generic;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Walker
{
    public class ConstantExpressionSyntaxEvaluatorTest
    {
        [Fact]
        public void SimpleStringConstEvaluatorTest()
        {
            var resolver = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            var textValue = "MyValue";
            var walker = new ConstantExpressionSyntaxEvaluator<string>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($@"""{textValue}""");

            var value = walker.Visit(exp);

            Assert.Equal(textValue, value);
        }

        [Fact]
        public void SimpleIntegerConstEvaluatorTest()
        {
            var resolver = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            var textValue = "123";
            var walker = new ConstantExpressionSyntaxEvaluator<int>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($@"{textValue}");

            var value = walker.Visit(exp);

            Assert.Equal(123, value);
        }

        [Fact]
        public void SimpleIntegerConstEvaluatorAsObjectTest()
        {
            var resolver = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

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
            var resolver = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            var textValue1 = "textValue1";
            var textValue2 = "textValue2";
            var textExp = $@"{arrayDecl} {{ ""{textValue1}"", ""{textValue2}"" }}";

            var walker = new ConstantExpressionSyntaxEvaluator<string[]>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExp);

            var array = walker.Visit(exp);

            array.ShouldNotBeNull();

            array.ShouldBe([textValue1, textValue2], ignoreOrder: true);
        }

        [Theory]
        [InlineData("new string[]")]
        [InlineData("new String[]")]
        [InlineData("new []")]
        public void StringArrayConstEvaluatorWithTypeProbeTest(string arrayDecl)
        {
            var resolver = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            var stringDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            stringDeclaration.FullName.Returns(typeof(string).FullName);

            resolver.Resolve("String", genericDeclaration).Returns(stringDeclaration);

            var textValue1 = "textValue1";
            var textValue2 = "textValue2";
            var textExp = $@"{arrayDecl} {{ ""{textValue1}"", ""{textValue2}"" }}";

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExp);

            var arrayObject = walker.Visit(exp);

            arrayObject.ShouldNotBeNull();

            var array = arrayObject.ShouldBeAssignableTo<IEnumerable<string>>();

            array.ShouldBe([textValue1, textValue2], ignoreOrder: true);
        }

        [Fact]
        public void StringArrayConstEvaluatorWithTypeProbeTestWithoutNewStatement()
        {
            var resolver = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            var textValue1 = "textValue1";
            var textValue2 = "textValue2";
            var textExp = $@"[ ""{textValue1}"", ""{textValue2}"" ]";

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExp);

            var arrayObject = walker.Visit(exp);

            arrayObject.ShouldNotBeNull();

            var array = arrayObject.ShouldBeAssignableTo<IEnumerable<string>>();

            array.ShouldBe([textValue1, textValue2], ignoreOrder: true);
        }

        [Theory]
        [InlineData("new int[]")]
        [InlineData("new []")]
        public void IntArrayConstEvaluatorWithTypeProbeTest(string arrayDecl)
        {
            var resolver = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            var intValue1 = 10;
            var intValue2 = 20;
            var textExp = $@"{arrayDecl} {{ {intValue1}, {intValue2} }}";

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolver, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExp);

            var arrayObject = walker.Visit(exp);

            arrayObject.ShouldNotBeNull();

            var array = arrayObject.ShouldBeAssignableTo<IEnumerable<int>>();

            array.ShouldBe([intValue1, intValue2], ignoreOrder: true);
        }

        [Theory]
        [InlineData("MyValue", "MyValue")]
        [InlineData("SomeClass.MyValue", "MyValue")]
        public void NameOfConstEvaluatorTest(string textExpression, string expectedValue)
        {
            var resolver = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

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
            var resolverMock = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolverMock, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($"typeof({textExpression})");

            var value = walker.Visit(exp);

            var typeDeclarationUse = value.ShouldBeAssignableTo<IDeclarationUse<SyntaxNode>>();
            typeDeclarationUse.Declaration.Name.ShouldBe(textExpression);
        }

        [Theory]
        [InlineData("$\"Name of is {nameof(Arg1)}\"", "Name of is Arg1")]
        public void InterpolationEvaluatorTest(string textExpression, string expectedValue)
        {
            var resolverMock = Substitute.For<IDeclarationResolver>();
            var genericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolverMock, genericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExpression);

            var value = walker.Visit(exp);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("ConstField", 123)]
        public void ConstFieldEvaluatorTest(string textExpression, object expectedValue)
        {
            var resolverMock = Substitute.For<IDeclarationResolver>();
            var genericDeclarationMock = MockGenericDeclarationWithConstant(textExpression, expectedValue);

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolverMock, genericDeclarationMock);
            var exp = SyntaxTreeHelper.GetExpressionSyntax(textExpression);

            var value = walker.Visit(exp);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("ClassName", "ConstField", 123)]
        public void ExternalClassConstFieldEvaluatorTest(string classExpression, string textExpression, object expectedValue)
        {
            var resolverMock = Substitute.For<IDeclarationResolver>();

            var genericDeclarationMock = MockGenericDeclarationWithConstant(textExpression, expectedValue);

            var currentGenericDeclaration = Substitute.For<IGenericDeclaration<SyntaxNode>>();

            resolverMock.Resolve(classExpression, currentGenericDeclaration).Returns(genericDeclarationMock);

            var walker = new ConstantExpressionSyntaxEvaluator<object>(resolverMock, currentGenericDeclaration);
            var exp = SyntaxTreeHelper.GetExpressionSyntax($"{classExpression}.{textExpression}");

            var value = walker.Visit(exp);

            Assert.Equal(expectedValue, value);
        }

        private static IGenericDeclaration<SyntaxNode> MockGenericDeclarationWithConstant(string constantName, object constantValue)
        {
            var genericDeclarationMock = Substitute.For<IGenericDeclaration<SyntaxNode>>();
            var constantDeclarationMock = Substitute.For<IConstantDeclaration>();
            var constantDeclarationSyntaxProviderMock = Substitute.For<ISyntaxNodeProvider<VariableDeclaratorSyntax>>();

            constantDeclarationMock.Name.Returns(constantName);
            constantDeclarationMock.SyntaxNodeProvider.Returns(constantDeclarationSyntaxProviderMock);

            var constExp = SyntaxTreeHelper.GetFieldSyntax("object", $"{constantName} = {constantValue}");

            constantDeclarationSyntaxProviderMock.SyntaxNode.Returns(constExp.Declaration.Variables.First());

            var constants = new IConstantDeclaration[] { constantDeclarationMock };

            genericDeclarationMock.Constants.Returns(constants);
            return genericDeclarationMock;
        }
    }
}
