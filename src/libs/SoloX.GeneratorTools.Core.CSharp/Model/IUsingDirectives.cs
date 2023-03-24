// ----------------------------------------------------------------------
// <copyright file="IUsingDirectives.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Using directives collection.
    /// </summary>
    public interface IUsingDirectives
    {
        /// <summary>
        /// Gets the usings for the current declaration.
        /// </summary>
        IReadOnlyList<string> Usings { get; }
    }
}
