﻿// ----------------------------------------------------------------------
// <copyright file="MetadataMethodSyntaxNodeProvider.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Metadata
{
    internal class MetadataMethodSyntaxNodeProvider : AMetadataSyntaxNodeProvider<MethodDeclarationSyntax>
    {
        protected override MethodDeclarationSyntax Generate()
        {
            throw new NotImplementedException();
        }
    }
}
