// ----------------------------------------------------------------------
// <copyright file="GeneratorLoggerFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Extensions.Utils
{
    /// <summary>
    /// GeneratorLogger wrapper to use standard ILogger.
    /// </summary>
    public class GeneratorLoggerFactory : IGeneratorLoggerFactory
    {
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// Setup a GeneratorLoggerFactory.
        /// </summary>
        /// <param name="loggerFactory">The standard logger factory.</param>
        public GeneratorLoggerFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        public IGeneratorLogger<TType> CreateLogger<TType>()
        {
            return new GeneratorLogger<TType>(this.loggerFactory.CreateLogger<TType>());
        }
    }
}
