// ----------------------------------------------------------------------
// <copyright file="IndexerDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Method declaration implementation.
    /// </summary>
    public class IndexerDeclaration : AMemberDeclaration<IndexerDeclarationSyntax>, IIndexerDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDeclaration"/> class.
        /// </summary>
        /// <param name="returnType">The return type use.</param>
        /// <param name="syntaxNodeProvider">The property syntax provider.</param>
        /// <param name="parameters">The method parameters.</param>
        /// <param name="attributes">Attributes attached to the method.</param>
        /// <param name="returnAttributes">Return attributes attached to the method return value.</param>
        /// <param name="hasGetter">Tells if the property has a getter.</param>
        /// <param name="hasSetter">Tells if the property has a setter.</param>
        public IndexerDeclaration(
            IDeclarationUse<SyntaxNode> returnType,
            ISyntaxNodeProvider<IndexerDeclarationSyntax> syntaxNodeProvider,
            IReadOnlyCollection<IParameterDeclaration> parameters,
            IReadOnlyList<IAttributeUse> attributes,
            IReadOnlyList<IAttributeUse> returnAttributes,
            bool hasGetter,
            bool hasSetter)
            : base(syntaxNodeProvider, attributes)
        {
            this.ReturnType = returnType;
            this.Parameters = parameters;
            this.ReturnAttributes = returnAttributes;
            this.HasGetter = hasGetter;
            this.HasSetter = hasSetter;
        }

        /// <inheritdoc/>
        public IDeclarationUse<SyntaxNode> ReturnType { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IParameterDeclaration> Parameters { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IAttributeUse> ReturnAttributes { get; }

        /// <inheritdoc/>
        public bool HasGetter { get; }

        /// <inheritdoc/>
        public bool HasSetter { get; }
    }
}
