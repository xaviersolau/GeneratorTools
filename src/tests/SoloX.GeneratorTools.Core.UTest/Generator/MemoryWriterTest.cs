// ----------------------------------------------------------------------
// <copyright file="MemoryWriterTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.Generator;
using System.IO;
using Xunit;

namespace SoloX.GeneratorTools.Core.UTest.Generator
{
    public class MemoryWriterTest
    {
        [Fact]
        public void ItMustGenerateAFile()
        {
            var text = "some text";
            var location = "someLocation";
            var fileName = "testfile";
            var fileExt = ".txt";

            var expectedLocation = location + Path.DirectorySeparatorChar + fileName + fileExt;

            var generatedLocation = string.Empty;
            var generatedText = string.Empty;

            var generator = new MemoryWriter(fileExt, (location, reader) =>
            {
                generatedLocation = location;
                generatedText = reader.ReadToEnd();
            });

            generator.Generate(location, fileName, (w) => w.Write(text));

            Assert.Equal(text, generatedText);
            Assert.Equal(expectedLocation, generatedLocation);
        }
    }
}
