// ----------------------------------------------------------------------
// <copyright file="UnknownDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Unknown declaration implementation.
    /// </summary>
    public class UnknownDeclaration : ADeclaration<SyntaxNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The declaration name space.</param>
        /// <param name="name">The name of the unknown declaration.</param>
        public UnknownDeclaration(string nameSpace, string name)
            : base(nameSpace, name, null, Array.Empty<string>(), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownDeclaration"/> class.
        /// </summary>
        /// <param name="name">The name of the unknown declaration.</param>
        public UnknownDeclaration(string name)
            : base(string.Empty, name, null, Array.Empty<string>(), null)
        {
        }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            // Nothing to do.
        }
    }
}
