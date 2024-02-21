// ----------------------------------------------------------------------
// <copyright file="Lambda2StatementSimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.Statement
{
    [Pattern<MultiSelector>]
    [Repeat(Pattern = nameof(ISimplePattern), Prefix = "I")]
    public class Lambda2StatementSimplePattern : ISimplePattern
    {
        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty { get; set; }

        [Repeat(Pattern = nameof(ISimplePattern.PatternProperty))]
        public int PatternPropertyWithLambdaStatement(ReadOnlyCollection<object> items)
        {
            return items.Count((object i) =>
            {
                return i == PatternProperty;
            });
        }
    }
}
