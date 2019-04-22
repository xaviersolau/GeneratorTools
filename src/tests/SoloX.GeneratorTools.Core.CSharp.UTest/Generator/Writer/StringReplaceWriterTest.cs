// ----------------------------------------------------------------------
// <copyright file="StringReplaceWriterTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.Writer.Impl;
using SoloX.GeneratorTools.Core.CSharp.UTest.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Generator.Writer
{
    public class StringReplaceWriterTest
    {
        [Theory]
        [InlineData("PatternClassName", "PatternClassName", "NewClassString", "NewClassString")]
        [InlineData("PatternClassName", "myPatternClassName", "NewClassString", "myNewClassString")]
        [InlineData("PatternClassName[]", "myPatternClassName", "NewClassString[]", "myNewClassString")]
        [InlineData("IEnumerable<PatternClassName>", "myPatternClassName", "IEnumerable<NewClassString>", "myNewClassString")]
        [InlineData("patternClassName", "myPatternClassName", "patternClassName", "myNewClassString")]
        public void SimplePropertyWriterTest(string type, string name, string genType, string genName)
        {
            var srw = SetupStringReplaceWriter("PatternClassName", "NewClassString");

            var implPatternPropNode = SyntaxTreeHelper.GetPropertyImplSyntax(type, name);

            var generatedProperty = NodeWriterHelper.WriteAndAssertSingleMemberOfType<PropertyDeclarationSyntax>(srw, implPatternPropNode);

            Assert.Equal(genName, generatedProperty.Identifier.Text);
            Assert.Equal(genType, generatedProperty.Type.ToString());
        }

        [Fact]
        public void MultiPropertyWriterTest()
        {
            var srw = SetupStringReplaceWriter("PatternClassName", new[] { "NewClassString1", "NewClassString2" });

            var implPatternPropNode = SyntaxTreeHelper.GetPropertyImplSyntax("object", "PatternClassName");

            var generatedProperties = NodeWriterHelper.WriteAndAssertMultiMemberOfType<PropertyDeclarationSyntax>(srw, implPatternPropNode);

            Assert.Equal(2, generatedProperties.Count);
            Assert.Equal("NewClassString1", generatedProperties[0].Identifier.Text);
            Assert.Equal("NewClassString2", generatedProperties[1].Identifier.Text);
        }

        private static StringReplaceWriter SetupStringReplaceWriter(string oldString, string newString)
        {
            return new StringReplaceWriter(oldString, newString);
        }

        private static StringReplaceWriter SetupStringReplaceWriter(string oldString, IReadOnlyList<string> newStringList)
        {
            return new StringReplaceWriter(oldString, newStringList);
        }
    }
}
