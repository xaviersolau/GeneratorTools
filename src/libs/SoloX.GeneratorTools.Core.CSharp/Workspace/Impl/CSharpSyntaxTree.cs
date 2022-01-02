// ----------------------------------------------------------------------
// <copyright file="CSharpSyntaxTree.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using System;
using System.Collections.Generic;
using System.IO;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implements ICSharpSyntaxTree.
    /// </summary>
    public class CSharpSyntaxTree : ICSharpSyntaxTree
    {
        private readonly IDeclarationFactory declarationFactory;

        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpSyntaxTree"/> class.
        /// </summary>
        /// <param name="syntaxTree">The CSharp SyntaxTree.</param>
        /// <param name="declarationFactory">The declaration factory to use to create declaration instances.</param>
        public CSharpSyntaxTree(SyntaxTree syntaxTree, IDeclarationFactory declarationFactory)
        {
            if (syntaxTree == null)
            {
                throw new ArgumentNullException(nameof(syntaxTree));
            }

            SyntaxTree = syntaxTree;

            this.declarationFactory = declarationFactory;

            var file = syntaxTree.FilePath;

            this.FileName = Path.GetFileName(file);
            this.FilePath = Path.GetDirectoryName(file);
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations { get; private set; }

        /// <inheritdoc/>
        public SyntaxTree SyntaxTree { get; }

        /// <inheritdoc/>
        public string FileName { get; }

        /// <inheritdoc/>
        public string FilePath { get; }

        /// <summary>
        /// Load the CSharp file.
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
            {
                return;
            }

            this.isLoaded = true;

            var declarations = new List<IDeclaration<SyntaxNode>>();

            new DeclarationWalker(this.declarationFactory, declarations, this.SyntaxTree.FilePath).Visit(this.SyntaxTree.GetRoot());

            this.Declarations = declarations;
        }
    }
}
