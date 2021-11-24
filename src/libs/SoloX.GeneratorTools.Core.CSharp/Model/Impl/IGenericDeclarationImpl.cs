// ----------------------------------------------------------------------
// <copyright file="IGenericDeclarationImpl.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Generic declaration implementation interface.
    /// The purpose is to give access to the AddExtendedBy method.
    /// </summary>
#pragma warning disable CA1711 // Les identificateurs ne doivent pas avoir un suffixe incorrect
    public interface IGenericDeclarationImpl
#pragma warning restore CA1711 // Les identificateurs ne doivent pas avoir un suffixe incorrect
    {
        /// <summary>
        /// Add ExtendedBy declaration.
        /// </summary>
        /// <param name="declaration">The declaration to add.</param>
        void AddExtendedBy(IGenericDeclaration<SyntaxNode> declaration);
    }
}
