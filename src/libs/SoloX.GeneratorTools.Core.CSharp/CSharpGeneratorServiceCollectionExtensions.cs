// ----------------------------------------------------------------------
// <copyright file="CSharpGeneratorServiceCollectionExtensions.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;

namespace SoloX.GeneratorTools.Core.CSharp
{
    /// <summary>
    /// CSharp Tools Generator ServiceCollection extensions.
    /// </summary>
    public static class CSharpGeneratorServiceCollectionExtensions
    {
        /// <summary>
        /// Add dependency injections for the CSharp tools generator.
        /// </summary>
        /// <param name="services">The service collection where to setup dependencies.</param>
        /// <returns>The input services once setup is done.</returns>
        public static IServiceCollection AddCSharpToolsGenerator(this IServiceCollection services)
        {
            return services
                .AddSingleton<ICSharpFactory, CSharpFactory>()
                .AddSingleton<ICSharpLoader, CSharpLoader>()
                .AddTransient<ICSharpWorkspace, CSharpWorkspace>();
        }
    }
}
