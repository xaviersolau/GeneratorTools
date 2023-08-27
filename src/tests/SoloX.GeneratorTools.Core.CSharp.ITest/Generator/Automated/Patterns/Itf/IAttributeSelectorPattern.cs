// ----------------------------------------------------------------------
// <copyright file="IAttributeSelectorPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using System.ComponentModel;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf
{
    public interface IAttributeSelectorPattern
    {
        [Pattern<AttributeSelector<DescriptionAttribute>>]
        object AttributeProperty { get; set; }

        [Pattern<ReadOnlyPropertySelector>]
        object PatternProperty { get; }
    }
}
