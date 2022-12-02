// ----------------------------------------------------------------------
// <copyright file="GenericClassWithArrayProperties2.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic
{
    public class GenericClassWithArrayProperties2<T>
    {
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable CA1814
        public T[,] Property { get; set; }
#pragma warning restore CA1814
#pragma warning restore CA1819 // Properties should not return arrays
    }
}
