// ----------------------------------------------------------------------
// <copyright file="GenericParameterDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Generic parameter declaration.
    /// </summary>
    public class GenericParameterDeclaration : ADeclaration<TypeParameterSyntax>, IGenericParameterDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterDeclaration"/> class.
        /// </summary>
        /// <param name="name">The declaration name.</param>
        /// <param name="syntaxNodeProvider">The declaration syntax node provider.</param>
        public GenericParameterDeclaration(string name, ISyntaxNodeProvider<TypeParameterSyntax> syntaxNodeProvider)
            : base(string.Empty, name, syntaxNodeProvider, Array.Empty<string>(), null)
        {
        }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            // Nothing to load.
        }
    }
}
