// ----------------------------------------------------------------------
// <copyright file="FileGeneratorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using SoloX.GeneratorTools.Core.Generator;
using Xunit;

namespace SoloX.GeneratorTools.Core.UTest.Generator
{
    public class FileGeneratorTest
    {
        [Fact]
        public void ItMustGenerateAFile()
        {
            var txt = "some text";

            var location = Path.GetTempPath();

            var generator = new FileWriter(".txt");

            generator.Generate(location, "testfile", (w) => w.Write(txt));

            var expectedFile = Path.Combine(location, "testfile.txt");

            Assert.True(File.Exists(expectedFile));

            Assert.Equal(txt, File.ReadAllText(expectedFile));
        }
    }
}
