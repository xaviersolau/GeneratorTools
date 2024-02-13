// ----------------------------------------------------------------------
// <copyright file="RecordDeclaration.cs" company="Xavier Solau">
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
    /// Record declaration implementation.
    /// </summary>
    public class RecordDeclaration : AGenericDeclaration<RecordDeclarationSyntax>, IRecordDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The Record declaration name space.</param>
        /// <param name="name">The Record name.</param>
        /// <param name="syntaxNodeProvider">The Record syntax node provider.</param>
        /// <param name="usingDirectives">The current using directive available for this class.</param>
        /// <param name="location">The location of the declaration.</param>
        /// <param name="loader">The class description loader.</param>
        public RecordDeclaration(
            string nameSpace,
            string name,
            ISyntaxNodeProvider<RecordDeclarationSyntax> syntaxNodeProvider,
            IUsingDirectives usingDirectives,
            string location,
            AGenericDeclarationLoader<RecordDeclarationSyntax> loader)
            : base(
                  nameSpace,
                  name,
                  syntaxNodeProvider,
                  usingDirectives,
                  location,
                  loader,
                  false,
                  true)
        {
        }
    }
}
