// ----------------------------------------------------------------------
// <copyright file="TextPatternHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern
{
    /// <summary>
    /// Helper to replace text pattern.
    /// </summary>
    public class TextPatternHelper
    {
        private readonly string pattern;
        private readonly string declaration;
        private readonly string optionalPrefix;
        private readonly string optionalSuffix;

        private readonly Func<string, string> replaceDelegate;

        private static readonly Dictionary<string, string> ReservedPatternNameMap = new Dictionary<string, string>()
        {
            ["object"] = nameof(Object),
            ["string"] = nameof(String),
        };

        /// <summary>
        /// Setup the text pattern.
        /// </summary>
        /// <param name="pattern">Pattern to be replaced.</param>
        /// <param name="declaration">Declaration to replace the pattern with.</param>
        /// <param name="optionalPrefix">Optional prefix.</param>
        /// <param name="optionalSuffix">Optional suffix.</param>
        public TextPatternHelper(string pattern, string declaration, string? optionalPrefix, string? optionalSuffix)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            if (string.IsNullOrEmpty(declaration))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            this.pattern = pattern;
            this.declaration = declaration;
            this.optionalPrefix = optionalPrefix;
            this.optionalSuffix = optionalSuffix;

            var patternLength = pattern.Length;
            var declarationLength = declaration.Length;

            if (!string.IsNullOrEmpty(optionalPrefix) && !string.IsNullOrEmpty(optionalSuffix))
            {
                var patternWithNoSuffix = pattern.Remove(patternLength - optionalSuffix.Length);
                var declarationWithNoSuffix = declaration.EndsWith(optionalSuffix, StringComparison.InvariantCultureIgnoreCase)
                    ? declaration.Remove(declarationLength - optionalSuffix.Length)
                    : declaration;

                var patternWithNoPrefix = pattern.Substring(optionalPrefix.Length);
                var declarationWithNoPrefix = declaration.StartsWith(optionalPrefix, StringComparison.InvariantCultureIgnoreCase)
                    ? declaration.Substring(optionalPrefix.Length)
                    : declaration;

                var patternWithNoSuffixNoPrefix = patternWithNoSuffix.Substring(optionalPrefix.Length);
                var declarationWithNoSuffixNoPrefix = declarationWithNoSuffix.StartsWith(optionalPrefix, StringComparison.InvariantCultureIgnoreCase)
                    ? declarationWithNoSuffix.Substring(optionalPrefix.Length)
                    : declarationWithNoSuffix;

                this.replaceDelegate = text =>
                {
                    var replacedText = ReplacePattern(pattern, declaration, text);
                    replacedText = ReplacePattern(patternWithNoPrefix, declarationWithNoPrefix, replacedText);
                    replacedText = ReplacePattern(patternWithNoSuffix, declarationWithNoSuffix, replacedText);
                    replacedText = ReplacePattern(patternWithNoSuffixNoPrefix, declarationWithNoSuffixNoPrefix, replacedText);

                    return replacedText;
                };
            }
            else if (!string.IsNullOrEmpty(optionalPrefix))
            {
                var patternWithNoPrefix = pattern.Substring(optionalPrefix.Length);
                var declarationWithNoPrefix = declaration.StartsWith(optionalPrefix, StringComparison.InvariantCultureIgnoreCase)
                    ? declaration.Substring(optionalPrefix.Length)
                    : declaration;

                this.replaceDelegate = text =>
                {
                    var replacedText = ReplacePattern(pattern, declaration, text);
                    replacedText = ReplacePattern(patternWithNoPrefix, declarationWithNoPrefix, replacedText);

                    return replacedText;
                };
            }
            else if (!string.IsNullOrEmpty(optionalSuffix))
            {
                var patternWithNoSuffix = pattern.Remove(patternLength - optionalSuffix.Length);
                var declarationWithNoSuffix = declaration.EndsWith(optionalSuffix, System.StringComparison.InvariantCultureIgnoreCase)
                    ? declaration.Remove(declarationLength - optionalSuffix.Length)
                    : declaration;

                this.replaceDelegate = text =>
                {
                    var replacedText = ReplacePattern(pattern, declaration, text);
                    replacedText = ReplacePattern(patternWithNoSuffix, declarationWithNoSuffix, replacedText);

                    return replacedText;
                };
            }
            else
            {
                if (declaration[0] >= 'A'
                    && declaration[0] <= 'Z'
                    && ReservedPatternNameMap.TryGetValue(pattern, out var patternMapValue))
                {

                    this.replaceDelegate = text =>
                    {
                        if (text.Contains(pattern))
                        {
                            text = text.Replace(pattern, patternMapValue);
                        }

                        return ReplacePattern(patternMapValue, declaration, text);
                    };
                }
                else
                {
                    this.replaceDelegate = text =>
                    {
                        return ReplacePattern(pattern, declaration, text);
                    };
                }
            }
        }

        private static string ReplacePattern(string pattern, string declaration, string text)
        {
            var firstLowerPattern = char.ToLowerInvariant(pattern[0]) + pattern.Substring(1);
            var firstLowerDeclaration = char.ToLowerInvariant(declaration[0]) + declaration.Substring(1);

            var firstUpperPattern = char.ToUpperInvariant(pattern[0]) + pattern.Substring(1);
            var firstUpperDeclaration = char.ToUpperInvariant(declaration[0]) + declaration.Substring(1);

            return text
                .Replace(firstUpperPattern, firstUpperDeclaration)
                .Replace(firstLowerPattern, firstLowerDeclaration)
                .Replace(pattern, declaration);
        }

        /// <summary>
        /// Replace pattern in the given text.
        /// </summary>
        /// <param name="text">Text where to find and replace the pattern.</param>
        /// <returns>The replaced text.</returns>
        public string ReplacePattern(string text)
        {
            return this.replaceDelegate(text);
        }
    }
}
