﻿// ----------------------------------------------------------------------
// <copyright file="ISelector.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Selectors
{
    /// <summary>
    /// Declaration selector.
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// Select the target where to apply the pattern.
        /// </summary>
        /// <param name="files">Files where to make the selection.</param>
        /// <returns>The selected declarations.</returns>
        IEnumerable<IDeclaration<SyntaxNode>> GetDeclarations(IEnumerable<ICSharpFile> files);

        /// <summary>
        /// Select the target where to apply the pattern.
        /// </summary>
        /// <param name="declaration">declaration where to make the selection.</param>
        /// <returns>The selected property declarations.</returns>
        IEnumerable<IPropertyDeclaration> GetProperties(IGenericDeclaration<SyntaxNode> declaration);
    }
}
