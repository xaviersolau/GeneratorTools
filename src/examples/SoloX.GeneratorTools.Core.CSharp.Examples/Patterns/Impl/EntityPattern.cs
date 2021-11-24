﻿// ----------------------------------------------------------------------
// <copyright file="EntityPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Impl
{
    /// <summary>
    /// Entity implementation pattern used in the generator.
    /// </summary>
    [GeneratedCode("TOOL", "VERSION")]
    public partial class EntityPattern : IEntityPattern
    {

        /// <inheritdoc/>
        public object PropertyPattern
        { get; set; }

        /// <summary>
        /// Example of a method that will be defined in the generated entity implementation.
        /// </summary>
        public static void AMethodToGenerate()
        {
            Console.Out.WriteLine("Hello!!");
        }
    }
}
