// ----------------------------------------------------------------------
// <copyright file="IMethodDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Method declaration interface.
    /// </summary>
    public interface IMethodDeclaration : IMemberDeclaration<MethodDeclarationSyntax>
    {
        /// <summary>
        /// Gets the method return type.
        /// </summary>
        IDeclarationUse<SyntaxNode> ReturnType { get; }
    }
}
