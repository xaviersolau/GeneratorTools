// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspace.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implements ICSharpWorkspace.
    /// </summary>
    public class CSharpWorkspace : ICSharpWorkspace
    {
        private Dictionary<string, ICSharpProject> projects = new Dictionary<string, ICSharpProject>();
        private Dictionary<string, ICSharpFile> files = new Dictionary<string, ICSharpFile>();
        private ICSharpFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpWorkspace"/> class.
        /// </summary>
        /// <param name="factory">The factory to create File and Project object.</param>
        public CSharpWorkspace(ICSharpFactory factory)
        {
            this.factory = factory;
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
            }

            return csProject;
        }
    }
}
