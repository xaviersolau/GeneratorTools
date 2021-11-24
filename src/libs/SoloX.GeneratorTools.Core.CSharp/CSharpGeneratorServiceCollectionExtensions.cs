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
                .AddSingleton<
                    ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>,
                    ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>>()
                .AddSingleton<
                    ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>,
                    ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>>()
                .AddSingleton<
                    ReflectionGenericDeclarationLoader<StructDeclarationSyntax>,
                    ReflectionGenericDeclarationLoader<StructDeclarationSyntax>>()
                .AddSingleton<
                    ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>,
                    ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>>()
                .AddSingleton<
                    ParserGenericDeclarationLoader<ClassDeclarationSyntax>,
                    ParserGenericDeclarationLoader<ClassDeclarationSyntax>>()
                .AddSingleton<
                    ParserGenericDeclarationLoader<StructDeclarationSyntax>,
                    ParserGenericDeclarationLoader<StructDeclarationSyntax>>()
                .AddSingleton<IDeclarationFactory, DeclarationFactory>()
                .AddSingleton<ICSharpFactory, CSharpFactory>()
                .AddSingleton<ICSharpLoader, CSharpLoader>()
                .AddTransient<ICSharpWorkspace, CSharpWorkspace>();
        }
    }
}
