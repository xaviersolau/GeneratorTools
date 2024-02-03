// ----------------------------------------------------------------------
// <copyright file="SimpleMethod6Pattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl
{
    [Pattern<MultiSelector>]
    [Repeat(Pattern = nameof(ISimpleMethodPattern), Prefix = "I")]
    public class SimpleMethod6Pattern : ISimpleMethodPattern
    {
        [Repeat(Pattern = nameof(ISimpleMethodPattern.PatternMethod))]
#pragma warning disable CA1034 // Nested types should not be visible
        public class PatternMethodPayload
        {
            [Repeat(Pattern = "someArgument")]
            public object SomeArgument { get; set; }
        }
#pragma warning restore CA1034 // Nested types should not be visible



        [Repeat(Pattern = nameof(ISimpleMethodPattern.PatternMethod))]
        public object PatternMethod([Repeat(Pattern = "someArgument")] object someArgument)
        {
            var payload = new PatternMethodPayload
            {
                SomeArgument = Repeat.Affectation("someArgument", someArgument),
            };

            return ProcessPatternMethodPayload(payload);
        }

        [Repeat(Pattern = nameof(ISimpleMethodPattern.PatternMethod))]
        [return: Repeat(Pattern = "someArgument")]
        private object ProcessPatternMethodPayload(PatternMethodPayload payload)
        {
            var someArgument = Repeat.Affectation("someArgument", payload.SomeArgument);

            return Process(Repeat.Argument("someArgument", someArgument));
        }

        private object Process(params object[] arguments)
        {
            throw new NotImplementedException();
        }
    }
}
