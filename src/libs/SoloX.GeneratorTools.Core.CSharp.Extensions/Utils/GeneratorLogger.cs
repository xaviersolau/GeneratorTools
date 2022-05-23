// ----------------------------------------------------------------------
// <copyright file="GeneratorLogger.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.Utils;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.Extensions.Utils
{
    /// <summary>
    /// GeneratorLogger wrapper to use standard ILogger.
    /// </summary>
    /// <typeparam name="TType">Logger type.</typeparam>
    public class GeneratorLogger<TType> : IGeneratorLogger<TType>
    {
        private readonly ILogger<TType> logger;

        /// <summary>
        /// Setup a logger instance.
        /// </summary>
        /// <param name="logger">Standard logger to wrap.</param>
        public GeneratorLogger(ILogger<TType> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void LogDebug(string message)
        {
            this.logger.LogDebug(message);
        }

        /// <inheritdoc/>
        public void LogDebug(Exception exception, string message)
        {
            this.logger.LogDebug(exception, message);
        }

        /// <inheritdoc/>
        public void LogError(string message)
        {
            this.logger.LogError(message);
        }

        /// <inheritdoc/>
        public void LogError(Exception exception, string message)
        {
            this.logger.LogError(exception, message);
        }

        /// <inheritdoc/>
        public void LogInformation(string message)
        {
            this.logger.LogInformation(message);
        }

        /// <inheritdoc/>
        public void LogInformation(Exception exception, string message)
        {
            this.logger.LogInformation(exception, message);
        }

        /// <inheritdoc/>
        public void LogWarning(string message)
        {
            this.logger.LogWarning(message);
        }

        /// <inheritdoc/>
        public void LogWarning(Exception exception, string message)
        {
            this.logger.LogWarning(exception, message);
        }
    }
}
