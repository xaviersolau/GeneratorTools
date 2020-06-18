// ----------------------------------------------------------------------
// <copyright file="IToolsGenerator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Generator
{
    /// <summary>
    /// Tools generator interface.
    /// </summary>
    public interface IToolsGenerator
    {
        /// <summary>
        /// Apply the generator tools on the given project.
        /// </summary>
        /// <param name="projectFile">Project file.</param>
        void Generate(string projectFile);
    }
}
