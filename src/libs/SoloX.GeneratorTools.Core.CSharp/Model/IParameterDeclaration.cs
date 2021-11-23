// ----------------------------------------------------------------------
// <copyright file="IParameterDeclaration.cs" company="SoloX Software">
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
    /// Parameter declaration.
    /// </summary>
    public interface IParameterDeclaration : IDeclaration<ParameterSyntax>
    {
        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        IDeclarationUse<SyntaxNode> ParameterType { get; }
    }
}
