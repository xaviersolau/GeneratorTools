// ----------------------------------------------------------------------
// <copyright file="ICSharpProject.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// Interface describing a CSharp project.
    /// </summary>
    public interface ICSharpProject
    {
        /// <summary>
        /// Gets the project file.
        /// </summary>
        string ProjectFile { get; }

        /// <summary>
        /// Gets the project path.
        /// </summary>
        string ProjectPath { get; }

        /// <summary>
        /// Gets the project root name space.
        /// </summary>
        string RootNameSpace { get; }

        /// <summary>
        /// Gets the project references.
        /// </summary>
        IReadOnlyCollection<ICSharpProject> ProjectReferences { get; }

        /// <summary>
        /// Gets the project SCharp files.
        /// </summary>
        IReadOnlyCollection<ICSharpFile> Files { get; }

        /// <summary>
        /// Gets the project SCharp assembly dependencies.
        /// </summary>
        IReadOnlyCollection<ICSharpAssembly> Assemblies { get; }
    }
}
