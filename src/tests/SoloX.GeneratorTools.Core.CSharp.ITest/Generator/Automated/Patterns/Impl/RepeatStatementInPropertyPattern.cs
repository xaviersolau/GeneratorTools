// ----------------------------------------------------------------------
// <copyright file="RepeatStatementInPropertyPattern.cs" company="Xavier Solau">
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
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class RepeatStatementInPropertyPattern : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty { get; set; }

        public string PatternWithStatementInProperty
        {
            get
            {
                var txt = string.Empty;
                Repeat.Statements(nameof(ISimplePattern.PatternProperty), () =>
                {
                    txt = txt + PatternProperty.ToString();
                });
                return txt;
            }
        }
    }
}
