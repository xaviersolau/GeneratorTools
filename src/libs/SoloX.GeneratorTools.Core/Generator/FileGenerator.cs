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
        private string rootPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileGenerator"/> class.
        /// </summary>
        /// <param name="root">The root folder where to generate files.</param>
        public FileGenerator(string root)
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            this.rootPath = root;
        }

        /// <inheritdoc/>
        public void Generate(string location, string name, Action<TextWriter> generator)
        {
            var file = $"{name}.generated.cs";

            var folder = string.IsNullOrEmpty(location) ?
                Path.Combine(this.rootPath) :
                Path.Combine(this.rootPath, location);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var filePath = Path.Combine(folder, file);

            using (var outStream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(outStream))
            {
                generator(writer);
            }
        }

        /// <summary>
        /// Create and or cleanup the full root folder.
        /// </summary>
        public void CreateOrCleanUp()
        {
            if (Directory.Exists(this.rootPath))
            {
                Directory.Delete(this.rootPath, true);

                while (Directory.Exists(this.rootPath))
                {
                    Thread.Sleep(10);
                }
            }

            Directory.CreateDirectory(this.rootPath);
        }
    }
}
