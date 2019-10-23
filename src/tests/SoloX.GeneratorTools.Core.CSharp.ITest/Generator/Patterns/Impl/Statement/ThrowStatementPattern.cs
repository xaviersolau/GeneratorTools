// ----------------------------------------------------------------------
// <copyright file="ThrowStatementPattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Impl
{
    public class ThrowStatementPattern : ISimplePattern
    {
        public object PatternProperty { get; set; }

        public static void PatternMethodWithThrowStatement(bool withSomeArguments)
        {
            if (withSomeArguments)
            {
                throw new Exception("This is an exception");
            }
        }
    }
}
