// ----------------------------------------------------------------------
// <copyright file="PathHelper.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Utils
{
    /// <summary>
    /// Helper that provides functions to handle path.
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Resolve a relative path given the working directory.
        /// </summary>
        /// <param name="workingDirectory">The working directory.</param>
        /// <param name="path">The path to resolve.</param>
        /// <returns>The resolved path.</returns>
        public static string ResolveRelativePath(string workingDirectory, string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            return Path.Combine(workingDirectory, path);
        }
    }
}
