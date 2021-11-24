// ----------------------------------------------------------------------
// <copyright file="IModelPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Examples.Core;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf
{
    /// <summary>
    /// Model pattern interface used by the Model pattern implementation.
    /// </summary>
    public interface IModelPattern : IModelBase
    {
        /// <summary>
        /// Gets or sets property declaration pattern.
        /// </summary>
        object PropertyPattern { get; set; }
    }
}
