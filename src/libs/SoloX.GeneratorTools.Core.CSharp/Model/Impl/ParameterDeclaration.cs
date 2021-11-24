// ----------------------------------------------------------------------
// <copyright file="ParameterDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Parameter declaration.
    /// </summary>
    public class ParameterDeclaration : ADeclaration<ParameterSyntax>, IParameterDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDeclaration"/> class.
        /// </summary>
        /// <param name="name">The declaration name.</param>
        /// <param name="parameterType">The parameter type use declaration.</param>
        /// <param name="syntaxNodeProvider">The declaration syntax node provider.</param>
        public ParameterDeclaration(string name, IDeclarationUse<SyntaxNode> parameterType, ISyntaxNodeProvider<ParameterSyntax> syntaxNodeProvider)
            : base(string.Empty, name, syntaxNodeProvider, Array.Empty<string>(), null)
        {
            this.ParameterType = parameterType;
        }

        /// <inheritdoc/>
        public IDeclarationUse<SyntaxNode> ParameterType { get; }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            // Nothing to load.
        }
    }
}
