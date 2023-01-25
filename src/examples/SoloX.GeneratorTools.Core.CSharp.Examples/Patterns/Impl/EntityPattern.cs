// ----------------------------------------------------------------------
// <copyright file="EntityPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using SoloX.GeneratorTools.Core.CSharp.Examples.Core;
using SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Impl
{
    /// <summary>
    /// Entity implementation pattern used in the generator.
    /// </summary>
    [GeneratedCode("TOOL", "VERSION")]
    [Pattern(typeof(InterfaceBasedOnSelector<IEntityBase>))]
    [Repeat(Pattern = nameof(IEntityPattern), Prefix = "I")]
    public partial class EntityPattern : IEntityPattern
    {
        /// <inheritdoc/>
        [Repeat(Pattern = nameof(IEntityPattern.PropertyPattern))]
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
