// ----------------------------------------------------------------------
// <copyright file="ICSharpFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace
{
    /// <summary>
    /// CSharp Project or File factory.
    /// </summary>
    public interface ICSharpFactory
    {
        /// <summary>
        /// Create a ICSharpProject.
        /// </summary>
        /// <param name="file">The project file.</param>
        /// <returns>The created project object.</returns>
        ICSharpProject CreateProject(string file);

        /// <summary>
        /// Create a ICSharpFile.
        /// </summary>
        /// <param name="file">The CSharp file.</param>
        /// <returns>The created file object.</returns>
        ICSharpFile CreateFile(string file);

        /// <summary>
        /// Create a ICSharpAssembly.
        /// </summary>
        /// <param name="assembly">The CSharp assembly.</param>
        /// <returns>The created assembly object.</returns>
        ICSharpAssembly CreateAssembly(Assembly assembly);

        /// <summary>
        /// Create a ICSharpSyntaxTree.
        /// </summary>
        /// <param name="syntaxTree">The CSharp syntax tree.</param>
        /// <returns>The created syntax tree object.</returns>
        ICSharpSyntaxTree CreateSyntaxTree(SyntaxTree syntaxTree);
    }
}
