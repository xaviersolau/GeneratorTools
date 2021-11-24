// ----------------------------------------------------------------------
// <copyright file="IReplacePatternHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern
{
    /// <summary>
    /// Handler to implement replace pattern.
    /// </summary>
    public interface IReplacePatternHandler
    {
        /// <summary>
        /// Apply text replacement on the given pattern text.
        /// </summary>
        /// <param name="patternText">The text where to apply the replacement.</param>
        /// <returns>The resulting patternText.</returns>
        string ApplyOn(string patternText);
    }
}
