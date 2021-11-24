// ----------------------------------------------------------------------
// <copyright file="FileWriter.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.IO;

namespace SoloX.GeneratorTools.Core.Generator
{
    /// <summary>
    /// File generator implementation.
    /// </summary>
    public class FileWriter : IWriter
    {
        private readonly string fileSufix;
        private readonly Action<string> generateCallBack;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWriter"/> class.
        /// </summary>
        /// <param name="fileSufix">The file suffix to use (like '.cs' or '.generated.cs').</param>
        /// <param name="generateCallBack">The callback to use once a new file is generated.</param>
        public FileWriter(string fileSufix, Action<string> generateCallBack = null)
        {
            this.fileSufix = fileSufix;
            this.generateCallBack = generateCallBack;
        }

        /// <inheritdoc/>
        public void Generate(string location, string name, Action<TextWriter> generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator), $"The argument {nameof(generator)} was null.");
            }

            var file = $"{name}{this.fileSufix}";

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

            this.generateCallBack?.Invoke(filePath);
        }
    }
}
