﻿// ----------------------------------------------------------------------
// <copyright file="IEntityBase.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Core
{
    /// <summary>
    /// IEntityBase is the base entity interface that will be:
    /// - extended by all entity interfaces.
    /// - used by the generator to find all entity to generate.
    /// </summary>
    /// <remarks>In this example, the base interface is empty but base methods and properties can be declared here.</remarks>
#pragma warning disable CA1040 // Avoid empty interfaces
    public interface IEntityBase
#pragma warning restore CA1040 // Avoid empty interfaces
    {
    }
}
