// ----------------------------------------------------------------------
// <copyright file="ICSharpLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// ICSharpLoader interface used to load workspace elements.
    /// </summary>
    public interface ICSharpLoader
    {
        /// <summary>
        /// Load a given project.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="project">The project to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpProject project);

        /// <summary>
        /// Load a given file.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="file">The file to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpFile file);

        /// <summary>
        /// Load a given syntaxTree.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="syntaxTree">The syntaxTree to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpSyntaxTree syntaxTree);

        /// <summary>
        /// Load the given assembly.
        /// </summary>
        /// <param name="workspace">The context workspace.</param>
        /// <param name="assembly">The assembly to load.</param>
        void Load(ICSharpWorkspace workspace, ICSharpAssembly assembly);

        /// <summary>
        /// Load the given CSharp declaration.
        /// </summary>
        /// <param name="resolver">The declaration resolver to resolve declaration dependencies.</param>
        /// <param name="declaration">The Declaration to load.</param>
        void Load(IDeclarationResolver resolver, IDeclaration<SyntaxNode> declaration);
    }
}
