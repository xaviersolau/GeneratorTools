// ----------------------------------------------------------------------
// <copyright file="MetadataAssemblyProbe.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.CodeQuality.Test.Helpers.XUnit;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using Xunit.Abstractions;

namespace SoloX.GeneratorTools.Core.Test.Helpers.XUnit
{
    /// <summary>
    /// Probe information from Assembly Metadata.
    /// </summary>
    public static class MetadataAssemblyProbe
    {
        /// <summary>
        /// Load metadata assembly and run assert handler.
        /// </summary>
        /// <param name="testOutputHelper">XUnit Logger to use while loading assembly data.</param>
        /// <param name="assemblyFile">Assembly file to load.</param>
        /// <param name="assertHandler">Assert handler.</param>
        public static void LoadMetadataAssemblyAndAssert(ITestOutputHelper testOutputHelper, string assemblyFile, Action<IDeclarationResolver> assertHandler)
        {
            Helpers.MetadataAssemblyProbe.LoadMetadataAssemblyAndAssert(
                assemblyFile,
                assertHandler,
                services => services.AddTestLogging(testOutputHelper));
        }
    }
}