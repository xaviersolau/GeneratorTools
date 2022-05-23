// ----------------------------------------------------------------------
// <copyright file="ICSharpMetadataAssembly.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// Interface describing CSharp metadata assembly.
    /// </summary>
    public interface ICSharpMetadataAssembly : ICSharpWorkspaceItem
    {
        /// <summary>
        /// Gets the assembly path.
        /// </summary>
        string AssemblyPath { get; }
    }
}
