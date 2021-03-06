﻿// ----------------------------------------------------------------------
// <copyright file="ICSharpLoader.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// ICSharpLoader interface used to load workspace elements.
    /// </summary>
    public interface ICSharpLoader
    {
        /// <summary>
        /// Load a given project.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="project">The project to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpProject project);

        /// <summary>
        /// Load a given file.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="file">The file to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpFile file);

        /// <summary>
        /// Load the given assembly.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="assembly">The assembly to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpAssembly assembly);

        /// <summary>
        /// Load the given CSharp declaration.
        /// </summary>
        /// <param name="resolver">The declaration resolver to resolve declaration dependencies.</param>
        /// <param name="declaration">The Declaration to load.</param>
        void Load(IDeclarationResolver resolver, IDeclaration<SyntaxNode> declaration);
    }
}
