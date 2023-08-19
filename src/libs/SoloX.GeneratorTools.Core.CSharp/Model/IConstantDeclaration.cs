// ----------------------------------------------------------------------
// <copyright file="IConstantDeclaration.cs" company="Xavier Solau">
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
    public interface IConstantDeclaration : IMemberDeclaration<VariableDeclaratorSyntax>
    {
        /// <summary>
        /// Gets the property type.
        /// </summary>
        IDeclarationUse<SyntaxNode> ConstantType { get; }
    }
}
