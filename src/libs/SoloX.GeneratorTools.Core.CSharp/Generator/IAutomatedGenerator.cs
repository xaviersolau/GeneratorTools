// ----------------------------------------------------------------------
// <copyright file="IAutomatedGenerator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using SoloX.GeneratorTools.Core.CSharp.Workspace;

namespace SoloX.GeneratorTools.Core.CSharp.Generator
{
    /// <summary>
    /// Automated generator Interface.
    /// </summary>
    public interface IAutomatedGenerator
    {
        /// <summary>
        /// Add name-spaces to the using ignore list.
        /// </summary>
        /// <param name="usingToIgnore">The name-spaces to add to the using ignore list.</param>
        void AddIgnoreUsing(params string[] usingToIgnore);

        /// <summary>
        /// Generate the implementation of the given interface declaration.
        /// </summary>
        /// <param name="files">The files where to apply the generator.</param>
        /// <returns>The generated items.</returns>
        IEnumerable<IGeneratedItem> Generate(IEnumerable<ICSharpFile> files);
    }
}
