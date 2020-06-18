// ----------------------------------------------------------------------
// <copyright file="DependencyAttribute.cs" company="SoloX Software">
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
    /// Attribute to specify that the property must be initialized from dependency injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DependencyAttribute : Attribute
    {
    }
}
