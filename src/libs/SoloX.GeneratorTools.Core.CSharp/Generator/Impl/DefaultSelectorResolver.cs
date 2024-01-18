// ----------------------------------------------------------------------
// <copyright file="DefaultSelectorResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl
{
    /// <summary>
    /// Default selector type resolver.
    /// </summary>
    public class DefaultSelectorResolver : ISelectorResolver
    {
        private static readonly IEnumerable<Type> DefaultTypes = new[]
        {
            typeof(AttributeSelector<>),
            typeof(ReadOnlyPropertySelector),
            typeof(ReadWritePropertySelector),
            typeof(InterfaceBasedOnSelector<>),
            typeof(AllSelector),
        };

        private readonly Dictionary<string, Type> typesByFullName;
        private readonly Dictionary<string, Type> typesByName;

        /// <summary>
        /// Setup instance with the given types to be resolved.
        /// </summary>
        /// <param name="types">Types to be resolved in selectors.</param>
        public DefaultSelectorResolver(params Type[] types)
        {
            var allTypes = types.Concat(DefaultTypes).ToArray();

            this.typesByFullName = allTypes.ToDictionary(t => t.FullName);
            this.typesByName = allTypes.ToDictionary(t => t.Name);
        }

        /// <inheritdoc/>
        public ISelector GetSelector(IDeclarationUse<SyntaxNode> selectorTypeUse)
        {
            if (selectorTypeUse == null)
            {
                throw new ArgumentNullException(nameof(selectorTypeUse));
            }

            var selectorType = ResolveAsType(selectorTypeUse);

            if (selectorType != null)
            {
                return (ISelector)Activator.CreateInstance(selectorType);
            }

            return null;
        }

        private Type ResolveAsType(IDeclarationUse<SyntaxNode> selectorTypeUse)
        {
            if (selectorTypeUse is IGenericDeclarationUse genericDeclarationUse)
            {
                var fullName = selectorTypeUse.Declaration.FullName;
                var genericCount = genericDeclarationUse.GenericParameters.Count;

                Type type;
                if (genericCount > 0)
                {
                    fullName = fullName + '`' + genericCount;

                    type = GetTypeFromName(fullName);

                    var genericParameters = new List<Type>();

                    foreach (var genericParameter in genericDeclarationUse.GenericParameters)
                    {
                        var genericParameterType = ResolveAsType(genericParameter);

                        genericParameters.Add(genericParameterType);
                    }

                    type = type.MakeGenericType(genericParameters.ToArray());
                }
                else
                {
                    type = GetTypeFromName(fullName);
                }

                return type;
            }

            return GetTypeFromName(selectorTypeUse.Declaration.Name);
        }

        /// <summary>
        /// Get the type matching the given name.
        /// </summary>
        /// <param name="name">Name of the type to resolve.</param>
        /// <returns>The resolver Type object or null.</returns>
        protected Type GetTypeFromName(string name)
        {
            if (this.typesByFullName.TryGetValue(name, out var type))
            {
                return type;
            }
            else if (this.typesByName.TryGetValue(name, out type))
            {
                return type;
            }

            throw new NotSupportedException($"Unable to resolve type from name: {name}");
        }
    }
}
