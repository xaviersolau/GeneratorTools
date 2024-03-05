// ----------------------------------------------------------------------
// <copyright file="AGenericDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

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
        /// <param name="isValueType">Tells if this is a valueType.</param>
        /// <param name="isRecordType">Tells if this is a record type.</param>
        protected AGenericDeclaration(
            string nameSpace,
            string name,
            ISyntaxNodeProvider<TNode> syntaxNodeProvider,
            IUsingDirectives usingDirectives,
            string location,
            AGenericDeclarationLoader<TNode> loader,
            bool isValueType,
            bool isRecordType)
            : base(nameSpace, name, syntaxNodeProvider, usingDirectives, location, isValueType, isRecordType)
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
        public IReadOnlyCollection<IMethodDeclaration> Methods
            => this.Members.OfType<IMethodDeclaration>().ToArray();

        /// <inheritdoc/>
        public IReadOnlyCollection<IConstantDeclaration> Constants
            => this.Members.OfType<IConstantDeclaration>().ToArray();

        /// <inheritdoc/>
        public IReadOnlyCollection<IIndexerDeclaration> Indexers
            => this.Members.OfType<IIndexerDeclaration>().ToArray();

        /// <inheritdoc/>
        public IReadOnlyCollection<INamedMemberDeclaration<SyntaxNode>> NamedMembers
            => this.Members.OfType<INamedMemberDeclaration<SyntaxNode>>().ToArray();

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.Extends?.Count > 0)
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
