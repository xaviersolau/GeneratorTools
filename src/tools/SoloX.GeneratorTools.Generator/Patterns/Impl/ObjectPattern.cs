// ----------------------------------------------------------------------
// <copyright file="ObjectPattern.cs" company="Xavier Solau">
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
    /// Implementation of Object pattern interface.
    /// </summary>
    [Pattern<AttributeSelector<FactoryAttribute>>]
    [Repeat(Pattern = nameof(IObjectPattern), Prefix = "I")]
    public class ObjectPattern : IObjectPattern
    {
        /// <inheritdoc/>
        [Repeat(Pattern = nameof(IObjectPattern.SomeReadOnlyValue))]
        public object SomeReadOnlyValue { get; set; }

        /// <inheritdoc/>
        [Repeat(Pattern = nameof(IObjectPattern.SomeValue))]
        public object SomeValue { get; set; }
    }
}
