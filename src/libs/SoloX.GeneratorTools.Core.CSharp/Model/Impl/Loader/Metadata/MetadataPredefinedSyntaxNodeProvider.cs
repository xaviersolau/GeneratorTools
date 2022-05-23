﻿// ----------------------------------------------------------------------
// <copyright file="MetadataPredefinedSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataPredefinedSyntaxNodeProvider : AReflectionSyntaxNodeProvider<PredefinedTypeSyntax>
    {
        private readonly string typeName;

        public MetadataPredefinedSyntaxNodeProvider(string typeName)
        {
            this.typeName = typeName;
        }

        protected override PredefinedTypeSyntax Generate()
        {
            throw new NotImplementedException();
        }
    }
}
