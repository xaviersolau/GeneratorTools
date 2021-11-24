// ----------------------------------------------------------------------
// <copyright file="IGeneratedItem.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.Generator
{
    /// <summary>
    /// Describe a generated item.
    /// </summary>
    public interface IGeneratedItem
    {
#pragma warning disable CA1716 // Les identificateurs ne doivent pas correspondre à des mots clés
        /// <summary>
        /// Gets the item Namespace.
        /// </summary>
        string Namespace { get; }
#pragma warning restore CA1716 // Les identificateurs ne doivent pas correspondre à des mots clés

        /// <summary>
        /// Gets the item name.
        /// </summary>
        string Name { get; }
    }
}
