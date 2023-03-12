// ----------------------------------------------------------------------
// <copyright file="IMetadataDeclarationFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System.Reflection.Metadata;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Declaration factory used to create declaration instances depending the Metadata load.
    /// </summary>
    public interface IMetadataDeclarationFactory
    {
        /// <summary>
        /// Create a declaration from the given typeDefinitionHandle.
        /// </summary>
        /// <param name="metadataReader">Metadata reader.</param>
        /// <param name="typeDefinitionHandle">Type definition.</param>
        /// <param name="location">Location of the declaration.</param>
        /// <returns></returns>
        IDeclaration<SyntaxNode> CreateDeclaration(MetadataReader metadataReader, TypeDefinitionHandle typeDefinitionHandle, string location);
    }
}
