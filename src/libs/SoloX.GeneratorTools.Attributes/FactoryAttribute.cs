// ----------------------------------------------------------------------
// <copyright file="FactoryAttribute.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.GeneratorTools.Attributes
{
    /// <summary>
    /// Attribute to specify that the class must be created by a generated factory.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class FactoryAttribute : Attribute
    {
    }
}
