// ----------------------------------------------------------------------
// <copyright file="RepeatAttribute.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Attributes
{
    /// <summary>
    /// Attribute used to tell that the pattern element must be repeated.
    /// For example use it if you want to repeat a piece of code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class RepeatAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the repeat pattern.
        /// </summary>
        public string? Pattern { get; set; }

        /// <summary>
        /// Gets or sets the repeat pattern prefix.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Gets or sets the repeat pattern suffix.
        /// </summary>
        public string? Suffix { get; set; }
    }
}
