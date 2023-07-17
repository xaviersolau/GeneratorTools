// ----------------------------------------------------------------------
// <copyright file="TypeNameReplaceMethodSimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.Method
{
    [Pattern<SampleSelector>]
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class TypeNameReplaceMethodSimplePattern : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty => null;

        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object ProcessPatternProperty(object withSomePatternPropertyArgument)
        {
            return this.PatternProperty.ToObject();
        }
    }

    public static class Ext
    {
        public static object ToObject(this object data)
        {
            return null;
        }
    }
}
