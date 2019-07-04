// ----------------------------------------------------------------------
// <copyright file="LibraryAssets.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets
{
    /// <summary>
    /// Library Assets.
    /// </summary>
    public class LibraryAssets
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryAssets"/> class.
        /// </summary>
        /// <param name="name">Library name.</param>
        public LibraryAssets(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the library name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets target item path.
        /// </summary>
        public string Path { get; internal set; }
    }
}
