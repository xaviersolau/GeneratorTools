// ----------------------------------------------------------------------
// <copyright file="CSharpAssembly.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implement ICSharpAssembly in order to load all type declarations.
    /// </summary>
    [DebuggerDisplay("CSharpAssembly {Assembly.GetName()}")]
    public class CSharpAssembly : ICSharpAssembly, ICSharpWorkspaceItemLoader<ICSharpAssembly>
    {
        private readonly IGeneratorLogger<CSharpAssembly> logger;
        private readonly IDeclarationFactory declarationFactory;
        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpAssembly"/> class.
        /// </summary>
        /// <param name="logger">The logger to log errors.</param>
        /// <param name="declarationFactory">The declaration factory to use to create declaration instances.</param>
        /// <param name="assembly">The assembly to load declaration from.</param>
        public CSharpAssembly(IGeneratorLogger<CSharpAssembly> logger, IDeclarationFactory declarationFactory, Assembly assembly)
        {
            this.Assembly = assembly;
            this.logger = logger;
            this.declarationFactory = declarationFactory;
        }

        /// <inheritdoc/>
        public Assembly Assembly { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations { get; private set; }

        /// <inheritdoc/>
        public ICSharpAssembly WorkspaceItem => this;

        /// <inheritdoc/>
        public void Load(ICSharpWorkspace workspace)
        {
            if (this.isLoaded)
            {
                return;
            }

            this.isLoaded = true;

            var declarations = new List<IDeclaration<SyntaxNode>>();

            try
            {
                foreach (var type in this.Assembly.GetExportedTypes())
                {
                    if (type.IsInterface)
                    {
                        var typeInterfaceDeclaration = this.declarationFactory.CreateInterfaceDeclaration(type);

                        declarations.Add(typeInterfaceDeclaration);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                this.logger?.LogWarning(e, $"Could not load types from {this.Assembly.GetName()}");
            }

            this.Declarations = declarations;
        }
    }
}