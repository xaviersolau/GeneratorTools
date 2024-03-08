// ----------------------------------------------------------------------
// <copyright file="InjectedGeneratorLogger.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.Utils;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.Extensions.Utils
{
    /// <summary>
    /// Injected GeneratorLogger wrapper that will use the injected IGeneratorLoggerFactory to make the target IGeneratorLogger.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public class InjectedGeneratorLogger<TType> : IGeneratorLogger<TType>
    {
        private readonly IGeneratorLogger<TType> logger;

        /// <summary>
        /// Setup GeneratorLogger through IoC.
        /// </summary>
        /// <param name="generatorLoggerFactory"></param>
        public InjectedGeneratorLogger(IGeneratorLoggerFactory generatorLoggerFactory)
        {
            if (generatorLoggerFactory == null)
            {
                throw new ArgumentNullException(nameof(generatorLoggerFactory));
            }

            this.logger = generatorLoggerFactory.CreateLogger<TType>();
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
