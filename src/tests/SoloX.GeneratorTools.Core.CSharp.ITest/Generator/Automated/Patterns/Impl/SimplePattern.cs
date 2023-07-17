// ----------------------------------------------------------------------
// <copyright file="SimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl
{
    [Pattern<SampleSelector>]
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class SimplePattern : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        private object patternProperty;

        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty
        {
            get { return this.patternProperty; }
            set { this.patternProperty = value; }
        }
    }
}
