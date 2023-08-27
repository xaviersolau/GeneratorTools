// ----------------------------------------------------------------------
// <copyright file="IRepeatPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf
{
    public interface IRepeatPattern
    {
        [Pattern<ReadWritePropertySelector>]
        object StatementProperty { get; set; }

        [Pattern<ReadOnlyPropertySelector>]
        object PatternProperty { get; }
    }
}
