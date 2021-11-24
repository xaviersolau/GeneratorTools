// ----------------------------------------------------------------------
// <copyright file="ToolsGeneratorExtensions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SoloX.GeneratorTools.Core.CSharp;
using SoloX.GeneratorTools.Generator.Impl;

namespace SoloX.GeneratorTools.Generator
{
    /// <summary>
    /// ToolsGenerator ServiceCollection extensions.
    /// </summary>
    public static class ToolsGeneratorExtensions
    {
        /// <summary>
        /// Add dependency injections for the state generator.
        /// </summary>
        /// <param name="services">The service collection where to setup dependencies.</param>
        /// <returns>The input services once setup is done.</returns>
        public static IServiceCollection AddToolsGenerator(this IServiceCollection services)
        {
            return services
                .AddCSharpToolsGenerator()
                .AddTransient<IToolsGenerator, ToolsGenerator>();
        }
    }
}
