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
        private const string PatternFieldName = "propertyPattern";

        private const string DeclPropName1 = "PropA";
        private const string DeclPropType1 = "int";
        private const string DeclFieldName1 = "propA";

        private const string DeclPropName2 = "PropB";
        private const string DeclPropType2 = "string";
        private const string DeclFieldName2 = "propB";

        [Fact]
        public void SimplePropertyWriterTest()
        {
            var pw = SetupPropertyWriter(PatternPropType, PatternPropName, (DeclPropType1, DeclPropName1));

            var implPatternPropNode = SyntaxTreeHelper.GetPropertyImplSyntax(PatternPropType, PatternPropName);

            var generatedProperty = NodeWriterHelper.WriteAndAssertSingleMemberOfType<PropertyDeclarationSyntax>(pw, implPatternPropNode);

            Assert.Equal(DeclPropName1, generatedProperty.Identifier.Text);
            Assert.Equal(DeclPropType1, generatedProperty.Type.ToString());
        }

        [Theory]
        [InlineData("propertyPattern", "propA")]
        [InlineData("myPropertyPattern", "myPropA")]
        [InlineData("propertyPatternTest", "propATest")]
        public void SimpleFieldWriterTest(string patternFieldName, string implPropName)
        {
            var pw = SetupPropertyWriter(PatternPropType, PatternPropName, (DeclPropType1, DeclPropName1));

            var implPatternPropNode = SyntaxTreeHelper.GetFieldSyntax(PatternPropType, patternFieldName);

            var fieldProperty = NodeWriterHelper.WriteAndAssertSingleMemberOfType<FieldDeclarationSyntax>(pw, implPatternPropNode);

            Assert.Equal(implPropName, fieldProperty.Declaration.Variables.Single().Identifier.Text);
            Assert.Equal(DeclPropType1, fieldProperty.Declaration.Type.ToString());
        }

        [Theory]
        [InlineData("propertyPattern", "propA")]
        [InlineData("myPropertyPattern", "myPropA")]
        [InlineData("propertyPatternTest", "propATest")]
        [InlineData("this.propertyPattern", "this.propA")]
        public void PropertyWithAccessorsWriterTest(string patternFieldName, string implPropName)
        {
            var pw = SetupPropertyWriter(PatternPropType, PatternPropName, (DeclPropType1, DeclPropName1));

            var implPatternPropNode = SyntaxTreeHelper.GetPropertyImplSyntax(PatternPropType, PatternPropName, patternFieldName);

            var generatedProperty = NodeWriterHelper.WriteAndAssertSingleMemberOfType<PropertyDeclarationSyntax>(pw, implPatternPropNode);

            Assert.Equal(DeclPropName1, generatedProperty.Identifier.Text);
            Assert.Equal(DeclPropType1, generatedProperty.Type.ToString());
            Assert.Contains(implPropName, generatedProperty.AccessorList.ToFullString(), StringComparison.InvariantCulture);
            Assert.DoesNotContain(patternFieldName, generatedProperty.AccessorList.ToFullString(), StringComparison.InvariantCulture);
        }

        [Theory]
        [InlineData("object", "int")]
        [InlineData("GenType<object>", "GenType<int>")]
        [InlineData("string", "string")]
        public void PropertyTypeWriterTest(string patternImplFieldType, string expectedGeneratedFieldName)
        {
            var pw = SetupPropertyWriter(PatternPropType, PatternPropName, (DeclPropType1, DeclPropName1));

            var implPatternFieldNode = SyntaxTreeHelper.GetFieldSyntax(patternImplFieldType, PatternFieldName);

            var generatedField = NodeWriterHelper.WriteAndAssertSingleMemberOfType<FieldDeclarationSyntax>(pw, implPatternFieldNode);

            Assert.Equal(expectedGeneratedFieldName, generatedField.Declaration.Type.ToString());
        }

        [Fact]
        public void MultiPropertyDeclarationWriterTest()
        {
            var pw = SetupPropertyWriter(PatternPropType, PatternPropName, (DeclPropType1, DeclPropName1), (DeclPropType2, DeclPropName2));

            var implPatternFieldNode = SyntaxTreeHelper.GetFieldSyntax(PatternPropType, PatternFieldName);

            var generatedFields = NodeWriterHelper.WriteAndAssertMultiMemberOfType<FieldDeclarationSyntax>(pw, implPatternFieldNode);

            Assert.Equal(2, generatedFields.Count);

            Assert.Equal(DeclPropType1, generatedFields[0].Declaration.Type.ToString());
            var v1 = Assert.Single(generatedFields[0].Declaration.Variables);
            Assert.Equal(DeclFieldName1, v1.Identifier.ValueText);

            Assert.Equal(DeclPropType2, generatedFields[1].Declaration.Type.ToString());
            var v2 = Assert.Single(generatedFields[1].Declaration.Variables);
            Assert.Equal(DeclFieldName2, v2.Identifier.ValueText);
        }

        [Fact]
        public void GenericTypeParameterPropertyWriterTest()
        {
            var itfType = "IList<IPatternType>";
            var declType = "IList<IDeclType>";
            var patternFieldType = "AnyImplType<IPatternType>";
            var expectedFieldType = "AnyImplType<IDeclType>";

            var pw = SetupPropertyWriter(
                itfType,
                PatternPropName,
                TypeParamExtract,
                (declType, DeclPropName1));

            var implPatternFieldNode = SyntaxTreeHelper.GetFieldSyntax(patternFieldType, PatternFieldName);

            var generatedFields = NodeWriterHelper.WriteAndAssertMultiMemberOfType<FieldDeclarationSyntax>(pw, implPatternFieldNode);

            var field = Assert.Single(generatedFields);
            Assert.Equal(expectedFieldType, field.Declaration.Type.ToString());
        }

        private static string TypeParamExtract(string type)
        {
            var start = type.IndexOf('<', StringComparison.InvariantCulture);
            var end = type.IndexOf('>', StringComparison.InvariantCulture);
            if (start > 0 && end > start)
            {
                return type.Substring(start, end - start);
            }

            return type;
        }

        private static PropertyWriter SetupPropertyWriter(
            string itfPatternPropertyType,
            string itfPatternPropertyName,
            params (string propertyType, string propertyName)[] itfDeclarations)
        {
            return SetupPropertyWriter(itfPatternPropertyType, itfPatternPropertyName, null, itfDeclarations);
        }

        private static PropertyWriter SetupPropertyWriter(
            string itfPatternPropertyType,
            string itfPatternPropertyName,
            Func<string, string> typeTextExtractor,
            params (string propertyType, string propertyName)[] itfDeclarations)
        {
            var itfPatternProp = DeclarationHelper.SetupPropertyDeclaration(itfPatternPropertyType, itfPatternPropertyName);
            var itfDeclProps = new List<IPropertyDeclaration>();
            foreach (var itfDeclaration in itfDeclarations)
            {
                var itfDeclProp = DeclarationHelper.SetupPropertyDeclaration(itfDeclaration.propertyType, itfDeclaration.propertyName);
                itfDeclProps.Add(itfDeclProp);
            }

            return new PropertyWriter(itfPatternProp, itfDeclProps, typeTextExtractor);
        }
    }
}
