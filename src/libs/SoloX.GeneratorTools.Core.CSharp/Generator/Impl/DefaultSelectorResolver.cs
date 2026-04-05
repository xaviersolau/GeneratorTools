// ----------------------------------------------------------------------
// <copyright file="DefaultSelectorResolver.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl
{
    /// <summary>
    /// Default selector type resolver.
    /// </summary>
    public class DefaultSelectorResolver : AInstanceResolver, ISelectorResolver
    {
        private static readonly IEnumerable<Type> DefaultTypes = new[]
        {
            typeof(AttributeSelector<>),
            typeof(ReadOnlyPropertySelector),
            typeof(ReadWritePropertySelector),
            typeof(InterfaceBasedOnSelector<>),
            typeof(AllSelector),
        };

        /// <summary>
        /// Setup instance with the given types to be resolved.
        /// </summary>
        /// <param name="types">Types to be resolved in selectors.</param>
        public DefaultSelectorResolver(params Type[] types)
            : base(DefaultTypes, types)
        {
        }

        /// <inheritdoc/>
        public ISelector? GetSelector(IDeclarationUse<SyntaxNode> selectorTypeUse)
        {
            return CreateInstance<ISelector>(selectorTypeUse);
        }
    }
}
