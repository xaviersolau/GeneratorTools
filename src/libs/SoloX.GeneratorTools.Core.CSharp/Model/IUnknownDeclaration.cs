// ----------------------------------------------------------------------
// <copyright file="IUnknownDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Unknown declaration interface.
    /// </summary>
    public interface IUnknownDeclaration : IDeclaration<CSharpSyntaxNode>
    {
    }
}
