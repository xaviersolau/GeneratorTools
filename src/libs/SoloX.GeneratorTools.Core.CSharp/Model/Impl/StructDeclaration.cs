// ----------------------------------------------------------------------
// <copyright file="StructDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Structure declaration implementation.
    /// </summary>
    public class StructDeclaration : AGenericDeclaration<StructDeclarationSyntax>, IStructDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The structure declaration name space.</param>
        /// <param name="name">The structure name.</param>
        /// <param name="syntaxNode">The structure syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this structure.</param>
        /// <param name="location">The location of the declaration.</param>
        /// <param name="loader">The class description loader.</param>
        public StructDeclaration(
            string nameSpace,
            string name,
            StructDeclarationSyntax syntaxNode,
            IReadOnlyList<string> usingDirectives,
            string location,
            AGenericDeclarationLoader<StructDeclarationSyntax> loader)
            : base(
                  nameSpace,
                  name,
                  syntaxNode,
                  usingDirectives,
                  location,
                  loader)
        {
        }
    }
}
