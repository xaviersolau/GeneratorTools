// ----------------------------------------------------------------------
// <copyright file="IObjectPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

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
        [Pattern<ReadOnlyPropertySelector>]
        object SomeReadOnlyValue { get; }

        /// <summary>
        /// Gets or sets SomeValue.
        /// </summary>
        [Pattern<ReadWritePropertySelector>]
        object SomeValue { get; set; }
    }
}
