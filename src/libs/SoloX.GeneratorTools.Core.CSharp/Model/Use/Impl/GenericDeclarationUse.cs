// ----------------------------------------------------------------------
// <copyright file="GenericDeclarationUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Generic declaration use implementation.
    /// </summary>
    public class GenericDeclarationUse : ADeclarationUse<SimpleNameSyntax>, IGenericDeclarationUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNodeProvider">The declaration use syntax node provider.</param>
        /// <param name="declaration">The declaration in use.</param>
        /// <param name="genericParameters">The generic parameters.</param>
        public GenericDeclarationUse(
            ISyntaxNodeProvider<SimpleNameSyntax> syntaxNodeProvider,
            IDeclaration<SyntaxNode> declaration,
            IReadOnlyCollection<IDeclarationUse<SyntaxNode>> genericParameters)
            : base(syntaxNodeProvider, declaration)
        {
            this.GenericParameters = genericParameters;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclarationUse<SyntaxNode>> GenericParameters { get; }
    }
}
