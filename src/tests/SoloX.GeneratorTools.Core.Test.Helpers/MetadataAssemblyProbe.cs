// ----------------------------------------------------------------------
// <copyright file="MetadataAssemblyProbe.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.CSharp.Extensions;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Workspace;

namespace SoloX.GeneratorTools.Core.Test.Helpers
{
    /// <summary>
    /// Probe information from Assembly Metadata.
    /// </summary>
    public static class MetadataAssemblyProbe
    {
        /// <summary>
        /// Load metadata assembly and run assert handler.
        /// </summary>
        /// <param name="assemblyFile">Assembly file to load.</param>
        /// <param name="assertHandler">Assert handler.</param>
        /// <returns>Metadata Assembly Loading Logs.</returns>
        public static IEnumerable<LoadingLogs> LoadMetadataAssemblyAndAssert(string assemblyFile, Action<IDeclarationResolver> assertHandler)
        {
            var logs = new List<LoadingLogs>();
            using var loggerFactory = new LoggerFactory(logs.Add);

            LoadMetadataAssemblyAndAssert(
                assemblyFile,
                assertHandler,
                services => services.AddSingleton<ILoggerFactory>(loggerFactory).AddSingleton<ILogger>(loggerFactory));

            return logs;
        }

        /// <summary>
        /// Load metadata assembly and run assert handler.
        /// </summary>
        /// <param name="assemblyFile">Assembly file to load.</param>
        /// <param name="assertHandler">Assert handler.</param>
        /// <param name="loggerSetup">Services delegate to set up the logger to use.</param>
        public static void LoadMetadataAssemblyAndAssert(string assemblyFile, Action<IDeclarationResolver> assertHandler, Func<IServiceCollection, IServiceCollection> loggerSetup)
        {
            var services = new ServiceCollection()
                .AddCSharpToolsGenerator();

            services = loggerSetup(services);

            using var provider = services.BuildServiceProvider();

            var wsf = provider.GetRequiredService<ICSharpWorkspaceFactory>();

            var ws = wsf.CreateWorkspace();

            var absAssemblyFile = Path.GetFullPath(assemblyFile);

            ws.RegisterMetadataAssembly(absAssemblyFile);

            var decResolver = ws.DeepLoad();

            assertHandler(decResolver);
        }

        private sealed class LoggerFactory : ILoggerFactory, ILogger
        {
            private readonly Action<LoadingLogs> logsHandler;

            public LoggerFactory(Action<LoadingLogs> logsHandler)
            {
                this.logsHandler = logsHandler;
            }

            public void AddProvider(ILoggerProvider provider)
            {
                throw new NotImplementedException();
            }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            {
                throw new NotImplementedException();
            }

            public ILogger CreateLogger(string categoryName)
            {
                return this;
            }

            public void Dispose()
            {
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                this.logsHandler(new LoadingLogs(logLevel, formatter(state, exception), exception));
            }
        }
    }

    public class LoadingLogs
    {
        public LoadingLogs(LogLevel logLevel, string message, Exception? exception)
        {
            LogLevel = logLevel;
            Message = message;
            Exception = exception;
        }

        public LogLevel LogLevel { get; }

        public string Message { get; }

        public Exception? Exception { get; }
    }
}
