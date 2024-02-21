// ----------------------------------------------------------------------
// <copyright file="CSharpGeneratorServiceCollectionExtensions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using SoloX.GeneratorTools.Core.Utils;
using SoloX.GeneratorTools.Core.CSharp.Extensions.Utils;
using System;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata;
using Microsoft.Extensions.Logging;

namespace SoloX.GeneratorTools.Core.CSharp.Extensions
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
        /// <param name="loggerFactory">Specific logger factory to use or null.</param>
        /// <returns>The input services once setup is done.</returns>
        public static IServiceCollection AddCSharpToolsGenerator(this IServiceCollection services, ILoggerFactory loggerFactory = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(ServiceDescriptor.Transient(typeof(IGeneratorLogger<>), typeof(GeneratorLogger<>)));

            return services
                .AddSingleton<
                    ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>,
                    ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>>()
                .AddSingleton<
                    ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>,
                    ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>>()
                .AddSingleton<
                    ReflectionGenericDeclarationLoader<StructDeclarationSyntax>,
                    ReflectionGenericDeclarationLoader<StructDeclarationSyntax>>()
                .AddSingleton<IReflectionDeclarationFactory, ReflectionDeclarationFactory>()
                .AddSingleton<
                    MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax>,
                    MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax>>()
                .AddSingleton<
                    MetadataGenericDeclarationLoader<ClassDeclarationSyntax>,
                    MetadataGenericDeclarationLoader<ClassDeclarationSyntax>>()
                .AddSingleton<
                    MetadataGenericDeclarationLoader<StructDeclarationSyntax>,
                    MetadataGenericDeclarationLoader<StructDeclarationSyntax>>()
                .AddSingleton<IMetadataDeclarationFactory, MetadataDeclarationFactory>()
                .AddSingleton<
                    ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>,
                    ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>>()
                .AddSingleton<
                    ParserGenericDeclarationLoader<ClassDeclarationSyntax>,
                    ParserGenericDeclarationLoader<ClassDeclarationSyntax>>()
                .AddSingleton<
                    ParserGenericDeclarationLoader<StructDeclarationSyntax>,
                    ParserGenericDeclarationLoader<StructDeclarationSyntax>>()
                .AddSingleton<IParserDeclarationFactory, ParserDeclarationFactory>()
                .AddSingleton<ICSharpWorkspaceItemFactory, CSharpWorkspaceItemFactory>()
                .AddTransient<ICSharpWorkspaceFactory, CSharpWorkspaceFactory>()
                .AddTransient<IGeneratorLoggerFactory>(
                    r =>
                    {
                        if (loggerFactory != null)
                        {
                            return new GeneratorLoggerFactory(loggerFactory);
                        }
                        return new GeneratorLoggerFactory(r.GetRequiredService<ILoggerFactory>());
                    });
        }
    }
}
