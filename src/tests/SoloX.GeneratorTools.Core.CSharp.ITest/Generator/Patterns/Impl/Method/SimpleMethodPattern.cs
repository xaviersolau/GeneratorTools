// ----------------------------------------------------------------------
// <copyright file="SimpleMethodPattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Impl.Method
{
    public class SimpleMethodPattern : ISimplePattern
    {
        private object patternProperty;

        public object PatternProperty
        {
            get { return this.patternProperty; }
            set { this.patternProperty = value; }
        }

        public object ProcessPatternProperty(object withSomePatternPropertyArgument)
        {
            this.patternProperty = withSomePatternPropertyArgument;
            return this.patternProperty;
        }
    }
}
