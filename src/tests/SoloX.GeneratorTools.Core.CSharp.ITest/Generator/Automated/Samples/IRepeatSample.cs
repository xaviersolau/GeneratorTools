// ----------------------------------------------------------------------
// <copyright file="IRepeatSample.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Samples
{
    public interface IRepeatSample
    {
        string Statement1 { get; set; }

        string Statement2 { get; set; }

        string Property1 { get; }

        string Property2 { get; }
    }
}
