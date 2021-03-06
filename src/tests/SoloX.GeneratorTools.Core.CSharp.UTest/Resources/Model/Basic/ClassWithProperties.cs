﻿// ----------------------------------------------------------------------
// <copyright file="ClassWithProperties.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic
{
    public class ClassWithProperties
    {
        public SimpleClass PropertyClass { get; set; }

        public int PropertyInt { get; set; }
    }
}
