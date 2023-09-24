// ----------------------------------------------------------------------
// <copyright file="ForEachStatementSimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.Statement
{
    [Pattern<MultiSelector>]
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class ForEachStatementSimplePattern : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PatternMethodWithForEachStatement(IEnumerable<object> withSomeArguments)
        {
            foreach (var arg in withSomeArguments)
            {
                Repeat.Statements(nameof(ISimplePattern.PatternProperty), () =>
                {
                    this.PatternProperty = default;
                });
            }
        }
    }
}
