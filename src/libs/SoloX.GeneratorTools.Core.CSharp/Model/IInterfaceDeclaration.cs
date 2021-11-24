// ----------------------------------------------------------------------
// <copyright file="IInterfaceDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Interface declaration interface.
    /// </summary>
    public interface IInterfaceDeclaration : IGenericDeclaration<InterfaceDeclarationSyntax>
    {
    }
}
