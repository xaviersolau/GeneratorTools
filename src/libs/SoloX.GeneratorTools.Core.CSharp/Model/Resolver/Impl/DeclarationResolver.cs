// ----------------------------------------------------------------------
// <copyright file="DeclarationResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Resolver.Impl
{
    /// <summary>
    /// Declaration resolver implementation.
    /// </summary>
    public class DeclarationResolver : IDeclarationResolver
    {
        private readonly Dictionary<string, List<IDeclaration<SyntaxNode>>> declarationMap = new Dictionary<string, List<IDeclaration<SyntaxNode>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DeclarationResolver"/> class.
        /// </summary>
        /// <param name="declarations">The declaration list the resolver is based on.</param>
        public DeclarationResolver(IEnumerable<IDeclaration<SyntaxNode>> declarations)
        {
            if (declarations == null)
            {
                throw new ArgumentNullException(nameof(declarations), $"The argument {nameof(declarations)} was null.");
            }

            this.Setup(declarations);
        }

        /// <inheritdoc/>
        public IGenericDeclaration<SyntaxNode> Resolve(
            string identifier, IReadOnlyList<IDeclarationUse<SyntaxNode>> genericParameters, IDeclaration<SyntaxNode> declarationContext)
        {
            var declarations = this.FindDeclarations(identifier, declarationContext);
            if (declarations != null)
            {
                var tParamCount = genericParameters == null ? 0 : genericParameters.Count;
                foreach (var declarationItem in declarations)
                {
                    // Make sure the declaration is loaded
                    declarationItem.DeepLoad(this);

                    if (declarationItem is IGenericDeclaration<SyntaxNode> gd && tParamCount == gd.GenericParameters.Count)
                    {
                        // TODO take into account the type parameter constraints.
                        return gd;
                    }
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public IGenericDeclaration<SyntaxNode> Resolve(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), $"The argument {nameof(type)} was null.");
            }

            var fullName = type.FullName;
            if (fullName == null)
            {
                return null;
            }

            fullName = ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(fullName);

            var declarations = this.FindDeclarations(fullName, null);
            if (declarations != null)
            {
                var tParamCount = type.GetTypeInfo().GenericTypeParameters.Length;
                foreach (var declarationItem in declarations)
                {
                    // Make sure the declaration is loaded
                    declarationItem.DeepLoad(this);

                    if (declarationItem is IGenericDeclaration<SyntaxNode> gd && tParamCount == gd.GenericParameters.Count)
                    {
                        // TODO take into account the type parameter constraints.
                        return gd;
                    }
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public IDeclaration<SyntaxNode> Resolve(string identifier, IDeclaration<SyntaxNode> declarationContext)
        {
            var declarations = this.FindDeclarations(identifier, declarationContext);
            if (declarations != null)
            {
                foreach (var declarationItem in declarations)
                {
                    // Make sure the declaration is loaded
                    declarationItem.DeepLoad(this);

                    if (declarationItem is IGenericDeclaration<SyntaxNode> gd)
                    {
                        if (gd.GenericParameters.Count == 0)
                        {
                            return gd;
                        }
                    }
                    else
                    {
                        return declarationItem;
                    }
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public IEnumerable<IDeclaration<SyntaxNode>> Find(string fullName)
        {
            return this.declarationMap.TryGetValue(fullName, out var declarations) ? declarations : null;
        }

        /// <summary>
        /// Load all declarations.
        /// </summary>
        internal void Load()
        {
            foreach (var declarationItem in this.declarationMap)
            {
                var declarationList = declarationItem.Value;
                foreach (var declaration in declarationList)
                {
                    declaration.DeepLoad(this);
                }
            }
        }

        private IEnumerable<IDeclaration<SyntaxNode>> FindDeclarations(
            string identifier, IDeclaration<SyntaxNode> declarationContext)
        {
            List<IDeclaration<SyntaxNode>> declarations;
            if (declarationContext != null)
            {
                foreach (var usingDirective in declarationContext.UsingDirectives)
                {
                    var lookupName = ADeclaration<SyntaxNode>.GetFullName(usingDirective, identifier);

                    if (this.declarationMap.TryGetValue(lookupName, out declarations))
                    {
                        return declarations;
                    }
                }

                foreach (var nameSpace in NameSpaceHelper.GetParentNameSpaces(declarationContext.DeclarationNameSpace))
                {
                    if (this.declarationMap.TryGetValue(
                        ADeclaration<SyntaxNode>.GetFullName(nameSpace, identifier),
                        out declarations))
                    {
                        return declarations;
                    }
                }
            }

            if (this.declarationMap.TryGetValue(identifier, out declarations))
            {
                return declarations;
            }

            return null;
        }

        private void Setup(IEnumerable<IDeclaration<SyntaxNode>> declarations)
        {
            foreach (var declaration in declarations)
            {
                var fullName = ADeclaration<SyntaxNode>.GetFullName(declaration.DeclarationNameSpace, declaration.Name);

                if (!this.declarationMap.TryGetValue(fullName, out var list))
                {
                    list = new List<IDeclaration<SyntaxNode>>();
                    this.declarationMap.Add(fullName, list);
                }

                list.Add(declaration);
            }
        }
    }
}
