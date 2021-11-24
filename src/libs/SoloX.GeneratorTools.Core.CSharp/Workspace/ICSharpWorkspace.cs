// ----------------------------------------------------------------------
// <copyright file="ICSharpWorkspace.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// The CSharp workspace.
    /// </summary>
    public interface ICSharpWorkspace
    {
        /// <summary>
        /// Gets the workspace projects.
        /// </summary>
        IReadOnlyCollection<ICSharpProject> Projects { get; }

        /// <summary>
        /// Gets the workspace CSharp files.
        /// </summary>
        IReadOnlyCollection<ICSharpFile> Files { get; }

        /// <summary>
        /// Gets the workspace SCharp assembly dependencies.
        /// </summary>
        IReadOnlyCollection<ICSharpAssembly> Assemblies { get; }

        /// <summary>
        /// Register the project associated to the given project file.
        /// </summary>
        /// <param name="projectFile">The project file.</param>
        /// <returns>The matching or created project.</returns>
        ICSharpProject RegisterProject(string projectFile);

        /// <summary>
        /// Register the ICSharpFile associated to the given file.
        /// </summary>
        /// <param name="file">The CSharp file.</param>
        /// <returns>The matching or created ICSharpFile.</returns>
        ICSharpFile RegisterFile(string file);

        /// <summary>
        /// Register the ICSharpAssembly associated to the given assembly.
        /// </summary>
        /// <param name="assemblyFile">The CSharp assembly file.</param>
        /// <returns>The matching or created ICSharpAssembly.</returns>
        ICSharpAssembly RegisterAssembly(string assemblyFile);

        /// <summary>
        /// Deep load the workspace.
        /// </summary>
        /// <returns>The resulting declaration resolver.</returns>
        IDeclarationResolver DeepLoad();
    }
}
