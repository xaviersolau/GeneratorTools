﻿// ----------------------------------------------------------------------
// <copyright file="PackStatementsAttribute.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Attributes
{
    /// <summary>
    /// Attribute used to tell that the pattern method statements must be packed.
    /// For example use it if all method statements must be taken as a unit of code and must not be considered in detail.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PackStatementsAttribute : Attribute
    {
    }
}
