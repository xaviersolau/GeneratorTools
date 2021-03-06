﻿// ----------------------------------------------------------------------
// <copyright file="IPredefinedDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Predefined declaration.
    /// </summary>
    public interface IPredefinedDeclaration : IDeclaration<PredefinedTypeSyntax>
    {
    }
}
