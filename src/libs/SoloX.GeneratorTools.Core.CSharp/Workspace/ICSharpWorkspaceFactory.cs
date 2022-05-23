// ----------------------------------------------------------------------
// <copyright file="ICSharpWorkspaceFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// CSharp Workspace Factory.
    /// </summary>
    public interface ICSharpWorkspaceFactory
    {
        /// <summary>
        /// Create a new workspace instance.
        /// </summary>
        /// <returns>The created workspace.</returns>
        ICSharpWorkspace CreateWorkspace();

        /// <summary>
        /// Create a new workspace instance associated to the given Compilation.
        /// </summary>
        /// <returns>The created workspace.</returns>
        ICSharpWorkspace CreateWorkspace(Compilation compilation);
    }
}
