// ----------------------------------------------------------------------
// <copyright file="IGlobalUsingDirectives.cs" company="Xavier Solau">
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
    public interface IGlobalUsingDirectives
    {
        /// <summary>
        /// All global usings.
        /// </summary>
        IReadOnlyList<string> Usings { get; }

        /// <summary>
        /// Register a global using.
        /// </summary>
        /// <param name="usingDirective">Using directive to register.</param>
        void Register(string usingDirective);
    }
}
