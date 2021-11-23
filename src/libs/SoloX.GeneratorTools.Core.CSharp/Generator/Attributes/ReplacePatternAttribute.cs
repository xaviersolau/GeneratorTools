// ----------------------------------------------------------------------
// <copyright file="ReplacePatternAttribute.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Attributes
{
    /// <summary>
    /// Replace Pattern attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ReplacePatternAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplacePatternAttribute"/> class.
        /// </summary>
        /// <param name="replacePatternHandlerType">The type of the replace pattern handler factory type to use.</param>
        public ReplacePatternAttribute(Type replacePatternHandlerType)
        {
            this.ReplacePatternHandlerFactory = (IReplacePatternHandlerFactory)Activator.CreateInstance(replacePatternHandlerType);
        }

        /// <summary>
        /// Gets the selector.
        /// </summary>
        public IReplacePatternHandlerFactory ReplacePatternHandlerFactory { get; }
    }
}
