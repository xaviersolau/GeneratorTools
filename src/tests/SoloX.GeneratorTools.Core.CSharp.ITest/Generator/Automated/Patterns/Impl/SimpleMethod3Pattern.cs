// ----------------------------------------------------------------------
// <copyright file="SimpleMethod3Pattern.cs" company="Xavier Solau">
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
    [Repeat(Pattern = nameof(ISimpleMethodPattern), Prefix = "I")]
    public class SimpleMethod3Pattern : ISimpleMethodPattern
    {
        [Repeat(Pattern = nameof(ISimpleMethodPattern.PatternMethod))]
        public object PatternMethod([Repeat(Pattern = "someArgument")] object someArgument)
        {
            var txt = string.Empty;

            Repeat.Statements(() =>
            {
                txt += someArgument.ToString();
            });

            return txt;
        }
    }
}
