// ----------------------------------------------------------------------
// <copyright file="IReplacePatternResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern
{
    /// <summary>
    /// ReplacePatternHandler resolver instantiating handler factory from a given name.
    /// </summary>
    public interface IReplacePatternResolver
    {
        /// <summary>
        /// Try get a handler factory matching a given name.
        /// </summary>
        /// <param name="replacePatternHandlerTypeUse"></param>
        /// <returns></returns>
        IReplacePatternHandlerFactory GetHandlerFactory(IDeclarationUse<SyntaxNode> replacePatternHandlerTypeUse);
    }
}
