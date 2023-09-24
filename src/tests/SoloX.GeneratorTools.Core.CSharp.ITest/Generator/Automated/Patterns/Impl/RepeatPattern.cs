// ----------------------------------------------------------------------
// <copyright file="RepeatPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl
{
    [Pattern<MultiSelector>]
    [Repeat(Pattern = nameof(IRepeatPattern), Prefix = "I")]
    public class RepeatPattern : IRepeatPattern
    {
        [Repeat(Pattern = nameof(IRepeatPattern.StatementProperty))]
        public object StatementProperty { get; set; }

        [Repeat(Pattern = nameof(IRepeatPattern.PatternProperty))]
        public object PatternProperty { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Repeat(Pattern = nameof(IRepeatPattern.PatternProperty))]
        public void PatternPropertyMethod()
        {
            Repeat.Statements(nameof(IRepeatPattern.StatementProperty),
                () => DoSome(StatementProperty, PatternProperty));
        }

        private static void DoSome(object arg1, object arg2)
        {
            // Any code...
        }
    }
}
