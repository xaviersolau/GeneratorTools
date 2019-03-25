// ----------------------------------------------------------------------
// <copyright file="IImplementationGenerator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Generator
{
    /// <summary>
    /// Implementation generator Interface.
    /// </summary>
    public interface IImplementationGenerator
    {
        /// <summary>
        /// Generate the implementation of the given interface declaration.
        /// </summary>
        /// <param name="itfDeclaration">The interface declaration to implement.</param>
        void Generate(IInterfaceDeclaration itfDeclaration);
    }
}
