// ----------------------------------------------------------------------
// <copyright file="GenericParameterDeclarationUse.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// Generic parameter declaration use.
    /// </summary>
    public class GenericParameterDeclarationUse : ADeclarationUse<SimpleNameSyntax>, IGenericParameterDeclarationUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNodeProvider">The declaration use syntax node provider.</param>
        /// <param name="declaration">The declaration in use.</param>
        public GenericParameterDeclarationUse(
            ISyntaxNodeProvider<SimpleNameSyntax> syntaxNodeProvider,
            IDeclaration<SyntaxNode> declaration)
            : base(syntaxNodeProvider, declaration)
        {
        }
    }
}
