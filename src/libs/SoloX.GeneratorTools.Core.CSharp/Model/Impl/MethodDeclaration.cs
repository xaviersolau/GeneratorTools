// ----------------------------------------------------------------------
// <copyright file="MethodDeclaration.cs" company="Xavier Solau">
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
    public class MethodDeclaration : AMemberDeclaration<MethodDeclarationSyntax>, IMethodDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDeclaration"/> class.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <param name="returnType">The return type use.</param>
        /// <param name="syntaxNodeProvider">The property syntax provider.</param>
        /// <param name="genericParameters">The generic method parameters.</param>
        /// <param name="parameters">The method parameters.</param>
        /// <param name="attributes">Attributes attached to the method.</param>
        /// <param name="returnAttributes">Return attributes attached to the method return value.</param>
        public MethodDeclaration(
            string name,
            IDeclarationUse<SyntaxNode> returnType,
            ISyntaxNodeProvider<MethodDeclarationSyntax> syntaxNodeProvider,
            IReadOnlyCollection<IGenericParameterDeclaration> genericParameters,
            IReadOnlyCollection<IParameterDeclaration> parameters,
            IReadOnlyList<IAttributeUse> attributes,
            IReadOnlyList<IAttributeUse> returnAttributes)
            : base(name, syntaxNodeProvider, attributes)
        {
            this.ReturnType = returnType;
            this.GenericParameters = genericParameters;
            this.Parameters = parameters;
            this.ReturnAttributes = returnAttributes;
        }

        /// <inheritdoc/>
        public IDeclarationUse<SyntaxNode> ReturnType { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IGenericParameterDeclaration> GenericParameters { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IParameterDeclaration> Parameters { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IAttributeUse> ReturnAttributes { get; }
    }
}
