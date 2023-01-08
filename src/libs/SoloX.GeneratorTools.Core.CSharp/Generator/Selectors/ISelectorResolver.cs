// ----------------------------------------------------------------------
// <copyright file="ISelectorResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Selectors
{
    /// <summary>
    /// Selector resolver instantiating selector from a given name.
    /// </summary>
    public interface ISelectorResolver
    {
        /// <summary>
        /// Try get a selector matching a given name.
        /// </summary>
        /// <param name="selectorName"></param>
        /// <returns></returns>
        ISelector GetSelector(string selectorName);
    }
}
