﻿// ----------------------------------------------------------------------
// <copyright file="ICSharpFile.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// Interface describing a CSharp file.
    /// </summary>
    public interface ICSharpFile
    {
        /// <summary>
        /// Gets the CSharp file name.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets the CSharp file path.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Gets the file declarations.
        /// </summary>
        IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations { get; }
    }
}
