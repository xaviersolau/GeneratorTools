// ----------------------------------------------------------------------
// <copyright file="ConstPattern.cs" company="Xavier Solau">
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
    [Repeat(Pattern = nameof(IConstPattern), Prefix = "I")]
    public class ConstPattern : IConstPattern
    {
        [Repeat(Pattern = nameof(IConstPattern.PatternConst))]
        private object patternConstField;

        [Repeat(Pattern = nameof(IConstPattern.PatternConst))]
        public object PatternConstProperty
        {
            get { return this.patternConstField; }
            set { this.patternConstField = value; }
        }
    }
}
