// ----------------------------------------------------------------------
// <copyright file="ForEachStatementPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl
{
    public class ForEachStatementPattern : ISimplePattern
    {
        public object PatternProperty { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PatternMethodWithForEachStatement(IEnumerable<object> withSomeArguments)
        {
            foreach (var arg in withSomeArguments)
            {
                this.PatternProperty = default;
            }
        }
    }
}
