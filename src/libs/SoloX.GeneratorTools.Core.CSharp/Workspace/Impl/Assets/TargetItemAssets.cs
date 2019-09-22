// ----------------------------------------------------------------------
// <copyright file="TargetItemAssets.cs" company="SoloX Software">
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
    /// Target item Assets.
    /// </summary>
    public class TargetItemAssets
    {
        private List<string> compileItems;
        private List<string> runtimeItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetItemAssets"/> class.
        /// </summary>
        /// <param name="name">The name of the target item.</param>
        public TargetItemAssets(string name)
        {
            this.Name = name;
            this.compileItems = new List<string>();
            this.runtimeItems = new List<string>();
        }

        /// <summary>
        /// Gets target item name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets item type.
        /// </summary>
        public string ItemType { get; internal set; }

        /// <summary>
        /// Gets Framework.
        /// </summary>
        public string Framework { get; internal set; }

        /// <summary>
        /// Gets dependencies.
        /// </summary>
        public IReadOnlyDictionary<string, string> Dependencies { get; private set; }

        /// <summary>
        /// Gets compile items.
        /// </summary>
        public IReadOnlyList<string> CompileItems => this.compileItems;

        /// <summary>
        /// Gets runtime items.
        /// </summary>
        public IReadOnlyList<string> RuntimeItems => this.runtimeItems;

        internal void SetDependencies(Dictionary<string, string> dependencies)
        {
            this.Dependencies = dependencies;
        }

        internal void AddCompile(string compileItem)
        {
            this.compileItems.Add(compileItem);
        }

        internal void AddRuntime(string runtimeItem)
        {
            this.runtimeItems.Add(runtimeItem);
        }
    }
}
