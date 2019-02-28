// ----------------------------------------------------------------------
// <copyright file="IGenericDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Generic declaration interface.
    /// </summary>
    public interface IGenericDeclaration : IDeclaration
    {
        /// <summary>
        /// Gets the generic parameters syntax node.
        /// </summary>
        TypeParameterListSyntax TypeParameterListSyntax { get; }

        /// <summary>
        /// Gets the generic parameters.
        /// </summary>
        IReadOnlyCollection<IGenericParameterDeclaration> GenericParameters { get; }

        /// <summary>
        /// Gets the extends list.
        /// </summary>
        IReadOnlyCollection<IDeclarationUse> Extends { get; }

        /// <summary>
        /// Gets the extended by list.
        /// </summary>
        IReadOnlyCollection<IGenericDeclaration> ExtendedBy { get; }

        /// <summary>
        /// Gets the member list.
        /// </summary>
        IReadOnlyCollection<IMemberDeclaration> Members { get; }
    }
}
