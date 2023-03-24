// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceItemFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.Utils;
using System.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// ICSharpWorkspaceItemFactory implementation.
    /// </summary>
    public class CSharpWorkspaceItemFactory : ICSharpWorkspaceItemFactory
    {
        private readonly IGeneratorLoggerFactory loggerFactory;
        private readonly IParserDeclarationFactory parserDeclarationFactory;
        private readonly IReflectionDeclarationFactory reflectionDeclarationFactory;
        private readonly IMetadataDeclarationFactory metadataDeclarationFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpWorkspaceItemFactory"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger provider.</param>
        /// <param name="parserDeclarationFactory">The declaration factory to use to create declaration instances.</param>
        public CSharpWorkspaceItemFactory(IGeneratorLoggerFactory loggerFactory,
            IParserDeclarationFactory parserDeclarationFactory,
            IReflectionDeclarationFactory reflectionDeclarationFactory,
            IMetadataDeclarationFactory metadataDeclarationFactory)
        {
            this.loggerFactory = loggerFactory;
            this.parserDeclarationFactory = parserDeclarationFactory;
            this.reflectionDeclarationFactory = reflectionDeclarationFactory;
            this.metadataDeclarationFactory = metadataDeclarationFactory;
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpAssembly> CreateAssembly(Assembly assembly)
        {
            return new CSharpAssembly(
                this.loggerFactory.CreateLogger<CSharpAssembly>(),
                this.reflectionDeclarationFactory,
                assembly);
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpFile> CreateFile(string file, IGlobalUsingDirectives globalUsing)
        {
            return new CSharpFile(file, this.parserDeclarationFactory, globalUsing);
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpMetadataAssembly> CreateMetadataAssembly(string assemblyFile)
        {
            return new CSharpMetadataAssembly(
                this.loggerFactory.CreateLogger<CSharpMetadataAssembly>(),
                this.metadataDeclarationFactory,
                assemblyFile);
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpProject> CreateProject(string file)
        {
            return new CSharpProject(file);
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpSyntaxTree> CreateSyntaxTree(SyntaxTree syntaxTree, IGlobalUsingDirectives globalUsing)
        {
            return new CSharpSyntaxTree(syntaxTree, this.parserDeclarationFactory, globalUsing);
        }
    }
}
