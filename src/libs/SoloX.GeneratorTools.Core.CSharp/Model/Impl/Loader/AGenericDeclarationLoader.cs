// ----------------------------------------------------------------------
// <copyright file="AGenericDeclarationLoader.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader
{
    /// <summary>
    /// Generic Declaration Loader.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public abstract class AGenericDeclarationLoader<TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Load the given declaration.
        /// </summary>
        /// <param name="declaration">The generic declaration to load.</param>
        /// <param name="resolver">The declaration resolver.</param>
        internal abstract void Load(AGenericDeclaration<TNode> declaration, IDeclarationResolver resolver);

        /// <summary>
        /// Get the TypeParameterListSyntaxProvider for the given.
        /// </summary>
        /// <param name="declaration">The generic declaration to load.</param>
        /// <returns>The TypeParameterListSyntaxProvider.</returns>
        internal abstract ISyntaxNodeProvider<TypeParameterListSyntax> GetTypeParameterListSyntaxProvider(AGenericDeclaration<TNode> declaration);
    }
}
