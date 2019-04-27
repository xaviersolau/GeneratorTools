// ----------------------------------------------------------------------
// <copyright file="ModelPattern.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Impl
{
    /// <summary>
    /// Model pattern implementation.
    /// </summary>
    public class ModelPattern : IModelPattern
    {
        private object propertyPattern;

        /// <inheritdoc/>
        public bool IsDirty
        { get; private set; }

        /// <inheritdoc/>
        public object PropertyPattern
        {
            get
            {
                return this.propertyPattern;
            }

            set
            {
                this.propertyPattern = value;
                this.IsDirty = true;
            }
        }
    }
}
