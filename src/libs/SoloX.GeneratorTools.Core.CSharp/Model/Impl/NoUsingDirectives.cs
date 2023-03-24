// ----------------------------------------------------------------------
// <copyright file="NoUsingDirectives.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Empty using directives.
    /// </summary>
    public class NoUsingDirectives : IUsingDirectives
    {
        /// <summary>
        /// Default instance.
        /// </summary>
        public static readonly NoUsingDirectives Instance = new NoUsingDirectives();

        /// <inheritdoc/>
        public IReadOnlyList<string> Usings => Array.Empty<string>();
    }
}
