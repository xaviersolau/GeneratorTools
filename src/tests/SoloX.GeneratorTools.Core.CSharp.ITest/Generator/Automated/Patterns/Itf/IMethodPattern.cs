// ----------------------------------------------------------------------
// <copyright file="IMethodPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf
{

    [Pattern<MultiSelector>]
    public interface IMethodPattern
    {
        [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
        ISimplePattern CreateSimplePattern(
            [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
            object patternProperty);
    }
}
