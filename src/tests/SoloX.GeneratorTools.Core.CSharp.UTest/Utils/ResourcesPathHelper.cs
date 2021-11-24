// ----------------------------------------------------------------------
// <copyright file="ResourcesPathHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;

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
            return Path.Combine(".", "Resources", "Model", "Basic", $"{className}.cs");
        }
    }
}
