// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspaceItemFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
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
        private readonly IDeclarationFactory declarationFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpWorkspaceItemFactory"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger provider.</param>
        /// <param name="declarationFactory">The declaration factory to use to create declaration instances.</param>
        public CSharpWorkspaceItemFactory(IGeneratorLoggerFactory loggerFactory, IDeclarationFactory declarationFactory)
        {
            this.loggerFactory = loggerFactory;
            this.declarationFactory = declarationFactory;
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpAssembly> CreateAssembly(Assembly assembly)
        {
            return new CSharpAssembly(
                this.loggerFactory.CreateLogger<CSharpAssembly>(),
                this.declarationFactory,
                assembly);
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpFile> CreateFile(string file)
        {
            return new CSharpFile(file, this.declarationFactory);
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpMetadataAssembly> CreateMetadataAssembly(string assemblyFile)
        {
            return new CSharpMetadataAssembly(
                this.loggerFactory.CreateLogger<CSharpMetadataAssembly>(),
                this.declarationFactory,
                assemblyFile);
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpProject> CreateProject(string file)
        {
            return new CSharpProject(file);
        }

        /// <inheritdoc/>
        public ICSharpWorkspaceItemLoader<ICSharpSyntaxTree> CreateSyntaxTree(SyntaxTree syntaxTree)
        {
            return new CSharpSyntaxTree(syntaxTree, this.declarationFactory);
        }
    }
}
