// ----------------------------------------------------------------------
// <copyright file="SnapshotWriter.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SoloX.GeneratorTools.Core.Generator;

namespace SoloX.GeneratorTools.Core.Test.Helpers.Snapshot
{
    public class SnapshotWriter : IWriter
    {
        private Dictionary<string, string> generated = new Dictionary<string, string>();

        public IReadOnlyDictionary<string, string> Generated => this.generated;

        public void Generate(string path, string name, Action<TextWriter> generator)
        {
            using (var buffer = new MemoryStream())
            using (var writer = new StreamWriter(buffer))
            {
                generator(writer);
                writer.Flush();

                buffer.Position = 0;
                using (var reader = new StreamReader(buffer))
                {
                    this.generated.Add(name, reader.ReadToEnd());
                }
            }
        }

        public string GetAllGenerated()
        {
            var txt = new StringBuilder();
            foreach (var item in this.Generated)
            {
                txt.AppendLine("---------------------");
                txt.AppendLine(item.Key);
                txt.AppendLine("---------------------");
                txt.AppendLine(item.Value);
            }

            return txt.ToString();
        }
    }
}
