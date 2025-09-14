// ----------------------------------------------------------------------
// <copyright file="SampleUsingLocalizerAssembly.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.Localization;

namespace SoloX.GeneratorTools.Core.CSharp.Sample3
{
    /// <summary>
    /// Test SampleUsingLocalizerAssembly class.
    /// </summary>
    public class SampleUsingLocalizerAssembly
    {
        /// <summary>
        /// Property with IStringLocalizer to make dependency on Microsoft.Extensions.Localization.Abstractions assembly.
        /// </summary>
        public IStringLocalizer Localizer { get; set; }
    }
}
