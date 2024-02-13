// ----------------------------------------------------------------------
// <copyright file="EnumDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Enum declaration implementation.
    /// </summary>
    public class EnumDeclaration : ADeclaration<EnumDeclarationSyntax>, IEnumDeclaration
    {
        private readonly AEnumDeclarationLoader loader;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDeclaration"/> class.
        /// </summary>
        /// <param name="nameSpace">The enum declaration name space.</param>
        /// <param name="name">The declaration name.</param>
        /// <param name="syntaxNodeProvider">The declaration syntax node provider.</param>
        /// <param name="usingDirectives">The current using directive available for this enum.</param>
        /// <param name="location">The location of the declaration.</param>
        /// <param name="loader">The loader to use when deep loading the declaration.</param>
        public EnumDeclaration(
            string nameSpace,
            string name,
            ISyntaxNodeProvider<EnumDeclarationSyntax> syntaxNodeProvider,
            IUsingDirectives usingDirectives,
            string location,
            AEnumDeclarationLoader loader)
            : base(nameSpace, name, syntaxNodeProvider, usingDirectives, location, true, false)
        {
            this.loader = loader;
        }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            this.loader.Load(this, resolver);
        }

        /// <inheritdoc/>
        public IDeclarationUse<SyntaxNode>? UnderlyingType { get; internal set; }
    }
}
