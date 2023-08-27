// ----------------------------------------------------------------------
// <copyright file="IAttributeSelectorSample.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.ComponentModel;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Samples
{
    public interface IAttributeSelectorSample
    {
        [Description("...")]
        string PropertyWithAttribute1 { get; set; }

        [Description("...")]
        string PropertyWithAttribute2 { get; set; }

        string PropertyWithNoAttribute3 { get; }
        string PropertyWithNoAttribute4 { get; }
        string PropertyWithNoAttribute5 { get; }

    }
}
