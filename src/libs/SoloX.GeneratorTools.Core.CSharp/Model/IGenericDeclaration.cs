// ----------------------------------------------------------------------
// <copyright file="IGenericDeclaration.cs" company="Xavier Solau">
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
    /// Generic declaration interface.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public interface IGenericDeclaration<out TNode>
         : IDeclaration<TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Gets the declaration syntax node provider.
        /// </summary>
        ISyntaxNodeProvider<TypeParameterListSyntax> TypeParameterListSyntaxProvider { get; }

        /// <summary>
        /// Gets the generic parameters.
        /// </summary>
        IReadOnlyCollection<IGenericParameterDeclaration> GenericParameters { get; }

        /// <summary>
        /// Gets the extends list.
        /// </summary>
        IReadOnlyCollection<IDeclarationUse<SyntaxNode>> Extends { get; }

        /// <summary>
        /// Gets the extended by list.
        /// </summary>
        IReadOnlyCollection<IGenericDeclaration<SyntaxNode>> ExtendedBy { get; }

        /// <summary>
        /// Gets the member list.
        /// </summary>
        IReadOnlyCollection<IMemberDeclaration<SyntaxNode>> Members { get; }

        /// <summary>
        /// Gets the property list.
        /// </summary>
        IReadOnlyCollection<IPropertyDeclaration> Properties { get; }

        /// <summary>
        /// Gets the method list.
        /// </summary>
        IReadOnlyCollection<IMethodDeclaration> Methods { get; }
    }
}
