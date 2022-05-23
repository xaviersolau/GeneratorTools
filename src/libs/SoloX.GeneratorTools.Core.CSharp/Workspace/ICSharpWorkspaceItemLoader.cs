// ----------------------------------------------------------------------
// <copyright file="ICSharpWorkspaceItemLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// CSharpItemLoader allows to load a C# workspace item.
    /// </summary>
    /// <typeparam name="TItem">C# workspace item type.</typeparam>
    public interface ICSharpWorkspaceItemLoader<TItem>
    {
        /// <summary>
        /// Get the Item to load.
        /// </summary>
        TItem WorkspaceItem { get; }

        /// <summary>
        /// Load the Item.
        /// </summary>
        /// <param name="workspace">Workspace to load from.</param>
        void Load(ICSharpWorkspace workspace);
    }
}
