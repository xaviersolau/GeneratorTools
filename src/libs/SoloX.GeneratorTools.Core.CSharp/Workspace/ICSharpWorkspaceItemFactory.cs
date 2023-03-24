// ----------------------------------------------------------------------
// <copyright file="ICSharpWorkspaceItemFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using System.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// CSharp Workspace Item factory.
    /// </summary>
    public interface ICSharpWorkspaceItemFactory
    {
        /// <summary>
        /// Create a ICSharpProject.
        /// </summary>
        /// <param name="file">The project file.</param>
        /// <returns>The created project object.</returns>
        ICSharpWorkspaceItemLoader<ICSharpProject> CreateProject(string file);

        /// <summary>
        /// Create a ICSharpFile.
        /// </summary>
        /// <param name="file">The CSharp file.</param>
        /// <param name="globalUsing">Global using referential.</param>
        /// <returns>The created file object.</returns>
        ICSharpWorkspaceItemLoader<ICSharpFile> CreateFile(string file, IGlobalUsingDirectives globalUsing);

        /// <summary>
        /// Create a ICSharpAssembly.
        /// </summary>
        /// <param name="assembly">The CSharp assembly.</param>
        /// <returns>The created assembly object.</returns>
        ICSharpWorkspaceItemLoader<ICSharpAssembly> CreateAssembly(Assembly assembly);

        /// <summary>
        /// Create a ICSharpMetadataAssembly.
        /// </summary>
        /// <param name="assemblyFile">The CSharp assembly file.</param>
        /// <returns>The created metadata assembly object.</returns>
        ICSharpWorkspaceItemLoader<ICSharpMetadataAssembly> CreateMetadataAssembly(string assemblyFile);

        /// <summary>
        /// Create a ICSharpSyntaxTree.
        /// </summary>
        /// <param name="syntaxTree">The CSharp syntax tree.</param>
        /// <param name="globalUsing">Global using referential.</param>
        /// <returns>The created syntax tree object.</returns>
        ICSharpWorkspaceItemLoader<ICSharpSyntaxTree> CreateSyntaxTree(SyntaxTree syntaxTree, IGlobalUsingDirectives globalUsing);
    }
}
