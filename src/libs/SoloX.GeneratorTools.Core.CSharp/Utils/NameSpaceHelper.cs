// ----------------------------------------------------------------------
// <copyright file="NameSpaceHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Utils
{
    /// <summary>
    /// Helper to handle NameSpace.
    /// </summary>
    public static class NameSpaceHelper
    {
        /// <summary>
        /// Compute the list of parent name spaces.
        /// </summary>
        /// <param name="declarationNameSpace">Declaration name space we get the parent NameSpaces from.</param>
        /// <returns>The parent NameSpaces.</returns>
        public static IEnumerable<string> GetParentNameSpaces(string declarationNameSpace)
        {
            var nameSpace = declarationNameSpace;
            while (!string.IsNullOrEmpty(nameSpace))
            {
                yield return nameSpace;
                var index = nameSpace.LastIndexOf('.');
                nameSpace = index >= 0 ? nameSpace.Substring(0, index) : null;
            }
        }
    }
}
