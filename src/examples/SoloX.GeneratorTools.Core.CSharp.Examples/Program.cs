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
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Generator;
using SoloX.GeneratorTools.Core.Generator.Impl;

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
            var prjFolder = "../../../../SoloX.GeneratorTools.Core.CSharp.Examples.Sample";
            var prjFile = Path.Combine(prjFolder, "SoloX.GeneratorTools.Core.CSharp.Examples.Sample.csproj");

            var projectNameSpace = "SoloX.GeneratorTools.Core.CSharp.Examples.Sample";

            this.logger.LogInformation($"Loading {Path.GetFileName(prjFile)}...");

            var csws = this.Service.GetService<ICSharpWorkspace>();

            csws.RegisterProject(prjFile);
            csws.RegisterFile("./Patterns/Itf/IEntityPattern.cs");
            csws.RegisterFile("./Patterns/Impl/EntityPattern.cs");

            var resolver = csws.DeepLoad();

            var declaration = resolver.Find("SoloX.GeneratorTools.Core.CSharp.Examples.Sample.IEntityBase").Single() as IGenericDeclaration;
            var itfPatternDeclaration = resolver.Find("SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf.IEntityPattern").Single() as IInterfaceDeclaration;
            var implPatternDeclaration = resolver.Find("SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Impl.EntityPattern").Single() as IGenericDeclaration;

            var locator = new RelativeLocator(prjFolder, projectNameSpace, suffix: "Impl");

            var generator = new ImplementationGenerator(
                new FileGenerator(),
                locator,
                itfPatternDeclaration,
                implPatternDeclaration);

            foreach (var extendedByItem in declaration.ExtendedBy.Where(d => d.Name != "IEntityPattern"))
            {
                this.logger.LogInformation(extendedByItem.FullName);

                generator.Generate((IInterfaceDeclaration)extendedByItem);
            }
        }
    }
}
