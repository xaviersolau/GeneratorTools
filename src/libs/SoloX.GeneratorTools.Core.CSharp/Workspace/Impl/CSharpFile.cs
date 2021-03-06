﻿// ----------------------------------------------------------------------
// <copyright file="CSharpFile.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implements ICSharpFile.
    /// </summary>
    public class CSharpFile : ICSharpFile
    {
        private readonly IDeclarationFactory declarationFactory;

        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpFile"/> class.
        /// </summary>
        /// <param name="file">The CSharp file.</param>
        /// <param name="declarationFactory">The declaration factory to use to create declaration instances.</param>
        public CSharpFile(string file, IDeclarationFactory declarationFactory)
        {
            this.declarationFactory = declarationFactory;

            if (!File.Exists(file))
            {
                throw new FileNotFoundException();
            }

            this.FileName = Path.GetFileName(file);
            this.FilePath = Path.GetDirectoryName(file);
        }

        /// <inheritdoc/>
        public string FileName { get; }

        /// <inheritdoc/>
        public string FilePath { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations { get; private set; }

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
            var location = Path.Combine(this.FilePath, this.FileName);
            CSharpFileReader.Read(location, new DeclarationWalker(this.declarationFactory, declarations, location));

            this.Declarations = declarations;
        }
    }
}
