// ----------------------------------------------------------------------
// <copyright file="SimpleEnum.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Enums
{
#pragma warning disable CA1008 // Enums should have zero value
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    [Flags]
    public enum SimpleEnum
    {
    }
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
#pragma warning restore CA1008 // Enums should have zero value
}
