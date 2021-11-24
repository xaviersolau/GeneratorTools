// ----------------------------------------------------------------------
// <copyright file="DependencyAttribute.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.GeneratorTools.Attributes
{
    /// <summary>
    /// Attribute to specify that the property must be initialized from dependency injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DependencyAttribute : Attribute
    {
    }
}
