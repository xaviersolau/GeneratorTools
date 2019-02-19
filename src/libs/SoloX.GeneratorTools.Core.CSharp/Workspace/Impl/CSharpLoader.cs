// ----------------------------------------------------------------------
// <copyright file="CSharpLoader.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// CSharp elements Loader implementation.
    /// </summary>
    public class CSharpLoader : ICSharpLoader
    {
        /// <inheritdoc/>
        public void Load(ICSharpWorkspace workspace, ICSharpProject project)
        {
            ((CSharpProject)project).Load(workspace);
        }

        /// <inheritdoc/>
        public void Load(ICSharpWorkspace workspace, ICSharpFile file)
        {
            ((CSharpFile)file).Load();
        }
    }
}
