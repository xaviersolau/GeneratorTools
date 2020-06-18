// ----------------------------------------------------------------------
// <copyright file="RepeatAttribute.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Attributes
{
    /// <summary>
    /// Attribute used to tell that the pattern element must be repeated.
    /// For example use it if you want to repeat a piece of code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
    public sealed class RepeatAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the repeat pattern.
        /// </summary>
        public string RepeatPattern { get; set; }

        /// <summary>
        /// Gets or sets the repeat pattern prefix.
        /// </summary>
        public string RepeatPatternPrefix { get; set; }
    }
}
