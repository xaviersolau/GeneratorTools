// ----------------------------------------------------------------------
// <copyright file="ICSharpFile.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// Interface describing a CSharp file.
    /// </summary>
    public interface ICSharpFile : ICSharpWorkspaceItem
    {
        /// <summary>
        /// Gets the CSharp file name.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets the CSharp file path.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Global using referential.
        /// </summary>
        IGlobalUsingDirectives GlobalUsing { get; }
    }
}
