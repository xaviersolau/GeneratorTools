// ----------------------------------------------------------------------
// <copyright file="EnumDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Enum declaration implementation.
    /// </summary>
    public class EnumDeclaration : ADeclaration<EnumDeclarationSyntax>, IEnumDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The enum declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <param name="syntaxNodeProvider">The declaration syntax node provider.</param>
        /// <param name="usingDirectives">The current using directive available for this enum.</param>
        /// <param name="location">The location of the declaration.</param>
        public EnumDeclaration(
            string nameSpace,
            string name,
            ISyntaxNodeProvider<EnumDeclarationSyntax> syntaxNodeProvider,
            IReadOnlyList<string> usingDirectives,
            string location)
            : base(nameSpace, name, syntaxNodeProvider, usingDirectives, location)
        {
        }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            // Nothing to load...
        }
    }
}
