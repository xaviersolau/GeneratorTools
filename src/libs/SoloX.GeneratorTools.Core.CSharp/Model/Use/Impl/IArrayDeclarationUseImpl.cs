// ----------------------------------------------------------------------
// <copyright file="IArrayDeclarationUseImpl.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl
{
    /// <summary>
    /// internal array declaration use interface.
    /// </summary>
    internal interface IArrayDeclarationUseImpl
    {
        /// <summary>
        /// Gets or sets array specification.
        /// </summary>
        IArraySpecification ArraySpecification { get; set; }
    }
}
