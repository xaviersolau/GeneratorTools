// ----------------------------------------------------------------------
// <copyright file="ICSharpLoader.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// ICSharpLoader interface used to load workspace elements.
    /// </summary>
    public interface ICSharpLoader
    {
        /// <summary>
        /// Load a given project.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="project">The project to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpProject project);

        /// <summary>
        /// Load a given file.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="file">The file to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpFile file);
    }
}
