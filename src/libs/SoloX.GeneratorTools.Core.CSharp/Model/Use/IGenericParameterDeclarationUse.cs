﻿// ----------------------------------------------------------------------
// <copyright file="IGenericParameterDeclarationUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Generic parameter declaration use.
    /// </summary>
    public interface IGenericParameterDeclarationUse : IDeclarationUse<SimpleNameSyntax>
    {
    }
}
