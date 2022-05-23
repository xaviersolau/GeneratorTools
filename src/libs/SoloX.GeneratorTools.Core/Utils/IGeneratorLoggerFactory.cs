// ----------------------------------------------------------------------
// <copyright file="IGeneratorLoggerFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.Utils
{
    /// <summary>
    /// Generator logger factory.
    /// </summary>
    public interface IGeneratorLoggerFactory
    {
        /// <summary>
        /// Create a Generator logger.
        /// </summary>
        /// <typeparam name="TType">Type of the logger.</typeparam>
        /// <returns>The created logger.</returns>
        IGeneratorLogger<TType> CreateLogger<TType>();
    }
}
