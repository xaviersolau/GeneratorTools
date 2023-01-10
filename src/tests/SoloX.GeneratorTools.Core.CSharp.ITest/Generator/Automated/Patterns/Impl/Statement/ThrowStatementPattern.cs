// ----------------------------------------------------------------------
// <copyright file="ThrowStatementPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl
{
#pragma warning disable CA2201 // Ne pas lever de types d'exception réservés
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
#pragma warning restore CA2201 // Ne pas lever de types d'exception réservés
}
