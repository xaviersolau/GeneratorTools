// ----------------------------------------------------------------------
// <copyright file="IReplacePatternHandlerFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern
{
    /// <summary>
    /// Replace pattern handler factory.
    /// </summary>
    public interface IReplacePatternHandlerFactory
    {
        /// <summary>
        /// Setup the handler.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="declaration">The declaration.</param>
        /// <returns>The created Replace Pattern Handler.</returns>
        IReplacePatternHandler Setup(
            IGenericDeclaration<SyntaxNode> pattern,
            IGenericDeclaration<SyntaxNode> declaration);

        /// <summary>
        /// Setup replace handler.
        /// </summary>
        /// <param name="pattern">The method pattern.</param>
        /// <param name="declaration">The method declaration.</param>
        /// <returns></returns>
        IReplacePatternHandler Setup(IMethodDeclaration pattern, IMethodDeclaration declaration);
    }
}
