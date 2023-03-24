// ----------------------------------------------------------------------
// <copyright file="CSharpUsingDirectives.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker
{
    /// <summary>
    /// Using declaration store.
    /// </summary>
    public class CSharpUsingDirectives : IUsingDirectives
    {
        private readonly IGlobalUsingDirectives globalUsing;
        private readonly IReadOnlyList<string> usingDirectives;

        /// <summary>
        /// Setup instance.
        /// </summary>
        /// <param name="globalUsing"></param>
        /// <param name="usingDirectives"></param>
        public CSharpUsingDirectives(IGlobalUsingDirectives globalUsing, IReadOnlyList<string> usingDirectives)
        {
            this.globalUsing = globalUsing;
            this.usingDirectives = usingDirectives;
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Usings => new HashSet<string>(this.globalUsing.Usings.Concat(this.usingDirectives)).ToList();
    }
}
