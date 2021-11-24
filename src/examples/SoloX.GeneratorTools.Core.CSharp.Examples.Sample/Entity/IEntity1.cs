// ----------------------------------------------------------------------
// <copyright file="IEntity1.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Examples.Core;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Sample.Entity
{
    /// <summary>
    /// IEntity1 is the entity interface what will be used as base to generate the implementation.
    /// </summary>
    public interface IEntity1 : IEntityBase
    {
        /// <summary>
        /// Gets the propertyA that will be implemented by the generator.
        /// </summary>
        int PropertyA { get; }

        /// <summary>
        /// Gets the propertyB that will be implemented by the generator.
        /// </summary>
        string PropertyB { get; }
    }
}
