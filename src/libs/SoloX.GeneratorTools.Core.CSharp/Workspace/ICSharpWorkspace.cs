// ----------------------------------------------------------------------
// <copyright file="ICSharpWorkspace.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model;
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
        /// Gets the workspace SCharp metadata assembly dependencies.
        /// </summary>
        IReadOnlyCollection<ICSharpMetadataAssembly> MetadataAssemblies { get; }

        /// <summary>
        /// Gets the workspace CSharp syntax trees.
        /// </summary>
        IReadOnlyCollection<ICSharpSyntaxTree> SyntaxTrees { get; }

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
        /// <param name="globalUsing">Global using declarations.</param>
        /// <returns>The matching or created ICSharpFile.</returns>
        ICSharpFile RegisterFile(string file, IGlobalUsingDirectives? globalUsing = null);

        /// <summary>
        /// Register the ICSharpAssembly associated to the given assembly.
        /// </summary>
        /// <param name="assembly">The CSharp assembly.</param>
        /// <returns>The matching or created ICSharpAssembly.</returns>
        ICSharpAssembly RegisterAssembly(Assembly assembly);

        /// <summary>
        /// Register the ICSharpAssembly and load only the given types.
        /// </summary>
        /// <param name="assembly">The CSharp assembly.</param>
        /// <param name="types">The CSharp types included in the given assembly (default is null).</param>
        /// <returns>The matching or created ICSharpAssembly.</returns>
        ICSharpAssembly RegisterAssemblyTypes(Assembly assembly, IEnumerable<Type>? types = null);

        /// <summary>
        /// Register the ICSharpMetadataAssembly associated to the given assembly file.
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        ICSharpMetadataAssembly RegisterMetadataAssembly(string assemblyFile);

        /// <summary>
        /// Deep load the workspace.
        /// </summary>
        /// <returns>The resulting declaration resolver.</returns>
        IDeclarationResolver DeepLoad();
    }
}
