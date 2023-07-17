// ----------------------------------------------------------------------
// <copyright file="IfStatementSimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.Statement
{
    [Pattern<SampleSelector>]
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class IfStatementSimplePattern : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty { get; set; }

        [RepeatStatements(Pattern = nameof(ISimplePattern.PatternProperty))]
        public void PatternMethodWithIfStatement(bool withSomeArguments)
        {
            if (withSomeArguments)
            {
                this.PatternProperty = default;
            }
        }
    }
}
