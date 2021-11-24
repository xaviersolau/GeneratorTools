// ----------------------------------------------------------------------
// <copyright file="ILocator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.Generator
{
    /// <summary>
    /// Locator interface used to compute the target name space and location of the code generated
    /// from the given declaration name space.
    /// </summary>
    public interface ILocator
    {
        /// <summary>
        /// Compute the target where to generate code from the given declaration name space.
        /// </summary>
        /// <param name="declarationNameSpace">The declaration name space from which the code will be generated.</param>
        /// <returns>The tuple location and name space where the code must be generated.</returns>
        (string location, string nameSpace) ComputeTargetLocation(string declarationNameSpace);
    }
}
