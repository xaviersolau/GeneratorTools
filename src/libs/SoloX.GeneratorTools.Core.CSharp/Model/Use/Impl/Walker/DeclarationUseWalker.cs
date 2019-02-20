// ----------------------------------------------------------------------
// <copyright file="DeclarationUseWalker.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker
{
    internal class DeclarationUseWalker : CSharpSyntaxVisitor<IDeclarationUse>
    {
        private IDeclarationResolver resolver;
        private AGenericDeclaration aGenericDeclaration;

        public DeclarationUseWalker(IDeclarationResolver resolver, AGenericDeclaration aGenericDeclaration)
        {
            this.resolver = resolver;
            this.aGenericDeclaration = aGenericDeclaration;
        }
    }
}
