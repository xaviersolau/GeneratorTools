// ----------------------------------------------------------------------
// <copyright file="ReadOnlyPropertySelector.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Selectors
{
    /// <summary>
    /// Read only property selector.
    /// </summary>
    public class ReadOnlyPropertySelector : ISelector
    {
        /// <inheritdoc/>
        public IEnumerable<IDeclaration<SyntaxNode>> GetDeclarations(IEnumerable<ICSharpFile> files)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<IPropertyDeclaration> GetProperties(IGenericDeclaration<SyntaxNode> declaration)
        {
            return declaration?.Properties.Where(p => p.HasGetter && !p.HasSetter);
        }
    }
}
