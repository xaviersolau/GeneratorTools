// ----------------------------------------------------------------------
// <copyright file="BasicDeclarationUse.cs" company="Xavier Solau">
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
    /// Basic declaration use implementation.
    /// </summary>
    public class BasicDeclarationUse : ADeclarationUse<NameSyntax>, IBasicDeclarationUse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicDeclarationUse"/> class.
        /// </summary>
        /// <param name="syntaxNodeProvider">The declaration use syntax node provider.</param>
        /// <param name="declaration">The declaration in use.</param>
        public BasicDeclarationUse(
            ISyntaxNodeProvider<NameSyntax> syntaxNodeProvider,
            IDeclaration<SyntaxNode> declaration)
            : base(syntaxNodeProvider, declaration)
        {
        }
    }
}
