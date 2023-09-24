// ----------------------------------------------------------------------
// <copyright file="FactoryPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Generator.Patterns.Itf;

namespace SoloX.GeneratorTools.Generator.Patterns.Impl
{
    /// <summary>
    /// Implemented factory to use as pattern.
    /// </summary>
    [Pattern<AttributeSelector<FactoryAttribute>>]
    public class FactoryPattern : IFactoryPattern
    {
        /// <inheritdoc/>
        [Repeat(Pattern = nameof(IObjectPattern), Prefix = "I")]
        public IObjectPattern CreateObjectPattern(
            [Repeat(Pattern = nameof(IObjectPattern.SomeReadOnlyValue))]
            object someReadOnlyValue,
            [Repeat(Pattern = nameof(IObjectPattern.SomeValue))]
            object someValue = default)
        {
            return new ObjectPattern()
            {
                SomeReadOnlyValue = Repeat.Affectation(nameof(IObjectPattern.SomeReadOnlyValue), someReadOnlyValue),
                SomeValue = Repeat.Affectation(nameof(IObjectPattern.SomeValue), someValue),
            };
        }
    }
}
