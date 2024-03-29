﻿// ----------------------------------------------------------------------
// <copyright file="ProjectAssetsTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using System.Text.Json;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace.Assets
{
    public class ProjectAssetsTest
    {
        private const string AssetsFile = "./Resources/Workspace/Assets/project.assets.json";

        [Fact]
        public void BasicReadAssetsTest()
        {
            var assets = ReadAssets(AssetsFile);

            Assert.NotEmpty(assets.Targets);
        }

        [Fact]
        public void ReadAssetsCompileDllPathTest()
        {
            var assets = ReadAssets(AssetsFile);

            var target = Assert.Single(assets.Targets);

            var compileItemFile = Assert.Single(target.Value.GetAllPackageCompileItems(assets));

            Assert.Equal(@"newtonsoft.json/12.0.2/lib/netstandard2.0/Newtonsoft.Json.dll", compileItemFile.Replace('\\', '/'));
        }

        [Fact]
        public void ReadAssetsRuntimeDllPathTest()
        {
            var assets = ReadAssets(AssetsFile);

            var target = Assert.Single(assets.Targets);

            var compileItemFile = Assert.Single(target.Value.GetAllPackageRuntimeItems(assets));

            Assert.Equal(@"newtonsoft.json/12.0.2/lib/netstandard2.0/Newtonsoft.Json.dll", compileItemFile.Replace('\\', '/'));
        }

        private static ProjectAssets ReadAssets(string file)
        {
            var assetsJson = File.ReadAllText(file);

            var projectAssets = JsonSerializer.Deserialize<ProjectAssets>(assetsJson);

            Assert.NotNull(projectAssets);

            return projectAssets;
        }
    }
}
