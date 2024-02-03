// ----------------------------------------------------------------------
// <copyright file="DefaultReplacePatternResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System.Collections.Generic;
using System;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl
{
    /// <summary>
    /// Default resolver for replace pattern handler factory.
    /// </summary>
    public class DefaultReplacePatternResolver : AInstanceResolver, IReplacePatternResolver
    {
        private static readonly IEnumerable<Type> DefaultTypes = new[]
        {
            typeof(TaskValueTypeReplaceHandler),
        };

        /// <summary>
        /// Setup instance with the given types to be resolved.
        /// </summary>
        /// <param name="types">Types to be resolved in selectors.</param>
        public DefaultReplacePatternResolver(params Type[] types)
            : base(DefaultTypes, types)
        {
        }

        /// <inheritdoc/>
        public IReplacePatternHandlerFactory GetHandlerFactory(IDeclarationUse<SyntaxNode> replacePatternHandlerTypeUse)
        {
            return CreateInstance<IReplacePatternHandlerFactory>(replacePatternHandlerTypeUse);
        }
    }
}
