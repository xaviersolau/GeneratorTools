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
using System.Text;
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

        private readonly Dictionary<string, ICSharpProject> projects = new Dictionary<string, ICSharpProject>();
        private readonly Dictionary<string, ICSharpFile> files = new Dictionary<string, ICSharpFile>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpWorkspace"/> class.
        /// </summary>
        /// <param name="factory">The factory to create File and Project object.</param>
        /// <param name="loader">The File and Project loader.</param>
        public CSharpWorkspace(ICSharpFactory factory, ICSharpLoader loader)
        {
            this.factory = factory;
            this.loader = loader;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpProject> Projects => this.projects.Values;

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpFile> Files => this.files.Values;

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
        public void DeepLoad()
        {
            var resolver = new DeclarationResolver(this.Files.SelectMany(f => f.Declarations), this.loader.Load);

            resolver.Load();
        }
    }
}
