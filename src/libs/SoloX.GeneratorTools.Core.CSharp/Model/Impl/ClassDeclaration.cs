// ----------------------------------------------------------------------
// <copyright file="ClassDeclaration.cs" company="Xavier Solau">
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
    /// Class declaration implementation.
    /// </summary>
    public class ClassDeclaration : AGenericDeclaration<ClassDeclarationSyntax>, IClassDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The class declaration name space.</param>
        /// <param name="name">The class name.</param>
        /// <param name="syntaxNodeProvider">The class syntax node provider.</param>
        /// <param name="usingDirectives">The current using directive available for this class.</param>
        /// <param name="location">The location of the declaration.</param>
        /// <param name="loader">The class description loader.</param>
        public ClassDeclaration(
            string nameSpace,
            string name,
            ISyntaxNodeProvider<ClassDeclarationSyntax> syntaxNodeProvider,
            IUsingDirectives usingDirectives,
            string location,
            AGenericDeclarationLoader<ClassDeclarationSyntax> loader)
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
