// ----------------------------------------------------------------------
// <copyright file="ConstantExpressionSyntaxEvaluatorTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Walker
{
    public class ConstantExpressionSyntaxEvaluatorTest
    {
        [Fact]
        public void SimpleStringConstEvaluatorTest()
        {
            var textValue = "MyValue";
            var walker = new ConstantExpressionSyntaxEvaluator<string>();
            var exp = SyntaxTreeHelper.GetExpressionSyntax($@"""{textValue}""");

            var value = walker.Visit(exp);

            Assert.Equal(textValue, value);
        }

        [Theory]
        [InlineData("new string[]")]
        [InlineData("new []")]
        public void StringArrayConstEvaluatorTest(string arrayDecl)
        {
            var textValue1 = "textValue1";
            var textValue2 = "textValue2";
            var textExp = $@"{arrayDecl} {{ ""{textValue1}"", ""{textValue2}"" }}";

            var walker = new ConstantExpressionSyntaxEvaluator<string[]>();
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
            var walker = new ConstantExpressionSyntaxEvaluator<string>();
            var exp = SyntaxTreeHelper.GetExpressionSyntax($"nameof({textExpression})");

            var value = walker.Visit(exp);

            Assert.Equal(expectedValue, value);
        }
    }
}
