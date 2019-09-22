// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspace.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implements ICSharpWorkspace.
    /// </summary>
    public class CSharpWorkspace : ICSharpWorkspace
    {
        private readonly ICSharpFactory factory;
        private readonly ICSharpLoader loader;
        private readonly ILogger<CSharpWorkspace> logger;

        private readonly Dictionary<string, ICSharpProject> projects = new Dictionary<string, ICSharpProject>();
        private readonly Dictionary<string, ICSharpFile> files = new Dictionary<string, ICSharpFile>();
        private readonly Dictionary<string, ICSharpAssembly> assemblies = new Dictionary<string, ICSharpAssembly>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpWorkspace"/> class.
        /// </summary>
        /// <param name="logger">The logger to log errors.</param>
        /// <param name="factory">The factory to create File and Project object.</param>
        /// <param name="loader">The File and Project loader.</param>
        public CSharpWorkspace(ILogger<CSharpWorkspace> logger, ICSharpFactory factory, ICSharpLoader loader)
        {
            this.logger = logger;
            this.factory = factory;
            this.loader = loader;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpProject> Projects => this.projects.Values;

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpFile> Files => this.files.Values;

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpAssembly> Assemblies => this.assemblies.Values;

        /// <inheritdoc/>
        public ICSharpFile RegisterFile(string file)
        {
            // Resolve the full path
            file = Path.GetFullPath(file);
            if (!this.files.TryGetValue(file, out var csFile))
            {
                csFile = this.factory.CreateFile(file);
                this.files.Add(file, csFile);

                this.loader.Load(this, csFile);
            }

            return csFile;
        }

        /// <inheritdoc/>
        public ICSharpProject RegisterProject(string projectFile)
        {
            // Resolve the full path
            projectFile = Path.GetFullPath(projectFile);
            if (!this.projects.TryGetValue(projectFile, out var csProject))
            {
                csProject = this.factory.CreateProject(projectFile);
                this.projects.Add(projectFile, csProject);

                this.loader.Load(this, csProject);
            }

            return csProject;
        }

        /// <inheritdoc/>
        public ICSharpAssembly RegisterAssembly(string assemblyFile)
        {
            if (!this.assemblies.TryGetValue(assemblyFile, out var csAssembly))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    csAssembly = this.factory.CreateAssembly(assembly);
                    this.assemblies.Add(assemblyFile, csAssembly);

                    this.loader.Load(this, csAssembly);
                }
                catch (FileLoadException e)
                {
                    this.logger?.LogWarning(e, $"Could not load assembly from {assemblyFile}");
                }
            }

            return csAssembly;
        }

        /// <inheritdoc/>
        public IDeclarationResolver DeepLoad()
        {
            var declarations = this.Assemblies.SelectMany(a => a.Declarations)
                .Concat(this.Files.SelectMany(f => f.Declarations));
            var resolver = new DeclarationResolver(declarations, this.loader.Load);

            resolver.Load();

            return resolver;
        }
    }
}
