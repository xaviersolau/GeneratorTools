// ----------------------------------------------------------------------
// <copyright file="ProjectAssets.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets
{
    /// <summary>
    /// Project Assets.
    /// </summary>
    [JsonConverter(typeof(JsonProjectAssetsConverter))]
    public class ProjectAssets
    {
        private Dictionary<string, TargetAssets> targets = new Dictionary<string, TargetAssets>();
        private Dictionary<string, LibraryAssets> libraries = new Dictionary<string, LibraryAssets>();
        private List<string> packageFolder = new List<string>();

        /// <summary>
        /// Gets or sets version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets targets.
        /// </summary>
        public IReadOnlyDictionary<string, TargetAssets> Targets => this.targets;

        /// <summary>
        /// Gets libraries.
        /// </summary>
        public IReadOnlyDictionary<string, LibraryAssets> Libraries => this.libraries;

        /// <summary>
        /// Gets the package folders.
        /// </summary>
        public IReadOnlyList<string> PackageFolder => this.packageFolder;

        internal void AddTarget(TargetAssets targetAssets)
        {
            this.targets.Add(targetAssets.Name, targetAssets);
        }

        internal void AddLibrary(LibraryAssets libraryAssets)
        {
            this.libraries.Add(libraryAssets.Name, libraryAssets);
        }

        internal void AddPackageFolder(string folder)
        {
            this.packageFolder.Add(folder);
        }
    }
}
