// ----------------------------------------------------------------------
// <copyright file="SimplePattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Simple.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Patterns.Simple.Impl
{
    public class SimplePattern : ISimplePattern
    {
        public object PatternProperty { get; set; }
    }
}
