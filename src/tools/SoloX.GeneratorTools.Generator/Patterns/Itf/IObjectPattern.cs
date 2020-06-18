// ----------------------------------------------------------------------
// <copyright file="IObjectPattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;

namespace SoloX.GeneratorTools.Generator.Patterns.Itf
{
    /// <summary>
    /// Object interface.
    /// </summary>
    public interface IObjectPattern
    {
        /// <summary>
        /// Gets SomeReadOnlyValue.
        /// </summary>
        [Pattern(typeof(ReadOnlyPropertySelector))]
        object SomeReadOnlyValue { get; }

        /// <summary>
        /// Gets or sets SomeValue.
        /// </summary>
        [Pattern(typeof(ReadWritePropertySelector))]
        object SomeValue { get; set; }
    }
}
