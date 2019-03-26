// ----------------------------------------------------------------------
// <copyright file="PropertyWriterTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Writer
{
    public class PropertyWriterTest
    {
        private const string PatternPropName = "PropertyPattern";
        private const string PatternPropType = "object";
        private const string DeclPropName = "PropA";
        private const string DeclPropType = "int";

        [Fact]
        public void SimplePropertyWriterTest()
        {
            var pw = SetupPropertyWriter();

            var implPatternPropNode = SyntaxTreeHelper.GetPropertyImplSyntax(PatternPropType, PatternPropName);

            var generatedProperty = WriteAndAssertSingleMemberOfType<PropertyDeclarationSyntax>(pw, implPatternPropNode);

            Assert.Equal(DeclPropName, generatedProperty.Identifier.Text);
            Assert.Equal(DeclPropType, generatedProperty.Type.ToString());
        }

        [Theory]
        [InlineData("propertyPattern", "propA")]
        [InlineData("myPropertyPattern", "myPropA")]
        [InlineData("propertyPatternTest", "propATest")]
        public void SimpleFieldWriterTest(string patternFieldName, string implPropName)
        {
            var pw = SetupPropertyWriter();

            var implPatternPropNode = SyntaxTreeHelper.GetFieldSyntax(PatternPropType, patternFieldName);

            var fieldProperty = WriteAndAssertSingleMemberOfType<FieldDeclarationSyntax>(pw, implPatternPropNode);

            Assert.Equal(implPropName, fieldProperty.Declaration.Variables.Single().Identifier.Text);
            Assert.Equal(DeclPropType, fieldProperty.Declaration.Type.ToString());
        }

        [Theory]
        [InlineData("propertyPattern", "propA")]
        [InlineData("myPropertyPattern", "myPropA")]
        [InlineData("propertyPatternTest", "propATest")]
        [InlineData("this.propertyPattern", "this.propA")]
        public void PropertyWithAccessorsWriterTest(string patternFieldName, string implPropName)
        {
            var pw = SetupPropertyWriter();

            var implPatternPropNode = SyntaxTreeHelper.GetPropertyImplSyntax(PatternPropType, PatternPropName, patternFieldName);

            var generatedProperty = WriteAndAssertSingleMemberOfType<PropertyDeclarationSyntax>(pw, implPatternPropNode);

            Assert.Equal(DeclPropName, generatedProperty.Identifier.Text);
            Assert.Equal(DeclPropType, generatedProperty.Type.ToString());
            Assert.Contains(implPropName, generatedProperty.AccessorList.ToFullString(), StringComparison.InvariantCulture);
            Assert.DoesNotContain(patternFieldName, generatedProperty.AccessorList.ToFullString(), StringComparison.InvariantCulture);
        }

        private static PropertyWriter SetupPropertyWriter()
        {
            var itfPatternProp = DeclarationHelper.SetupPropertyDeclaration(PatternPropType, PatternPropName);
            var itfDeclProp = DeclarationHelper.SetupPropertyDeclaration(DeclPropType, DeclPropName);
            return new PropertyWriter(itfPatternProp, new[] { itfDeclProp });
        }

        private static T WriteAndAssertSingleMemberOfType<T>(PropertyWriter propertyWriter, SyntaxNode implPatternPropNode)
            where T : SyntaxNode
        {
            var output = new StringBuilder();
            propertyWriter.Write(implPatternPropNode, s => output.Append(s));

            return AssertSingleMemberOfType<T>(output);
        }

        private static T AssertSingleMemberOfType<T>(StringBuilder output)
            where T : SyntaxNode
        {
            var resultingNode = SyntaxTreeHelper.GetSyntaxNode(output.ToString());
            var cun = Assert.IsType<CompilationUnitSyntax>(resultingNode);
            var member = Assert.Single(cun.Members);
            return Assert.IsType<T>(member);
        }
    }
}
