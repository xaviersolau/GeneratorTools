// ----------------------------------------------------------------------
// <copyright file="AInstanceResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl
{
    /// <summary>
    /// Base type resolver to find a Type from a IDeclarationUse
    /// </summary>
    public abstract class AInstanceResolver
    {
        private readonly Dictionary<string, Type> typesByFullName;
        private readonly Dictionary<string, Type> typesByName;

        /// <summary>
        /// Setup instance with the given types to be resolved.
        /// </summary>
        /// <param name="types">Types to be resolved in selectors.</param>
        protected AInstanceResolver(IEnumerable<Type> defaultTypes, params Type[] types)
        {
            var allTypes = types.Concat(defaultTypes).ToArray();

            this.typesByFullName = allTypes.ToDictionary(t => t.FullName);
            this.typesByName = allTypes.ToDictionary(t => t.Name);
        }

        /// <summary>
        /// Create a Type instance from the given declarationUse
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="declarationUse"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected TInstance? CreateInstance<TInstance>(IDeclarationUse<SyntaxNode> declarationUse)
        {
            if (declarationUse == null)
            {
                throw new ArgumentNullException(nameof(declarationUse));
            }

            var instanceType = ResolveAsType(declarationUse);

            if (instanceType != null)
            {
                return (TInstance)Activator.CreateInstance(instanceType);
            }

            return default;
        }

        private Type ResolveAsType(IDeclarationUse<SyntaxNode> declarationUse)
        {
            if (declarationUse is IGenericDeclarationUse genericDeclarationUse)
            {
                var fullName = declarationUse.Declaration.FullName;
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

            return GetTypeFromName(declarationUse.Declaration.Name);
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
