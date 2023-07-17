// ----------------------------------------------------------------------
// <copyright file="IFactoryPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;

namespace SoloX.GeneratorTools.Generator.Patterns.Itf
{
    /// <summary>
    /// factory interface to use as pattern.
    /// </summary>
    [Pattern<AttributeSelector<FactoryAttribute>>]
    public interface IFactoryPattern
    {
        /// <summary>
        /// Create a IObjectPattern instance.
        /// </summary>
        /// <param name="someReadOnlyValue">Some optional parameter.</param>
        /// <param name="someValue">Some mandatory parameter.</param>
        /// <returns>The created IObjectPattern instance.</returns>
        [Repeat(Pattern = nameof(IObjectPattern), Prefix = "I")]
        IObjectPattern CreateObjectPattern(
            [Repeat(Pattern = nameof(IObjectPattern.SomeReadOnlyValue))]
            object someReadOnlyValue,
            [Repeat(Pattern = nameof(IObjectPattern.SomeValue))]
            object someValue = default);
    }
}
