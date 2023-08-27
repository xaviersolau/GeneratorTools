// ----------------------------------------------------------------------
// <copyright file="ForStatementSimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.Statement
{
    [Pattern<MultiSelector>]
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class ForStatementSimplePattern : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty { get; set; }

        [RepeatStatements(Pattern = nameof(ISimplePattern.PatternProperty))]
        public void PatternMethodWithForStatement(IList<object> withSomeArguments)
        {
            var len = withSomeArguments.Count;
            for (var i = 0; i < len; i++)
            {
                withSomeArguments[i] = this.PatternProperty;
            }
        }
    }
}
