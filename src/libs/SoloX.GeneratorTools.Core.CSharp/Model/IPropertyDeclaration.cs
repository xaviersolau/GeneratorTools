// ----------------------------------------------------------------------
// <copyright file="IPropertyDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model
{
    /// <summary>
    /// Property declaration interface.
    /// </summary>
    public interface IPropertyDeclaration : IMemberDeclaration
    {
        /// <summary>
        /// Gets the property type.
        /// </summary>
        IDeclarationUse PropertyType { get; }
    }
}
