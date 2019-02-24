// ----------------------------------------------------------------------
// <copyright file="IDeclarationResolver.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Resolver
{
    /// <summary>
    /// Declaration resolver interface.
    /// </summary>
    public interface IDeclarationResolver
    {
        /// <summary>
        /// Resolve identifier as a generic declaration.
        /// </summary>
        /// <param name="identifier">The identifier to resolve.</param>
        /// <param name="genericParameters">The generic parameter list.</param>
        /// <param name="declarationContext">The declaration context.</param>
        /// <returns>The matching generic declaration or null if no match.</returns>
        IGenericDeclaration Resolve(
            string identifier,
            IReadOnlyList<IDeclarationUse> genericParameters,
            IDeclaration declarationContext);

        /// <summary>
        /// Resolve identifier as a declaration.
        /// </summary>
        /// <param name="identifier">The identifier to resolve.</param>
        /// <param name="declarationContext">The declaration context.</param>
        /// <returns>The matching declaration or null if no match.</returns>
        IDeclaration Resolve(string identifier, IDeclaration declarationContext);
    }
}
