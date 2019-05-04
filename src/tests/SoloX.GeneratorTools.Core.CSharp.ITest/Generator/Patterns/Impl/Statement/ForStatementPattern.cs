// ----------------------------------------------------------------------
// <copyright file="ForStatementPattern.cs" company="SoloX Software">
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
    public class ForStatementPattern : ISimplePattern
    {
        public object PatternProperty { get; set; }

        public void PatternMethodForForStatementPattern(IList<object> withSomeArguments)
        {
            var len = withSomeArguments.Count;
            for (var i = 0; i < len; i++)
            {
                withSomeArguments[i] = this.PatternProperty;
            }
        }
    }
}
