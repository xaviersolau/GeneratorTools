// ----------------------------------------------------------------------
// <copyright file="EntityPattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Impl
{
    /// <summary>
    /// Entity implementation pattern used in the generator.
    /// </summary>
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
