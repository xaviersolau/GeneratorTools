// ----------------------------------------------------------------------
// <copyright file="ICSharpWorkspaceItem.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// CSharp Workspace Item interface.
    /// </summary>
    public interface ICSharpWorkspaceItem
    {
        /// <summary>
        /// Gets the assembly declarations.
        /// </summary>
        IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations { get; }
    }
}
