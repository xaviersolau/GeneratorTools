// ----------------------------------------------------------------------
// <copyright file="CSharpFile.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implements ICSharpFile.
    /// </summary>
    public class CSharpFile : ICSharpFile
    {
        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpFile"/> class.
        /// </summary>
        /// <param name="file">The CSharp file.</param>
        public CSharpFile(string file)
        {
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
        public IReadOnlyCollection<IDeclaration> Declarations { get; private set; }

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

            var declarations = new List<IDeclaration>();
            var location = Path.Combine(this.FilePath, this.FileName);
            CSharpFileReader.Read(location, new DeclarationWalker(declarations, location));

            this.Declarations = declarations;
        }
    }
}
