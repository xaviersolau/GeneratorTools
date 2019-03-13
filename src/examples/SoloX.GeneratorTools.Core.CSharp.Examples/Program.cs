// ----------------------------------------------------------------------
// <copyright file="Program.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Examples
{
    /// <summary>
    /// Example program.
    /// </summary>
    public sealed class Program : IDisposable
    {
        private readonly ILogger<Program> logger;

        private Program()
        {
            IServiceCollection sc = new ServiceCollection();

            sc.AddLogging(b => b.AddConsole());

            sc.AddSingleton<ICSharpFactory, CSharpFactory>();
            sc.AddSingleton<ICSharpLoader, CSharpLoader>();
            sc.AddTransient<ICSharpWorkspace, CSharpWorkspace>();

            this.Service = sc.BuildServiceProvider();

            this.logger = this.Service.GetService<ILogger<Program>>();
        }

        private ServiceProvider Service { get; }

        /// <summary>
        /// Main program entry point.
        /// </summary>
        public static void Main()
        {
            new Program().Run();
            Console.ReadLine();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Service.Dispose();
        }

        private void Run()
        {
            var prjFile = "../../../../../libs/SoloX.GeneratorTools.Core.CSharp/SoloX.GeneratorTools.Core.CSharp.csproj";

            this.logger.LogInformation($"Loading {Path.GetFileName(prjFile)}...");

            var csws = this.Service.GetService<ICSharpWorkspace>();

            csws.RegisterProject(prjFile);

            var resolver = csws.DeepLoad();

            var declaration = resolver.Find("SoloX.GeneratorTools.Core.CSharp.Model.IDeclaration").Single() as IGenericDeclaration;

            foreach (var extendedByItem in declaration.ExtendedBy)
            {
                this.logger.LogInformation(extendedByItem.FullName);
            }
        }
    }
}
