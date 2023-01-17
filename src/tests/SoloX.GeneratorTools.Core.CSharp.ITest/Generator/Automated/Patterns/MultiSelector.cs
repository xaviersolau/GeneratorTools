// ----------------------------------------------------------------------
// <copyright file="MultiSelector.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Samples;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns
{
    /// <summary>
    /// Selector that always return the ISimpleSample descriptor. (For test purpose.)
    /// </summary>
    public class MultiSelector : ISelector
    {
        public IEnumerable<IDeclaration<SyntaxNode>> GetDeclarations(IEnumerable<ICSharpFile> files)
        {
            return files
                .SelectMany(file => file.Declarations)
                .Where(d => d.Name == nameof(ISimpleSample) || d.Name == nameof(IOtherSample));
        }

        public IEnumerable<IMethodDeclaration> GetMethods(IGenericDeclaration<SyntaxNode> declaration)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPropertyDeclaration> GetProperties(IGenericDeclaration<SyntaxNode> declaration)
        {
            throw new NotImplementedException();
        }
    }
}
