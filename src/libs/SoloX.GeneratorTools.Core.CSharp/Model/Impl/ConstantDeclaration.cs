// ----------------------------------------------------------------------
// <copyright file="ConstantDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Property declaration implementation.
    /// </summary>
    public class ConstantDeclaration : AMemberDeclaration<VariableDeclaratorSyntax>, IConstantDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDeclaration"/> class.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <param name="constantType">The property type use.</param>
        /// <param name="syntaxNodeProvider">The property syntax provider.</param>
        /// <param name="attributes">Attributes attached to the property.</param>
        public ConstantDeclaration(
            string name,
            IDeclarationUse<SyntaxNode> constantType,
            ISyntaxNodeProvider<VariableDeclaratorSyntax> syntaxNodeProvider,
            IReadOnlyList<IAttributeUse> attributes)
            : base(name, syntaxNodeProvider, attributes)
        {
            this.ConstantType = constantType;
        }

        /// <inheritdoc/>
        public IDeclarationUse<SyntaxNode> ConstantType { get; }
    }
}
