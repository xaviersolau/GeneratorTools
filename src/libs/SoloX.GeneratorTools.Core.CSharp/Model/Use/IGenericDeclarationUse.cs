﻿// ----------------------------------------------------------------------
// <copyright file="IGenericDeclarationUse.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Generic declaration  use interface.
    /// </summary>
    public interface IGenericDeclarationUse : IDeclarationUse<SimpleNameSyntax>
    {
        /// <summary>
        /// Gets the generic parameters used on the generic declaration.
        /// </summary>
        IReadOnlyCollection<IDeclarationUse<SyntaxNode>> GenericParameters { get; }
    }
}
