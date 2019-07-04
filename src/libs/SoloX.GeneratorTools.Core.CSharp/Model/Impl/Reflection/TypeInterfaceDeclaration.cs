// ----------------------------------------------------------------------
// <copyright file="TypeInterfaceDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Reflection
{
    /// <summary>
    /// Interface declaration from Type reflection implementation.
    /// </summary>
    public class TypeInterfaceDeclaration : ATypeGenericDeclaration, IInterfaceDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeInterfaceDeclaration"/> class.
        /// </summary>
        /// <param name="type">Type to load the declaration from.</param>
        public TypeInterfaceDeclaration(Type type)
            : base(type)
        {
        }

        /// <inheritdoc/>
        public InterfaceDeclarationSyntax InterfaceDeclarationSyntax => null;

        /// <inheritdoc/>
        protected override void LoadImpl(IDeclarationResolver resolver)
        {
            this.LoadGenericParameters();
            this.LoadExtends(resolver);
            this.LoadMembers(resolver);
        }
    }
}
