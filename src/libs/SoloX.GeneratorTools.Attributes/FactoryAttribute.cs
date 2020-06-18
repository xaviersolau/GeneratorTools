// ----------------------------------------------------------------------
// <copyright file="FactoryAttribute.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Attributes
{
    /// <summary>
    /// Attribute to specify that the class must be created by a generated factory.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class FactoryAttribute : Attribute
    {
    }
}
