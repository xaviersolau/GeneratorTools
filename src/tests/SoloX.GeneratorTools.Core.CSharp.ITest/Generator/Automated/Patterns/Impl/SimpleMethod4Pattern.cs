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

            var someArgumentVar = Repeat.Affectation(someArgument.ToString());

            var someArgumentVar2 = Repeat.Affectation<string>(default);

            s1 = s1 + "111";

            someArgumentVar2 = Repeat.Affectation<string>(someArgument.ToString());

            Process(Repeat.Argument(someArgument));

            Repeat.Statements(() =>
            {
                Process(someArgument.ToString());

                Process(someArgumentVar);
            });

            Repeat.Statements(() =>
            {
                if (someArgumentVar2 != null)
                {
                    Process(someArgumentVar2);
                }
            });

            if (true)
            {
                Repeat.Statements(() =>
                {
                    Process(someArgumentVar2);
                });
            }

            return string.Join(',', new string[] { Repeat.Argument(someArgumentVar2) });
        }

        private static void Process(params object[] strings)
        {
            // ...
        }
    }
}
