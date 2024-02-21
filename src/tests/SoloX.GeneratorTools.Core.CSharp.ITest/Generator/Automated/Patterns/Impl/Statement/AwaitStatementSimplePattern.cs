// ----------------------------------------------------------------------
// <copyright file="AwaitStatementSimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Threading.Tasks;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.Statement
{
    [Pattern<MultiSelector>]
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class AwaitStatementSimplePattern : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty { get; set; }

        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public static async Task PatternPropertyWithAwaitStatement(bool withSomeArguments)
        {
            if (withSomeArguments)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }
    }
}
