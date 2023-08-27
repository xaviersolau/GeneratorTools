// ----------------------------------------------------------------------
// <copyright file="AttributeSelectorPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl
{
    [Pattern<MultiSelector>]
    [Repeat(Pattern = nameof(IAttributeSelectorPattern), Prefix = "I")]
    public class AttributeSelectorPattern : IAttributeSelectorPattern
    {
        [Repeat(Pattern = nameof(IAttributeSelectorPattern.AttributeProperty))]
        public object AttributeProperty { get; set; }

        [Repeat(Pattern = nameof(IAttributeSelectorPattern.PatternProperty))]
        public object PatternProperty { get; set; }

        [Repeat(Pattern = nameof(IAttributeSelectorPattern.AttributeProperty))]
        public object ProcessAttributeProperty()
        {
            return AttributeProperty;
        }
    }
}
