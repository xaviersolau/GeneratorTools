// ----------------------------------------------------------------------
// <copyright file="CSharpFactory.cs" company="SoloX Software">
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
    /// ICSharpFactory implementation.
    /// </summary>
    public class CSharpFactory : ICSharpFactory
    {
        /// <inheritdoc/>
        public ICSharpFile CreateFile(string file)
        {
            return new CSharpFile(file);
        }

        /// <inheritdoc/>
        public ICSharpProject CreateProject(string file)
        {
            return new CSharpProject(file);
        }
    }
}
