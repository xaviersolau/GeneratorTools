// ----------------------------------------------------------------------
// <copyright file="ISimpleObject.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Attributes;

namespace SoloX.GeneratorTools.Generator.ITest.Resources.Data
{
    [Factory]
    public interface ISimpleObject
    {
        int Property1 { get; }

        string Property2 { get; }

        int Property3 { get; set; }

        string Property4 { get; set; }
    }
}
