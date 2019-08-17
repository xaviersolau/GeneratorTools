// ----------------------------------------------------------------------
// <copyright file="AGenericDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Base abstract generic declaration implementation.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public abstract class AGenericDeclaration<TNode> : ADeclaration<TNode>, IGenericDeclaration<TNode>, IGenericDeclarationImpl
        where TNode : SyntaxNode
    {
        private readonly List<IGenericDeclaration<SyntaxNode>> extendedBy = new List<IGenericDeclaration<SyntaxNode>>();
        private readonly AGenericDeclarationLoader<TNode> loader;

        /// <summary>
        /// Initializes a new instance of the <see cref="AGenericDeclaration{TNode}"/> class.
        /// </summary>
        /// <param name="nameSpace">The class declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <param name="syntaxNodeProvider">The declaration syntax node provider.</param>
        /// <param name="usingDirectives">The current using directive available for this class.</param>
        /// <param name="location">The location of the declaration.</param>
        /// <param name="loader">The loader to use when deep loading the declaration.</param>
        protected AGenericDeclaration(
            string nameSpace,
            string name,
            ISyntaxNodeProvider<TNode> syntaxNodeProvider,
            IReadOnlyList<string> usingDirectives,
            string location,
            AGenericDeclarationLoader<TNode> loader)
            : base(nameSpace, name, syntaxNodeProvider, usingDirectives, location)
        {
            this.loader = loader;
        }

        /// <inheritdoc/>
        public ISyntaxNodeProvider<TypeParameterListSyntax> TypeParameterListSyntaxProvider
            => this.loader.GetTypeParameterListSyntaxProvider(this);

        /// <inheritdoc/>
        public IReadOnlyCollection<IGenericParameterDeclaration> GenericParameters { get; internal set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclarationUse<SyntaxNode>> Extends { get; internal set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IGenericDeclaration<SyntaxNode>> ExtendedBy
            => this.extendedBy;

        /// <inheritdoc/>
        public IReadOnlyCollection<IMemberDeclaration<SyntaxNode>> Members { get; internal set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IPropertyDeclaration> Properties
            => this.Members.OfType<IPropertyDeclaration>().ToArray();

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.Extends?.Any() ?? false)
            {
                return $"{base.ToString()}: {string.Join(", ", this.Extends?.Select(e => e.ToString()))}";
            }

            return base.ToString();
        }

        /// <inheritdoc/>
        public void AddExtendedBy(IGenericDeclaration<SyntaxNode> declaration)
        {
            this.extendedBy.Add(declaration);
        }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            this.loader.Load(this, resolver);
        }
    }
}
