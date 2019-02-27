// ----------------------------------------------------------------------
// <copyright file="ResourcesPathHelper.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Utils
{
    /// <summary>
    /// Resource path method extension helper.
    /// </summary>
    public static class ResourcesPathHelper
    {
        /// <summary>
        /// Convert the given class name to the Resources/Model/Basic path.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The class file path in Resources/Model/Basic.</returns>
        public static string ToBasicPath(this string className)
        {
            return $"./Resources/Model/Basic/{className}.cs";
        }
    }
}
