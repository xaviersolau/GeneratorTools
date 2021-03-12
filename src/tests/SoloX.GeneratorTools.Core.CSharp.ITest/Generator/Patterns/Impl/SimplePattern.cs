// ----------------------------------------------------------------------
// <copyright file="SimplePattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Impl
{
    [Pattern(typeof(SampleSelector))]
    [Repeat(RepeatPattern = nameof(ISimplePattern), RepeatPatternPrefix = "I")]
    public class SimplePattern : ISimplePattern
    {
        [Repeat(RepeatPattern = nameof(ISimplePattern.PatternProperty))]
        private object patternProperty;

        [Repeat(RepeatPattern = nameof(ISimplePattern.PatternProperty))]
        public object PatternProperty
        {
            get { return this.patternProperty; }
            set { this.patternProperty = value; }
        }
    }
}
