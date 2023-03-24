// ----------------------------------------------------------------------
// <copyright file="PredefinedDeclarationUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Predefined declaration use definition.
    /// </summary>
    public class PredefinedDeclarationUse : IPredefinedDeclarationUse, IPredefinedDeclaration, IArrayDeclarationUseImpl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNodeProvider">The predefined syntax node provider.</param>
        /// <param name="name">Predefined type name.</param>
        public PredefinedDeclarationUse(ISyntaxNodeProvider<PredefinedTypeSyntax> syntaxNodeProvider, string name)
        {
            this.SyntaxNodeProvider = syntaxNodeProvider;
            this.Name = name;

            IsValueType = name != "object" && name != "string";
        }

        /// <inheritdoc/>
        public ISyntaxNodeProvider<PredefinedTypeSyntax> SyntaxNodeProvider { get; }

        /// <inheritdoc/>
        public string DeclarationNameSpace => string.Empty;

        /// <inheritdoc/>
        public IDeclaration<SyntaxNode> Declaration => this;

        /// <inheritdoc/>
        public IArraySpecification ArraySpecification { get; set; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string FullName => this.Name;

        /// <inheritdoc/>
        public bool IsValueType { get; }

        /// <inheritdoc/>
        public IUsingDirectives UsingDirectives => NoUsingDirectives.Instance;

        /// <inheritdoc/>
        public IReadOnlyList<IAttributeUse> Attributes => Array.Empty<IAttributeUse>();

        /// <inheritdoc/>
        public string Location { get; }

        /// <inheritdoc/>
        public void DeepLoad(IDeclarationResolver resolver)
        {
            // Nothing to load.
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"U {this.Name}";
        }
    }
}
