// ----------------------------------------------------------------------
// <copyright file="InterfaceDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Interface declaration implementation.
    /// </summary>
    public class InterfaceDeclaration : AGenericDeclaration<InterfaceDeclarationSyntax>, IInterfaceDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The interface declaration name space.</param>
        /// <param name="name">The interface name.</param>
        /// <param name="syntaxNodeProvider">The interface syntax node provider.</param>
        /// <param name="usingDirectives">The current using directive available for this interface.</param>
        /// <param name="location">The location of the declaration.</param>
        /// <param name="loader">The interface declaration loader.</param>
        public InterfaceDeclaration(
            string nameSpace,
            string name,
            ISyntaxNodeProvider<InterfaceDeclarationSyntax> syntaxNodeProvider,
            IUsingDirectives usingDirectives,
            string location,
            AGenericDeclarationLoader<InterfaceDeclarationSyntax> loader)
            : base(
                  nameSpace,
                  name,
                  syntaxNodeProvider,
                  usingDirectives,
                  location,
                  loader,
                  false)
        {
        }
    }
}
