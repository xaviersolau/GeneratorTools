// ----------------------------------------------------------------------
// <copyright file="SimpleMethodPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.Method
{
    [Pattern(typeof(SampleSelector))]
    [Repeat(RepeatPattern = nameof(ISimplePattern), RepeatPatternPrefix = "I")]
    public class SimpleMethodPattern : ISimplePattern
    {
        [Repeat(RepeatPattern = nameof(ISimplePattern.PatternProperty))]
        private object patternProperty;

        [Repeat(RepeatPattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty
        {
            get { return this.patternProperty; }
            set { this.patternProperty = value; }
        }

        [Repeat(RepeatPattern = nameof(ISimplePattern.PatternProperty))]
        public object ProcessPatternProperty(object withSomePatternPropertyArgument)
        {
            this.patternProperty = withSomePatternPropertyArgument;
            return this.patternProperty;
        }
    }
}
