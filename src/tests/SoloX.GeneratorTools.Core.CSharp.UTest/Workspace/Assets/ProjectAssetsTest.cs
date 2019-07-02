// ----------------------------------------------------------------------
// <copyright file="ProjectAssetsTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace.Assets
{
    public class ProjectAssetsTest
    {
        [Fact]
        public void ReadAssetsTest()
        {
            var file = "./Resources/Workspace/Assets/project.assets.json";

            var assetsJson = File.ReadAllText(file);

            var projectAssets = JsonConvert.DeserializeObject<ProjectAssets>(assetsJson);

            Assert.NotNull(projectAssets);
        }
    }
}
