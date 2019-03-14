// ----------------------------------------------------------------------
// <copyright file="IEntityPattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Examples.Sample;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf
{
    /// <summary>
    /// Entity interface pattern used by the implementation pattern.
    /// </summary>
    public interface IEntityPattern : IEntityBase
    {
        /// <summary>
        /// Gets or sets property declaration pattern.
        /// </summary>
        object PropertyPattern { get; set; }
    }
}
