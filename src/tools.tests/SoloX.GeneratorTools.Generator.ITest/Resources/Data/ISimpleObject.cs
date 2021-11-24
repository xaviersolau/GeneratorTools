// ----------------------------------------------------------------------
// <copyright file="ISimpleObject.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Attributes;

namespace SoloX.GeneratorTools.Generator.ITest.Resources.Data
{
    [Factory]
    public interface ISimpleObject
    {
        int Property1 { get; }

        string Property2 { get; }

        int Property3 { get; set; }

        string Property4 { get; set; }
    }
}
