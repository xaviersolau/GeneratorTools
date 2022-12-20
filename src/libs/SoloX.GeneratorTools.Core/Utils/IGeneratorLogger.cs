// ----------------------------------------------------------------------
// <copyright file="IGeneratorLogger.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.GeneratorTools.Core.Utils
{
    /// <summary>
    /// Logger interface to provide an abstraction to be able to remove all assembly dependencies.
    /// </summary>
    /// <typeparam name="TType">Logger type.</typeparam>
    public interface IGeneratorLogger<TType> : IGeneratorLogger
    {
    }

    /// <summary>
    /// Logger interface.
    /// </summary>
    public interface IGeneratorLogger
    {
        /// <summary>
        /// Log debug.
        /// </summary>
        /// <param name="message">Debug message.</param>
        void LogDebug(string message);

        /// <summary>
        /// Log debug.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        /// <param name="message">Debug message.</param>
        void LogDebug(Exception exception, string message);

        /// <summary>
        /// Log an information.
        /// </summary>
        /// <param name="message">Information message.</param>
        void LogInformation(string message);

        /// <summary>
        /// Log an information.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        /// <param name="message">Information message.</param>
        void LogInformation(Exception exception, string message);

        /// <summary>
        /// Log a warning.
        /// </summary>
        /// <param name="message">Warning message.</param>
        void LogWarning(string message);

        /// <summary>
        /// Log a warning.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        /// <param name="message">Warning message.</param>
        void LogWarning(Exception exception, string message);

        /// <summary>
        /// Log an error.
        /// </summary>
        /// <param name="message">Error message.</param>
        void LogError(string message);

        /// <summary>
        /// Log an error.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        /// <param name="message">Error message.</param>
        void LogError(Exception exception, string message);
    }
}
