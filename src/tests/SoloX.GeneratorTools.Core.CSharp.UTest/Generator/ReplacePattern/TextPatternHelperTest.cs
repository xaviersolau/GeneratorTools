// ----------------------------------------------------------------------
// <copyright file="TextPatternHelperTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Generator.ReplacePattern
{
    public class TextPatternHelperTest
    {
        [Theory]
        [InlineData("object", "int", "", "", "object", "int")]
        [InlineData("object", "int", "", "", "ToObject", "ToInt")]
        [InlineData("IPattern", "IDeclaration", "I", "", "ToPattern", "ToDeclaration")]
        [InlineData("IPattern", "IDeclaration", "", "", "IPattern", "IDeclaration")]
        [InlineData("IPattern", "IDeclaration", "I", "", "Pattern", "Declaration")]
        [InlineData("IPattern", "IDeclaration", "I", "", "pattern", "declaration")]
        [InlineData("IPattern", "IDeclaration", "I", "", "_pattern", "_declaration")]
        [InlineData("IPattern", "Declaration", "I", "", "Pattern", "Declaration")]
        [InlineData("IPattern", "Declaration", "I", "", "IPattern", "Declaration")]
        [InlineData("IPatternList", "IDeclarationList", "I", "List", "ToPattern", "ToDeclaration")]
        [InlineData("IPatternText", "DeclarationText", "I", "", "IPatternText", "DeclarationText")]
        public void ItShouldReplacePatternWithDeclarationInText(string pattern, string declaration, string optionalPrefix, string optionalSuffix, string text, string expected)
        {
            var textPatternHelper = new TextPatternHelper(pattern, declaration, optionalPrefix, optionalSuffix);

            var replacedText = textPatternHelper.ReplacePattern(text);

            replacedText.Should().Be(expected);
        }
    }
}
