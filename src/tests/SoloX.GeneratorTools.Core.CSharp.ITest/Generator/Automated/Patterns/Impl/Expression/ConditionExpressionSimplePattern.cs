﻿// ----------------------------------------------------------------------
// <copyright file="ConditionExpressionSimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl.Expression
{
    public class ConditionExpressionSimplePattern : ISimplePattern
    {
        public object PatternProperty { get; set; }

#pragma warning disable CA1508 // Éviter le code conditionnel mort
#pragma warning disable CA2201 // Ne pas lever de types d'exception réservés
        public void PatternMethodForConditionStatementPattern(bool withSomeArguments)
        {
            if (withSomeArguments || this.PatternProperty == default)
            {
                throw new Exception();
            }

            if (this.PatternProperty == default || withSomeArguments)
            {
                throw new Exception();
            }
        }
#pragma warning restore CA2201 // Ne pas lever de types d'exception réservés
#pragma warning restore CA1508 // Éviter le code conditionnel mort
    }
}
