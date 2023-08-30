// ----------------------------------------------------------------------
// <copyright file="SimplePatternWithCreate.cs" company="Xavier Solau">
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
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class SimplePatternWithCreate : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty
        {
            get { return new SomeClass("some", "arg"); }
        }

#pragma warning disable CA1034 // Nested types should not be visible
        public class SomeClass
        {
            public SomeClass(object arg1, object arg2)
            {
                // setup.
            }
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
