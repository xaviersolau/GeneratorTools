// ----------------------------------------------------------------------
// <copyright file="DeclarationResolver.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Workspace;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Resolver.Impl
{
    /// <summary>
    /// Declaration resolver implementation.
    /// </summary>
    public class DeclarationResolver : IDeclarationResolver
    {
        private readonly Action<IDeclarationResolver, IDeclaration> loader;

        private readonly Dictionary<string, List<IDeclaration>> declarationMap = new Dictionary<string, List<IDeclaration>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DeclarationResolver"/> class.
        /// </summary>
        /// <param name="declarations">The declaration list the resolver is based on.</param>
        /// <param name="loader">The loader delegate to load a declaration is needed.</param>
        public DeclarationResolver(
            IEnumerable<IDeclaration> declarations, Action<IDeclarationResolver, IDeclaration> loader)
        {
            this.loader = loader;

            this.Setup(declarations);
        }

        /// <inheritdoc/>
        public IEnumerable<IDeclaration> Find(string fullName)
        {
            return this.declarationMap.TryGetValue(fullName, out var declarations) ? declarations : null;
        }

        /// <inheritdoc/>
        public IGenericDeclaration Resolve(
            string identifier, IReadOnlyList<IDeclarationUse> genericParameters, IDeclaration declarationContext)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IDeclaration Resolve(string identifier, IDeclaration declarationContext)
        {
            throw new NotImplementedException();
        }

        private void Setup(IEnumerable<IDeclaration> declarations)
        {
            foreach (var declaration in declarations)
            {
                var fullName = $"{declaration.DeclarationNameSpace}.{declaration.Name}";
                if (!this.declarationMap.TryGetValue(fullName, out var list))
                {
                    list = new List<IDeclaration>();
                    this.declarationMap.Add(fullName, list);
                }

                list.Add(declaration);
            }
        }
    }
}
