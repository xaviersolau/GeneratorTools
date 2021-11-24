// ----------------------------------------------------------------------
// <copyright file="PackedForEachStatementPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Impl
{
    public class PackedForEachStatementPattern : ISimplePattern
    {
        public object PatternProperty { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [PackStatements]
        public void PatternMethodWithPackedForEachStatement(IEnumerable<object> withSomeArguments)
        {
            foreach (var arg in withSomeArguments)
            {
                this.PatternProperty = default;
            }
        }
    }
}
