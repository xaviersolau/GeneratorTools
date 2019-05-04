// ----------------------------------------------------------------------
// <copyright file="ForEachStatementPattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Impl
{
    public class ForEachStatementPattern : ISimplePattern
    {
        public object PatternProperty { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PatternMethodForForEachStatementPattern(IEnumerable<object> withSomeArguments)
        {
            foreach (var arg in withSomeArguments)
            {
                this.PatternProperty = default;
            }
        }
    }
}
