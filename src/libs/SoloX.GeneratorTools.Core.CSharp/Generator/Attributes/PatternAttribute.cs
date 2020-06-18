// ----------------------------------------------------------------------
// <copyright file="PatternAttribute.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Attributes
{
    /// <summary>
    /// Attribute used to tell that the class/interface/struct/enum is a pattern.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct |
        AttributeTargets.Enum | AttributeTargets.Property)]
    public sealed class PatternAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternAttribute"/> class.
        /// </summary>
        /// <param name="selectorType">The type of the selector to use to find where the pattern must be applied.</param>
        public PatternAttribute(Type selectorType)
        {
            this.Selector = (ISelector)Activator.CreateInstance(selectorType);
        }

        /// <summary>
        /// Gets the selector.
        /// </summary>
        public ISelector Selector { get; }
    }
}
