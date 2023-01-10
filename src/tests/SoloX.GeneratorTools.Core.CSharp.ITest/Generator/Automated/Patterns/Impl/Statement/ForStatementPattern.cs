// ----------------------------------------------------------------------
// <copyright file="ForStatementPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl
{
    public class ForStatementPattern : ISimplePattern
    {
        public object PatternProperty { get; set; }

        public void PatternMethodWithForStatement(IList<object> withSomeArguments)
        {
            var len = withSomeArguments.Count;
            for (var i = 0; i < len; i++)
            {
                withSomeArguments[i] = this.PatternProperty;
            }
        }
    }
}
