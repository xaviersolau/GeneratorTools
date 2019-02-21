// ----------------------------------------------------------------------
// <copyright file="UnknownDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Unknown declaration implementation.
    /// </summary>
    public class UnknownDeclaration : ADeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownDeclaration"/> class.
        /// </summary>
        /// <param name="name">The name of the unknown declaration.</param>
        public UnknownDeclaration(string name)
            : base(string.Empty, name, null, Array.Empty<string>())
        {
        }

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            // Nothing to do.
        }
    }
}
