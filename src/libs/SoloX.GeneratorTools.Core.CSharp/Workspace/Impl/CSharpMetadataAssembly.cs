// ----------------------------------------------------------------------
// <copyright file="CSharpMetadataAssembly.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implement ICSharpMetadataAssembly in order to load all type declarations form MetadataReader.
    /// </summary>
    public class CSharpMetadataAssembly : ICSharpMetadataAssembly, ICSharpWorkspaceItemLoader<ICSharpMetadataAssembly>
    {
        private readonly IGeneratorLogger<CSharpMetadataAssembly> logger;
        private readonly IDeclarationFactory declarationFactory;
        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpMetadataAssembly"/> class.
        /// </summary>
        /// <param name="logger">The logger to log errors.</param>
        /// <param name="declarationFactory">The declaration factory to use to create declaration instances.</param>
        /// <param name="assemblyPath">The assembly to load declaration from.</param>
        public CSharpMetadataAssembly(IGeneratorLogger<CSharpMetadataAssembly> logger, IDeclarationFactory declarationFactory, string assemblyPath)
        {
            this.logger = logger;
            this.declarationFactory = declarationFactory;
            AssemblyPath = assemblyPath;
        }

        /// <inheritdoc/>
        public string AssemblyPath { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations { get; private set; }

        /// <inheritdoc/>
        public ICSharpMetadataAssembly WorkspaceItem => this;

        /// <inheritdoc/>
        public void Load(ICSharpWorkspace workspace)
        {
            if (workspace == null)
            {
                throw new ArgumentNullException(nameof(workspace), $"The argument {nameof(workspace)} was null.");
            }

            if (this.isLoaded)
            {
                return;
            }

            this.isLoaded = true;

            var declarations = new List<IDeclaration<SyntaxNode>>();

            try
            {
                using var portableExecutableReader = new PEReader(File.OpenRead(this.AssemblyPath));

                if (!portableExecutableReader.HasMetadata)
                {
                    this.Declarations = declarations;
                    return;
                }

                var metadataReader = portableExecutableReader.GetMetadataReader();

                foreach (var assemblyReferenceHandle in metadataReader.AssemblyReferences)
                {
                    var assemblyReference = metadataReader.GetAssemblyReference(assemblyReferenceHandle);

                    var assemblyname = metadataReader.GetString(assemblyReference.Name);

                    workspace.RegisterMetadataAssembly($"{assemblyname}.dll");
                }

                foreach (var typeDefinitionHandle in metadataReader.TypeDefinitions)
                {
                    var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

                    var name = metadataReader.GetString(typeDefinition.Name);

                    if (!name.StartsWith("<", StringComparison.Ordinal))
                    {
                        var attributes = typeDefinition.Attributes;

                        var visibility = attributes & TypeAttributes.VisibilityMask;

                        if (visibility == TypeAttributes.Public || visibility == TypeAttributes.NestedPublic)
                        {
                            var classSemantics = attributes & TypeAttributes.ClassSemanticsMask;
                            if (classSemantics == TypeAttributes.Interface)
                            {
                                var typeInterfaceDeclaration = this.declarationFactory.CreateInterfaceDeclaration(metadataReader, typeDefinitionHandle, this.AssemblyPath);
                                declarations.Add(typeInterfaceDeclaration);
                            }
                            else if (classSemantics == TypeAttributes.Class)
                            {
                                var typeClassDeclaration = this.declarationFactory.CreateClassDeclaration(metadataReader, typeDefinitionHandle, this.AssemblyPath);
                                declarations.Add(typeClassDeclaration);
                            }
                            else
                            {
                                // TODO
                            }
                        }
                        else
                        {
                            // TODO
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                this.logger?.LogWarning(e, $"Could not load types from {this.AssemblyPath}");
            }

            this.Declarations = declarations;
        }
    }
}
