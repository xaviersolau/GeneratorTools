// ----------------------------------------------------------------------
// <copyright file="CSharpProjectData.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// CSharp project data.
    /// </summary>
    public class CSharpProjectData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpProjectData"/> class.
        /// </summary>
        /// <param name="targetFramework">The TargetFramework property.</param>
        /// <param name="targetFrameworks">The TargetFrameworks property.</param>
        /// <param name="rootNamespace">The project root name space.</param>
        /// <param name="projectAssetsFile">The project assets file path.</param>
        /// <param name="projectReferences">The project references.</param>
        /// <param name="compileList">The compile list.</param>
        public CSharpProjectData(
            string targetFramework,
            string targetFrameworks,
            string rootNamespace,
            string projectAssetsFile,
            IReadOnlyCollection<string> projectReferences,
            IReadOnlyCollection<string> compileList)
        {
            this.TargetFramework = targetFramework;
            this.TargetFrameworks = targetFrameworks;
            this.RootNamespace = rootNamespace;
            this.ProjectAssetsFile = projectAssetsFile;
            this.ProjectReferences = projectReferences ?? Array.Empty<string>();
            this.CompileList = compileList ?? Array.Empty<string>();
        }

        /// <summary>
        /// Gets the TargetFramework property
        /// </summary>
        public string TargetFramework { get; private set; }

        /// <summary>
        /// Gets the TargetFrameworks property
        /// </summary>
        public string TargetFrameworks { get; private set; }

        /// <summary>
        /// Gets root name space.
        /// </summary>
        public string RootNamespace { get; private set; }

        /// <summary>
        /// Gets project assets file.
        /// </summary>
        public string ProjectAssetsFile { get; private set; }

        /// <summary>
        /// Gets project references.
        /// </summary>
        public IReadOnlyCollection<string> ProjectReferences { get; private set; }

        /// <summary>
        /// Gets compile list.
        /// </summary>
        public IReadOnlyCollection<string> CompileList { get; private set; }
    }
}
