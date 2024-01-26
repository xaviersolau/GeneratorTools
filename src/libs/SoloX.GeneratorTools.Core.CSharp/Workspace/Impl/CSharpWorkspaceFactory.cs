// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// CSharp Workspace factory.
    /// </summary>
    public class CSharpWorkspaceFactory : ICSharpWorkspaceFactory
    {
        private readonly IGeneratorLoggerFactory loggerFactory;

        /// <summary>
        /// Setup instance.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public CSharpWorkspaceFactory(IGeneratorLoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        public ICSharpWorkspace CreateWorkspace()
        {
            return new CSharpWorkspace(
                this.loggerFactory.CreateLogger<CSharpWorkspace>(),
                CreateWorkspaceItemFactory());
        }

        /// <inheritdoc/>
        public ICSharpWorkspace CreateWorkspace(Compilation compilation)
        {
            var ws = new CSharpWorkspace(
                this.loggerFactory.CreateLogger<CSharpWorkspace>(),
                CreateWorkspaceItemFactory(), true);

            ws.RegisterCompilation(compilation);
            return ws;
        }

        private CSharpWorkspaceItemFactory CreateWorkspaceItemFactory()
        {
            return new CSharpWorkspaceItemFactory(
                this.loggerFactory,
                new ParserDeclarationFactory(
                    new ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>(this.loggerFactory.CreateLogger<ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>>()),
                    new ParserGenericDeclarationLoader<ClassDeclarationSyntax>(this.loggerFactory.CreateLogger<ParserGenericDeclarationLoader<ClassDeclarationSyntax>>()),
                    new ParserGenericDeclarationLoader<StructDeclarationSyntax>(this.loggerFactory.CreateLogger<ParserGenericDeclarationLoader<StructDeclarationSyntax>>()),
                    new ParserGenericDeclarationLoader<RecordDeclarationSyntax>(this.loggerFactory.CreateLogger<ParserGenericDeclarationLoader<RecordDeclarationSyntax>>()),
                    new ParserEnumDeclarationLoader(this.loggerFactory.CreateLogger<ParserEnumDeclarationLoader>())),
                new ReflectionDeclarationFactory(
                    new ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>(this.loggerFactory.CreateLogger<ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>>()),
                    new ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>(this.loggerFactory.CreateLogger<ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>>()),
                    new ReflectionGenericDeclarationLoader<StructDeclarationSyntax>(this.loggerFactory.CreateLogger<ReflectionGenericDeclarationLoader<StructDeclarationSyntax>>()),
                    new ReflectionGenericDeclarationLoader<RecordDeclarationSyntax>(this.loggerFactory.CreateLogger<ReflectionGenericDeclarationLoader<RecordDeclarationSyntax>>()),
                    new ReflectionEnumDeclarationLoader(this.loggerFactory.CreateLogger<ReflectionEnumDeclarationLoader>())),
                new MetadataDeclarationFactory(
                    new MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax>(this.loggerFactory.CreateLogger<MetadataGenericDeclarationLoader<InterfaceDeclarationSyntax>>()),
                    new MetadataGenericDeclarationLoader<ClassDeclarationSyntax>(this.loggerFactory.CreateLogger<MetadataGenericDeclarationLoader<ClassDeclarationSyntax>>()),
                    new MetadataGenericDeclarationLoader<StructDeclarationSyntax>(this.loggerFactory.CreateLogger<MetadataGenericDeclarationLoader<StructDeclarationSyntax>>()),
                    new MetadataGenericDeclarationLoader<RecordDeclarationSyntax>(this.loggerFactory.CreateLogger<MetadataGenericDeclarationLoader<RecordDeclarationSyntax>>()),
                    new MetadataEnumDeclarationLoader(this.loggerFactory.CreateLogger<MetadataEnumDeclarationLoader>())));
        }
    }
}
