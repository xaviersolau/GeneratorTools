// ----------------------------------------------------------------------
// <copyright file="CSharpLoader.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// CSharp elements Loader implementation.
    /// </summary>
    public class CSharpLoader : ICSharpLoader
    {
        /// <inheritdoc/>
        public void Load(ICSharpWorkspace workspace, ICSharpProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), $"The argument {nameof(project)} was null.");
            }

            ((CSharpProject)project).Load(workspace);
        }

        /// <inheritdoc/>
        public void Load(ICSharpWorkspace workspace, ICSharpFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), $"The argument {nameof(file)} was null.");
            }

            ((CSharpFile)file).Load();
        }

        /// <inheritdoc/>
        public void Load(ICSharpWorkspace cSharpWorkspace, ICSharpAssembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly), $"The argument {nameof(assembly)} was null.");
            }

            ((CSharpAssembly)assembly).Load();
        }

        /// <inheritdoc/>
        public void Load(IDeclarationResolver resolver, IDeclaration<SyntaxNode> declaration)
        {
            if (declaration == null)
            {
                throw new ArgumentNullException(nameof(declaration), $"The argument {nameof(declaration)} was null.");
            }

            ((ADeclaration)declaration).Load(resolver);
        }
    }
}
