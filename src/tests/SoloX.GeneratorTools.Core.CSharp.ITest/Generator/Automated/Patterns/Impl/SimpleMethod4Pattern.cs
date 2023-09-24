// ----------------------------------------------------------------------
// <copyright file="SimpleMethod4Pattern.cs" company="Xavier Solau">
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
    public class SimpleMethod4Pattern : ISimpleMethodPattern
    {
        [Repeat(Pattern = nameof(ISimpleMethodPattern.PatternMethod))]
        public object PatternMethod([Repeat(Pattern = "someArgument")] object someArgument)
        {
            var s1 = "???";

            var someArgumentVar = Repeat.Affectation("someArgument", someArgument.ToString());

            var someArgumentVar2 = Repeat.Affectation<string>("someArgument", default);

            s1 = s1 + "111";

            someArgumentVar2 = Repeat.Affectation<string>("someArgument", someArgument.ToString());

            Process(Repeat.Argument("someArgument", someArgument));

            Repeat.Statements("someArgument", () =>
            {
                Process(someArgument.ToString());

                Process(someArgumentVar);
            });

            Repeat.Statements("someArgument", () =>
            {
                if (someArgumentVar2 != null)
                {
                    Process(someArgumentVar2);
                }
            });

            if (true)
            {
                Repeat.Statements("someArgument", () =>
                {
                    Process(someArgumentVar2);
                });
            }

            return default;
        }

        private static void Process(params object[] strings)
        {
            // ...
        }
    }
}
