// ----------------------------------------------------------------------
// <copyright file="IToolsGenerator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

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
