// ----------------------------------------------------------------------
// <copyright file="ICSharpAssembly.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// Interface describing CSharp assembly.
    /// </summary>
    public interface ICSharpAssembly : ICSharpWorkspaceItem
    {
        /// <summary>
        /// Gets the assembly.
        /// </summary>
        Assembly Assembly { get; }
    }
}
