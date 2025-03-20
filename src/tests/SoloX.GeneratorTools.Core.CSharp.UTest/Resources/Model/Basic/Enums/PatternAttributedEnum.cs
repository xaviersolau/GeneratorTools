// ----------------------------------------------------------------------
// <copyright file="PatternAttributedEnum.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Enums
{
    [PatternAttribute<AttributeSelector<Attribute>>]
#pragma warning disable CA1008 // Enums should have zero value
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public enum PatternAttributedEnum
    {
    }
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
#pragma warning restore CA1008 // Enums should have zero value
}
