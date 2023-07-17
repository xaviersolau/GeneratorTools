// ----------------------------------------------------------------------
// <copyright file="PatternAttribute.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Attributes
{
    /// <summary>
    /// APatternAttribute
    /// </summary>
    public abstract class PatternAttribute : Attribute
    {
        /// <summary>
        /// Gets the selector.
        /// </summary>
        public ISelector Selector { get; protected set; }
    }

    /// <summary>
    /// Attribute used to tell that the class/interface/struct/enum is a pattern.
    /// </summary>
    /// <typeparam name="TSelector"></typeparam>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct |
        AttributeTargets.Enum | AttributeTargets.Property)]
    public sealed class PatternAttribute<TSelector> : PatternAttribute where TSelector : class, ISelector, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternAttribute"/> class.
        /// </summary>
        public PatternAttribute()
        {
            this.Selector = new TSelector();
        }

        /// <summary>
        /// Gets the selector type.
        /// </summary>
        public Type SelectorType => typeof(TSelector);
    }
}
