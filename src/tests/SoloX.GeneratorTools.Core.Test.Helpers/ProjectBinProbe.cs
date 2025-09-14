// ----------------------------------------------------------------------
// <copyright file="ProjectBinProbe.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;

namespace SoloX.GeneratorTools.Core.Test.Helpers
{
    /// <summary>
    /// Probe information from Project Bin folder.
    /// </summary>
    public static class ProjectBinProbe
    {
        /// <summary>
        /// Get configuration name from the assembly file that contain the given type.
        /// For example if the assembly is generated in the location:
        /// YourProject\bin\Debug\net9.0\YourProject.dll
        /// The configuration would be "Debug".
        /// </summary>
        /// <typeparam name="TAssemblyType">Any type defined in the assembly to probe.</typeparam>
        /// <returns>Configuration name.</returns>
        public static string GetConfiguration<TAssemblyType>()
        {
            var location = Path.GetDirectoryName(typeof(TAssemblyType).Assembly.Location);

            return Path.GetFileName(Path.GetDirectoryName(location)!);
        }

        /// <summary>
        /// Get framework name from the assembly file that contain the given type.
        /// For example if the assembly is generated in the location:
        /// YourProject\bin\Debug\net9.0\YourProject.dll
        /// The framework would be "net9.0".
        /// </summary>
        /// <typeparam name="TAssemblyType">Any type defined in the assembly to probe.</typeparam>
        /// <returns>Configuration name.</returns>
        public static string GetFramework<TAssemblyType>()
        {
            var location = Path.GetDirectoryName(typeof(TAssemblyType).Assembly.Location);

            return Path.GetFileName(location!);
        }
    }
}
