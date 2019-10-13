// ----------------------------------------------------------------------
// <copyright file="SnapshotHelper.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using SoloX.GeneratorTools.Core.Generator;
using Xunit;

namespace SoloX.GeneratorTools.Core.Test.Helpers.Snapshot
{
    public static class SnapshotHelper
    {
        public static bool IsOverWrite { get; } = true;

        public static void AssertSnapshot(string generated, string snapshotName, string location)
        {
            string snapshotFolder = Path.Combine(location, "Snapshots");
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
                    generatedRef.Replace("\r\n", "\n"),
                    generated.Replace("\r\n", "\n"));
            }
        }

        public static string GetLocationFromCallingProjectRoot(string folder)
        {
            var callingAssembly = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Assembly;
            var assemblyFolder = Path.GetDirectoryName(callingAssembly.Location);
            var projectRoot = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyFolder)));
            return folder != null ? Path.Combine(projectRoot, folder) : projectRoot;
        }
    }
}
