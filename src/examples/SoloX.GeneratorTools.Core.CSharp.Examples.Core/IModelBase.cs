// ----------------------------------------------------------------------
// <copyright file="IModelBase.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Core
{
    /// <summary>
    /// Model base interface that defines a IsDirty property.
    /// </summary>
    public interface IModelBase
    {
        /// <summary>
        /// Gets a value indicating whether the model is dirty or not.
        /// The model is going to be dirty as soon as one of its property is set.
        /// </summary>
        bool IsDirty { get; }
    }
}
