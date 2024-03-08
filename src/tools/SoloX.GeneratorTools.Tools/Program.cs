// ----------------------------------------------------------------------
// <copyright file="Program.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SoloX.GeneratorTools.Generator;

namespace SoloX.GeneratorTools.Tools
{
    /// <summary>
    /// Program entry point class.
    /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class Program : IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        private readonly ILogger<Program> logger;
        private readonly IConfiguration configuration;
        private readonly ILoggerFactory generatorLoggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        /// <param name="configuration">The configuration that contains all arguments.</param>
        public Program(IConfiguration configuration)
        {
            this.configuration = configuration;

            var fileLogger = new LoggerConfiguration()
                .WriteTo
                .File("logs.txt", formatProvider: CultureInfo.InvariantCulture, rollOnFileSizeLimit: true, fileSizeLimitBytes: 5 * 1_024 * 1_024)
                .MinimumLevel
                .Debug()
                .CreateLogger();

            generatorLoggerFactory = LoggerFactory.Create(
                b =>
                {
                    b.ClearProviders();
                    b.AddSerilog(fileLogger);
                    b.SetMinimumLevel(LogLevel.Debug);
                });

            IServiceCollection sc = new ServiceCollection();

            sc.AddLogging(b => b.AddConsole());
            sc.AddSingleton(configuration);
            sc.AddToolsGenerator(generatorLoggerFactory);

            this.Service = sc.BuildServiceProvider();

            this.logger = this.Service.GetService<ILogger<Program>>();
        }

        private ServiceProvider Service { get; }

        /// <summary>
        /// Main program entry point.
        /// </summary>
        /// <param name="args">Tools arguments.</param>
        /// <returns>Error code.</returns>
        public static int Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);
            var config = builder.Build();

            using var program = new Program(config);

            return program.Run();
        }

        /// <inheritdoc/>
#pragma warning disable CA1063 // Implement IDisposable Correctly
        public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            generatorLoggerFactory.Dispose();

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Run the tools command.
        /// </summary>
        /// <returns>Error code.</returns>
        public int Run()
        {
            var projectFile = this.configuration.GetValue<string>("project");

            if (string.IsNullOrEmpty(projectFile))
            {
                this.logger.LogError($"Missing project file parameter.");
                return -1;
            }

            if (!File.Exists(projectFile))
            {
                this.logger.LogError($"Could not find project file {projectFile}");
                return -1;
            }

            var generator = this.Service.GetService<IToolsGenerator>();
            generator.Generate(projectFile);

            return 0;
        }
    }
}
