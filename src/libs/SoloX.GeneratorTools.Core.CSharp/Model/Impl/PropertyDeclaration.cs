// ----------------------------------------------------------------------
// <copyright file="PropertyDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Property declaration implementation.
    /// </summary>
    public class PropertyDeclaration : ANamedMemberDeclaration<PropertyDeclarationSyntax>, IPropertyDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDeclaration"/> class.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <param name="propertyType">The property type use.</param>
        /// <param name="syntaxNodeProvider">The property syntax provider.</param>
        /// <param name="attributes">Attributes attached to the property.</param>
        /// <param name="hasGetter">Tells if the property has a getter.</param>
        /// <param name="hasSetter">Tells if the property has a setter.</param>
        public PropertyDeclaration(
            string name,
            IDeclarationUse<SyntaxNode> propertyType,
            ISyntaxNodeProvider<PropertyDeclarationSyntax> syntaxNodeProvider,
            IReadOnlyList<IAttributeUse> attributes,
            bool hasGetter,
            bool hasSetter)
            : base(name, syntaxNodeProvider, attributes)
        {
            this.PropertyType = propertyType;
            this.HasGetter = hasGetter;
            this.HasSetter = hasSetter;
        }

        /// <inheritdoc/>
        public IDeclarationUse<SyntaxNode> PropertyType { get; }

        /// <inheritdoc/>
        public bool HasGetter { get; }

        /// <inheritdoc/>
        public bool HasSetter { get; }
    }
}
