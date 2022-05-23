// ----------------------------------------------------------------------
// <copyright file="IGenericDeclarationUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use
{
    /// <summary>
    /// Generic declaration  use interface.
    /// </summary>
    public interface IGenericDeclarationUse : IDeclarationUse<NameSyntax>
    {
        /// <summary>
        /// Gets the generic parameters used on the generic declaration.
        /// </summary>
        IReadOnlyCollection<IDeclarationUse<SyntaxNode>> GenericParameters { get; }
    }
}
