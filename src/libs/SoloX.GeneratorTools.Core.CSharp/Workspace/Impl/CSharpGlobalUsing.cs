// ----------------------------------------------------------------------
// <copyright file="CSharpGlobalUsing.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Model;
using System.Collections.Generic;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Using directives store.
    /// </summary>
    public class CSharpGlobalUsing : IGlobalUsingDirectives
    {
        private readonly HashSet<string> usingDirectives = new HashSet<string>();

        /// <inheritdoc/>
        public void Register(string usingDirective)
        {
            this.usingDirectives.Add(usingDirective);
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Usings => this.usingDirectives.ToArray();
    }
}
