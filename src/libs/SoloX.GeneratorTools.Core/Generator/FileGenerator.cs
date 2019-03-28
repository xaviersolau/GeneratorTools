// ----------------------------------------------------------------------
// <copyright file="FileGenerator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SoloX.GeneratorTools.Core.Generator
{
    /// <summary>
    /// File generator implementation.
    /// </summary>
    public class FileGenerator : IGenerator
    {
        /// <inheritdoc/>
        public void Generate(string location, string name, Action<TextWriter> generator)
        {
            var file = $"{name}.generated.cs";

            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            var filePath = Path.Combine(location, file);

            using (var outStream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(outStream))
            {
                generator(writer);
            }
        }
    }
}
