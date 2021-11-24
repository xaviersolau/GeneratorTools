﻿// ----------------------------------------------------------------------
// <copyright file="IWriter.cs" company="Xavier Solau">
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
    /// Writer interface.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Process the given generator.
        /// </summary>
        /// <param name="location">The location where to generate.</param>
        /// <param name="name">The element name to generate. (The file name in the case of a file generation.)</param>
        /// <param name="generator">The generator to process.</param>
        void Generate(string location, string name, Action<TextWriter> generator);
    }
}
