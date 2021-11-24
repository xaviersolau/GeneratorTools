// ----------------------------------------------------------------------
// <copyright file="TargetAssets.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets
{
    /// <summary>
    /// Target Assets.
    /// </summary>
    public class TargetAssets
    {
        private readonly Dictionary<string, TargetItemAssets> targetItems;

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
        /// Gets all package runtime items.
        /// </summary>
        /// <param name="projectAssets">The project assets.</param>
        /// <returns>All package runtime items.</returns>
        public IEnumerable<string> GetAllPackageRuntimeItems(ProjectAssets projectAssets)
        {
            return this.GetAllPackageDllItems(projectAssets, item => item.RuntimeItems);
        }

        /// <summary>
        /// Gets all package compile items.
        /// </summary>
        /// <param name="projectAssets">The project assets.</param>
        /// <returns>All package compile items.</returns>
        public IEnumerable<string> GetAllPackageCompileItems(ProjectAssets projectAssets)
        {
            return this.GetAllPackageDllItems(projectAssets, item => item.CompileItems);
        }

        internal void AddTargetItem(TargetItemAssets targetItemAssets)
        {
            this.targetItems.Add(targetItemAssets.Name, targetItemAssets);
        }

        private static void LoadItems(
            ProjectAssets projectAssets,
            Dictionary<string, TargetItemAssets> map,
            TargetItemAssets item,
            HashSet<string> compileItems,
            HashSet<TargetItemAssets> loaded,
            Func<TargetItemAssets, IEnumerable<string>> itemsGetter)
        {
            loaded.Add(item);

            if (item.Dependencies != null)
            {
                foreach (var dependency in item.Dependencies)
                {
                    // var depItem = map[$"{dependency.Key}/{dependency.Value}"];
                    var depItem = map[$"{dependency.Key}"];
                    if (!loaded.Contains(depItem))
                    {
                        LoadItems(projectAssets, map, depItem, compileItems, loaded, itemsGetter);
                    }
                }
            }

            var dllItems = itemsGetter(item);
            if (dllItems != null)
            {
                foreach (var dllItem in dllItems.Where(s => !s.EndsWith("_._", StringComparison.InvariantCulture)))
                {
                    compileItems.Add(Path.Combine(projectAssets.Libraries[item.Name].Path, dllItem));
                }
            }
        }

        private IEnumerable<string> GetAllPackageDllItems(
            ProjectAssets projectAssets,
            Func<TargetItemAssets, IEnumerable<string>> itemsGetter)
        {
            if (projectAssets == null)
            {
                throw new ArgumentNullException(nameof(projectAssets));
            }

            var map = new Dictionary<string, TargetItemAssets>();
            foreach (var item in this.GetAllTargetItemAssets())
            {
                // map.Add(item.Name, item);
                map.Add(item.Name.Split('/').First(), item);
            }

            var compileItems = new HashSet<string>();
            var loaded = new HashSet<TargetItemAssets>();

            foreach (var item in map.Values)
            {
                LoadItems(projectAssets, map, item, compileItems, loaded, itemsGetter);
            }

            return compileItems;
        }

        /// <summary>
        /// Gets all target item assets.
        /// </summary>
        /// <returns>All target item assets.</returns>
        private IEnumerable<TargetItemAssets> GetAllTargetItemAssets()
            => this.TargetItems.Values.Where(v => v.ItemType == "package");
    }
}
