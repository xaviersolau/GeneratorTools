// ----------------------------------------------------------------------
// <copyright file="PropertyDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Property declaration implementation.
    /// </summary>
    public class PropertyDeclaration : AMemberDeclaration, IPropertyDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDeclaration"/> class.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <param name="propertyType">The property type use.</param>
        /// <param name="syntaxNode">The property syntax.</param>
        public PropertyDeclaration(string name, IDeclarationUse propertyType, CSharpSyntaxNode syntaxNode)
            : base(name, syntaxNode)
        {
            this.PropertyType = propertyType;
        }

        /// <inheritdoc/>
        public IDeclarationUse PropertyType { get; }
    }
}
