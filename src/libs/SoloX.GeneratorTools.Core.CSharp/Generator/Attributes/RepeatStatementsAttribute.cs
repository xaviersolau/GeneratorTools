// ----------------------------------------------------------------------
// <copyright file="RepeatStatementsAttribute.cs" company="Xavier Solau">
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
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RepeatStatementsAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the repeat pattern.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Pack the method statement.
        /// For example use it if all method statements must be taken as a unit of code and must not be considered in detail.
        /// </summary>
        public bool PackStatement { get; set; }
    }
}
