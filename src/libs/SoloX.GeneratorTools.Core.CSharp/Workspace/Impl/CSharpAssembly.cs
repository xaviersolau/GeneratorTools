// ----------------------------------------------------------------------
// <copyright file="CSharpAssembly.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implement ICSharpAssembly in order to load all type declarations.
    /// </summary>
    public class CSharpAssembly : ICSharpAssembly
    {
        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpAssembly"/> class.
        /// </summary>
        /// <param name="assembly">The assembly to load declaration from.</param>
        public CSharpAssembly(Assembly assembly)
        {
            this.Assembly = assembly;
        }

        /// <inheritdoc/>
        public Assembly Assembly { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations { get; private set; }

        /// <summary>
        /// Load assembly declarations.
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
            {
                return;
            }

            this.isLoaded = true;

            var declarations = new List<IDeclaration<SyntaxNode>>();

            foreach (var type in this.Assembly.GetExportedTypes())
            {
                if (type.IsInterface)
                {
                    var typeInterfaceDeclaration = new InterfaceDeclaration(
                        type.Namespace,
                        ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>.GetNameWithoutGeneric(type.Name),
                        null,
                        Array.Empty<string>(),
                        this.Assembly.Location,
                        ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>.Shared);

                    typeInterfaceDeclaration.DeclarationType = type;

                    declarations.Add(typeInterfaceDeclaration);
                }
            }

            this.Declarations = declarations;
        }
    }
}