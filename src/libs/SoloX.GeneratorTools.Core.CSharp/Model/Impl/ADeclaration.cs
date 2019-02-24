// ----------------------------------------------------------------------
// <copyright file="ADeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Base abstract declaration implementation.
    /// </summary>
    public abstract class ADeclaration : IDeclaration
    {
        private bool isLoaded = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ADeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <param name="syntaxNode">The declaration syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this class.</param>
        protected ADeclaration(string nameSpace, string name, CSharpSyntaxNode syntaxNode, IReadOnlyList<string> usingDirectives)
        {
            this.DeclarationNameSpace = nameSpace;
            this.Name = name;
            this.SyntaxNode = syntaxNode;
            this.UsingDirectives = usingDirectives;
            this.FullName = GetFullName(nameSpace, name);
        }

        /// <inheritdoc/>
        public string DeclarationNameSpace { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string FullName { get; }

        /// <inheritdoc/>
        public CSharpSyntaxNode SyntaxNode { get; }

        /// <inheritdoc/>
        public IReadOnlyList<string> UsingDirectives { get; }

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
        /// Compute the full name from the name space and the name of the declaration.
        /// </summary>
        /// <param name="nameSpace">The declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <returns>The declaration full name.</returns>
        internal static string GetFullName(string nameSpace, string name)
            => string.IsNullOrEmpty(nameSpace) ? name : $"{nameSpace}.{name}";

        /// <summary>
        /// Implementation of the declaration loading.
        /// </summary>
        /// <param name="resolver">The resolver to resolve dependencies.</param>
        protected abstract void LoadImpl(IDeclarationResolver resolver);
    }
}
