// ----------------------------------------------------------------------
// <copyright file="PatternAttributedInterface.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Interfaces
{
#pragma warning disable CA1040 // Avoid empty interfaces
#pragma warning disable CA1715 // Identifiers should have correct prefix
#pragma warning disable IDE1006 // Naming Styles
    [Pattern<AttributeSelector<Attribute>>]
    public interface PatternAttributedInterface
    {
    }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CA1715 // Identifiers should have correct prefix
#pragma warning restore CA1040 // Avoid empty interfaces
}
