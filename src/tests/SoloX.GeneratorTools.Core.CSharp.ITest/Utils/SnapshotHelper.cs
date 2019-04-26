// ----------------------------------------------------------------------
// <copyright file="SnapshotHelper.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SoloX.GeneratorTools.Core.Generator;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Utils
{
    public static class SnapshotHelper
    {
        public static bool IsOverWrite { get; } = true;

        public static void AssertSnapshot(string generated, string snapshotName, string location)
        {
            string root = GetProjectRoot();

            string snapshotFolder = Path.Combine(root, location, "Snapshots");
            string snapshotFile = Path.Combine(snapshotFolder, $"{snapshotName}.snapshot");

            if (!Directory.Exists(snapshotFolder) && IsOverWrite)
            {
                Directory.CreateDirectory(snapshotFolder);
            }

            if (!File.Exists(snapshotFile))
            {
                if (IsOverWrite)
                {
                    File.WriteAllText(snapshotFile, generated);
                }

                Assert.Equal($"The snapshot file {snapshotName} does not exist", generated);
            }
            else
            {
                var generatedRef = File.ReadAllText(snapshotFile);

                if (IsOverWrite)
                {
                    File.WriteAllText(snapshotFile, generated);
                }

                Assert.Equal(
                    generatedRef.Replace("\r\n", "\n", StringComparison.InvariantCulture),
                    generated.Replace("\r\n", "\n", StringComparison.InvariantCulture));
            }
        }

        private static string GetProjectRoot()
        {
            var assemblyFolder = Path.GetDirectoryName(typeof(SnapshotHelper).Assembly.Location);
            var projectRoot = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyFolder)));
            return projectRoot;
        }
    }
}
