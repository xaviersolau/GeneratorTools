// ----------------------------------------------------------------------
// <copyright file="ClassDeclaration.cs" company="SoloX Software">
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
    /// Class declaration implementation.
    /// </summary>
    public class ClassDeclaration : AGenericDeclaration<ClassDeclarationSyntax>, IClassDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The class declaration name space.</param>
        /// <param name="name">The class name.</param>
        /// <param name="syntaxNode">The class syntax node.</param>
        /// <param name="usingDirectives">The current using directive available for this class.</param>
        /// <param name="location">The location of the declaration.</param>
        /// <param name="loader">The class description loader.</param>
        public ClassDeclaration(
            string nameSpace,
            string name,
            ClassDeclarationSyntax syntaxNode,
            IReadOnlyList<string> usingDirectives,
            string location,
            AGenericDeclarationLoader<ClassDeclarationSyntax> loader)
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
