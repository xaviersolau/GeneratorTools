// ----------------------------------------------------------------------
// <copyright file="GenericResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System.Linq;
using System.Collections.Generic;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata.Provider
{
    internal class GenericResolver
    {
        private readonly IGenericDeclaration<SyntaxNode> declaration;
        private readonly IReadOnlyCollection<IGenericParameterDeclaration> genericMethodParameters;

        public GenericResolver(
            IGenericDeclaration<SyntaxNode> declaration,
            IReadOnlyCollection<IGenericParameterDeclaration> genericMethodParameters = null)
        {
            this.declaration = declaration;
            this.genericMethodParameters = genericMethodParameters;
        }

        public IDeclarationUse<SyntaxNode> ResolveMethodParameter(int index)
        {
            var genericParameter = this.genericMethodParameters.ElementAt(index);

            return new GenericParameterDeclarationUse(null, genericParameter);
        }

        public IDeclarationUse<SyntaxNode> ResolveParameter(int index)
        {
            var genericParameter = this.declaration.GenericParameters.ElementAt(index);

            return new GenericParameterDeclarationUse(null, genericParameter);
        }
    }
}
