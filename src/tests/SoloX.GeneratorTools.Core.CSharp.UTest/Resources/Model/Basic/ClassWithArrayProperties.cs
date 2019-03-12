// ----------------------------------------------------------------------
// <copyright file="ClassWithArrayProperties.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic
{
    public class ClassWithArrayProperties
    {
#pragma warning disable CA1819 // Properties should not return arrays
        public SimpleClass[] PropertyClass { get; set; }

        public int[] PropertyInt { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
    }
}
