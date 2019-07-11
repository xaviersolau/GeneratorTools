// ----------------------------------------------------------------------
// <copyright file="ADeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Base abstract declaration implementation.
    /// </summary>
    public abstract class ADeclaration
    {
        private bool isLoaded = false;

        /// <summary>
        /// Load the declaration.
        /// </summary>
        /// <param name="resolver">The resolver to resolve dependencies.</param>
        public void Load(IDeclarationResolver resolver)
        {
            if (this.isLoaded)
            {
                return;
            }

            this.isLoaded = true;

            this.LoadImpl(resolver);
        }

        /// <summary>
        /// Implementation of the declaration loading.
        /// </summary>
        /// <param name="resolver">The resolver to resolve dependencies.</param>
        protected abstract void LoadImpl(IDeclarationResolver resolver);
    }

    /// <summary>
    /// Base abstract declaration implementation.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
#pragma warning disable SA1402 // File may only contain a single type
    public abstract class ADeclaration<TNode> : ADeclaration, IDeclaration<TNode>
#pragma warning restore SA1402 // File may only contain a single type
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ADeclaration{TNode}"/> class.
        /// </summary>
        /// <param name="nameSpace">The declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <param name="syntaxNode">The declaration syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this class.</param>
        /// <param name="location">The location of the declaration.</param>
        protected ADeclaration(string nameSpace, string name, TNode syntaxNode, IReadOnlyList<string> usingDirectives, string location)
        {
            this.DeclarationNameSpace = nameSpace;
            this.Name = name;
            this.SyntaxNode = syntaxNode;
            this.UsingDirectives = usingDirectives;
            this.FullName = GetFullName(nameSpace, name);
            this.Location = location;
        }

        /// <inheritdoc/>
        public string DeclarationNameSpace { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string FullName { get; }

        /// <inheritdoc/>
        public TNode SyntaxNode { get; }

        /// <inheritdoc/>
        public ISyntaxNodeProvider<TNode> SyntaxNodeProvider { get; }

        /// <inheritdoc/>
        public IReadOnlyList<string> UsingDirectives { get; }

        /// <inheritdoc/>
        public string Location { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"D {this.Name}";
        }

        /// <summary>
        /// Compute the full name from the name space and the name of the declaration.
        /// </summary>
        /// <param name="nameSpace">The declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <returns>The declaration full name.</returns>
        internal static string GetFullName(string nameSpace, string name)
            => string.IsNullOrEmpty(nameSpace) ? name : $"{nameSpace}.{name}";
    }
}
