// ----------------------------------------------------------------------
// <copyright file="FactoryPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Generator.Patterns.Itf;

namespace SoloX.GeneratorTools.Generator.Patterns.Impl
{
    /// <summary>
    /// Implemented factory to use as pattern.
    /// </summary>
    [Pattern(typeof(AttributeSelector<FactoryAttribute>))]
    public class FactoryPattern : IFactoryPattern
    {
        /// <inheritdoc/>
        [Repeat(RepeatPattern = nameof(IObjectPattern), RepeatPatternPrefix = "I")]
        public IObjectPattern CreateObjectPattern(
            [Repeat(RepeatPattern = nameof(IObjectPattern.SomeReadOnlyValue))]
            object someReadOnlyValue,
            [Repeat(RepeatPattern = nameof(IObjectPattern.SomeValue))]
            object someValue = default)
        {
            return new ObjectPattern()
            {
                SomeReadOnlyValue = someReadOnlyValue,
                SomeValue = someValue,
            };
        }
    }
}
