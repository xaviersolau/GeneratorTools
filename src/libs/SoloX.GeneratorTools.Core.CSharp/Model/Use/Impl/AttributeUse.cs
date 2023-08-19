// ----------------------------------------------------------------------
// <copyright file="AttributeUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Attribute use model implementation.
    /// </summary>
    public class AttributeUse : IAttributeUse
    {
        private readonly Lazy<IReadOnlyDictionary<string, object>> namedArguments;
        private readonly Lazy<IReadOnlyCollection<object>> constructorArguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeUse"/> class.
        /// </summary>
        /// <param name="declarationUse">The attribute declaration.</param>
        /// <param name="syntaxNodeProvider">The attribute syntax node provider.</param>
        /// <param name="namedArguments"></param>
        /// <param name="constructorArguments"></param>
        public AttributeUse(
            IDeclarationUse<SyntaxNode> declarationUse,
            ISyntaxNodeProvider<AttributeSyntax> syntaxNodeProvider,
            IReadOnlyDictionary<string, object> namedArguments,
            IReadOnlyCollection<object> constructorArguments)
            : this(declarationUse, syntaxNodeProvider, () => namedArguments, () => constructorArguments)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeUse"/> class.
        /// </summary>
        /// <param name="declarationUse">The attribute declaration.</param>
        /// <param name="syntaxNodeProvider">The attribute syntax node provider.</param>
        /// <param name="namedArgumentsLoader"></param>
        /// <param name="constructorArgumentsLoader"></param>
        public AttributeUse(
            IDeclarationUse<SyntaxNode> declarationUse,
            ISyntaxNodeProvider<AttributeSyntax> syntaxNodeProvider,
            Func<IReadOnlyDictionary<string, object>> namedArgumentsLoader,
            Func<IReadOnlyCollection<object>> constructorArgumentsLoader)
        {
            if (declarationUse == null)
            {
                throw new ArgumentNullException(nameof(declarationUse), $"{nameof(declarationUse)} must not be null.");
            }

            var name = declarationUse.Declaration.Name;
            if (!name.EndsWith(nameof(Attribute), StringComparison.InvariantCulture))
            {
                name = $"{name}{nameof(Attribute)}";
            }

            this.Name = name;
            this.DeclarationUse = declarationUse;
            this.SyntaxNodeProvider = syntaxNodeProvider;

            this.namedArguments = new Lazy<IReadOnlyDictionary<string, object>>(namedArgumentsLoader);
            this.constructorArguments = new Lazy<IReadOnlyCollection<object>>(constructorArgumentsLoader);
        }

        /// <inheritdoc/>
        public ISyntaxNodeProvider<AttributeSyntax> SyntaxNodeProvider { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public IDeclarationUse<SyntaxNode> DeclarationUse { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, object> NamedArguments => this.namedArguments.Value;

        /// <inheritdoc/>
        public IReadOnlyCollection<object> ConstructorArguments => this.constructorArguments.Value;
    }
}
