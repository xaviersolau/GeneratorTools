// ----------------------------------------------------------------------
// <copyright file="IEntity1.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Model
{
    /// <summary>
    /// IEntity1 is the entity interface what will be used as base to generate the implementation.
    /// </summary>
    public interface IEntity1 : IEntityBase
    {
        /// <summary>
        /// Gets the property that will be implemented by the generator.
        /// </summary>
        int PropertyA { get; }
    }
}
