// ----------------------------------------------------------------------
// <copyright file="ICSharpAssembly.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// Interface describing CSharp assembly.
    /// </summary>
    public interface ICSharpAssembly
    {
        /// <summary>
        /// Gets the assembly.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// Gets the assembly declarations.
        /// </summary>
        IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations { get; }
    }
}
