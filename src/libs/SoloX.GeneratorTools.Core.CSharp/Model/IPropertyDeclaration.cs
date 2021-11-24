// ----------------------------------------------------------------------
// <copyright file="IPropertyDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Property declaration interface.
    /// </summary>
    public interface IPropertyDeclaration : IMemberDeclaration<PropertyDeclarationSyntax>
    {
        /// <summary>
        /// Gets the property type.
        /// </summary>
        IDeclarationUse<SyntaxNode> PropertyType { get; }

        /// <summary>
        /// Gets a value indicating whether tells if the property has a getter.
        /// </summary>
        bool HasGetter { get; }

        /// <summary>
        /// Gets a value indicating whether tells if the property has a setter.
        /// </summary>
        bool HasSetter { get; }
    }
}
