// ----------------------------------------------------------------------
// <copyright file="ClassWithArrayProperties.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic
{
    public class ClassWithArrayProperties
    {
#pragma warning disable CA1819 // Properties should not return arrays
        public SimpleClass[] PropertyClass { get; set; }

        public int[] PropertyInt { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
    }
}
