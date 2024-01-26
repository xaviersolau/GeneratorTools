// ----------------------------------------------------------------------
// <copyright file="AEnumDeclarationLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader
{
    /// <summary>
    /// Enum Declaration Loader.
    /// </summary>
    public abstract class AEnumDeclarationLoader
    {
        /// <summary>
        /// Load the given declaration.
        /// </summary>
        /// <param name="declaration">The generic declaration to load.</param>
        /// <param name="resolver">The declaration resolver.</param>
        internal abstract void Load(EnumDeclaration declaration, IDeclarationResolver resolver);
    }
}
