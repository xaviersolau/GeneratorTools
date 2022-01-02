// ----------------------------------------------------------------------
// <copyright file="MemoryWriter.cs" company="Xavier Solau">
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
    /// Memory generator implementation.
    /// </summary>
    public class MemoryWriter : IWriter
    {
        private readonly string fileSufix;
        private readonly Action<string, TextReader> generatedCallBack;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryWriter"/> class.
        /// </summary>
        /// <param name="fileSufix">The file suffix to use (like '.cs' or '.generated.cs').</param>
        /// <param name="generatedCallBack">The callback to use once a new file is generated.</param>
        public MemoryWriter(string fileSufix, Action<string, TextReader> generatedCallBack)
        {
            this.fileSufix = fileSufix;
            this.generatedCallBack = generatedCallBack;
        }

        /// <inheritdoc/>
        public void Generate(string location, string name, Action<TextWriter> generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator), $"The argument {nameof(generator)} was null.");
            }

            var file = $"{name}{this.fileSufix}";

            var filePath = Path.Combine(location, file);

            var memoryStream = new MemoryStream();

            using var writer = new StreamWriter(memoryStream);

            generator(writer);

            writer.Flush();

            memoryStream.Position = 0;

            using var reader = new StreamReader(memoryStream);

            this.generatedCallBack.Invoke(filePath, reader);
        }
    }
}
