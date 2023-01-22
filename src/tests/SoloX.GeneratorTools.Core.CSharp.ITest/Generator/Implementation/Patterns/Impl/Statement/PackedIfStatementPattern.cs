﻿// ----------------------------------------------------------------------
// <copyright file="PackedIfStatementPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Implementation.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Implementation.Patterns.Impl.Statement
{
    public class PackedIfStatementPattern : ISimplePattern
    {
        public object PatternProperty { get; set; }

        [PackStatements]
        public void PatternMethodWithPackedIfStatement(bool withSomeArguments)
        {
            if (withSomeArguments)
            {
                PatternProperty = default;
            }
        }
    }
}