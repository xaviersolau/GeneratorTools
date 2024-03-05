// ----------------------------------------------------------------------
// <copyright file="IIndexerDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Method declaration interface.
    /// </summary>
    public interface IIndexerDeclaration : IMemberDeclaration<IndexerDeclarationSyntax>
    {
        /// <summary>
        /// Gets the method return type.
        /// </summary>
        IDeclarationUse<SyntaxNode> ReturnType { get; }

        /// <summary>
        /// Return attributes.
        /// </summary>
        IReadOnlyList<IAttributeUse> ReturnAttributes { get; }

        /// <summary>
        /// Gets the method parameters.
        /// </summary>
        IReadOnlyCollection<IParameterDeclaration> Parameters { get; }

        /// <summary>
        /// Gets a value indicating whether tells if the property has a getter.
        /// </summary>
        bool HasGetter { get; }

        /// <summary>
        /// Gets a value indicating whether tells if the property has a setter.
        /// </summary>
        bool HasSetter { get; }
    }
}
