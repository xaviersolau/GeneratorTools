// ----------------------------------------------------------------------
// <copyright file="IArrayDeclarationUseImpl.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

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
