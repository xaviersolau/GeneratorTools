// ----------------------------------------------------------------------
// <copyright file="TargetAssets.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets
{
    /// <summary>
    /// Target Assets.
    /// </summary>
    public class TargetAssets
    {
        private Dictionary<string, TargetItemAssets> targetItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetAssets"/> class.
        /// </summary>
        /// <param name="name">The target name.</param>
        public TargetAssets(string name)
        {
            this.Name = name;
            this.targetItems = new Dictionary<string, TargetItemAssets>();
        }

        /// <summary>
        /// Gets target name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets target Items.
        /// </summary>
        public IReadOnlyDictionary<string, TargetItemAssets> TargetItems => this.targetItems;

        /// <summary>
        /// Gets all package compile items.
        /// </summary>
        public IEnumerable<string> AllPackageCompileItems => this.TargetItems.Values
            .Where(v => v.ItemType == "package")
            .SelectMany(
                v => v.CompileItems.Where(s => !s.EndsWith("_._", StringComparison.InvariantCulture)),
                (v, s) => Path.Combine(v.Name, s));

        internal void AddTargetItem(TargetItemAssets targetItemAssets)
        {
            this.targetItems.Add(targetItemAssets.Name, targetItemAssets);
        }
    }
}
