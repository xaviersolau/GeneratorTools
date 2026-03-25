// ----------------------------------------------------------------------
// <copyright file="IEntityPattern.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Examples.Core;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf
{
    /// <summary>
    /// Entity pattern interface used by the Entity pattern implementation.
    /// </summary>
    internal interface IEntityPattern : IEntityBase
    {
        /// <summary>
        /// Gets or sets property declaration pattern.
        /// </summary>
        object PropertyPattern { get; set; }
    }
}
