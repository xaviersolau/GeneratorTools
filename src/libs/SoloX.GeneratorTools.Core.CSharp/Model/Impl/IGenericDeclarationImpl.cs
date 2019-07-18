// ----------------------------------------------------------------------
// <copyright file="IGenericDeclarationImpl.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Generic declaration implementation interface.
    /// The purpose is to give access to the AddExtendedBy method.
    /// </summary>
    public interface IGenericDeclarationImpl
    {
        /// <summary>
        /// Add ExtendedBy declaration.
        /// </summary>
        /// <param name="declaration">The declaration to add.</param>
        void AddExtendedBy(IGenericDeclaration<SyntaxNode> declaration);
    }
}
