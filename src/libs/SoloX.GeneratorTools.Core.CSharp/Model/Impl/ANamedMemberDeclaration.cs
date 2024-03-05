// ----------------------------------------------------------------------
// <copyright file="ANamedMemberDeclaration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System.Collections.Generic;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Base abstract member declaration implementation.
    /// </summary>
    /// <typeparam name="TNode">Syntax Node type (based on SyntaxNode).</typeparam>
    public class ANamedMemberDeclaration<TNode> : AMemberDeclaration<TNode>, INamedMemberDeclaration<TNode>
        where TNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMemberDeclaration{TNode}"/> class.
        /// </summary>
        /// <param name="name">The member name.</param>
        /// <param name="syntaxNodeProvider">The member syntax node provider.</param>
        /// <param name="attributes">Attributes attached to the member.</param>
        protected ANamedMemberDeclaration(string name, ISyntaxNodeProvider<TNode> syntaxNodeProvider, IReadOnlyList<IAttributeUse> attributes)
            : base(syntaxNodeProvider, attributes)
        {
            this.Name = name;
        }

        /// <inheritdoc/>
        public string Name { get; }
    }
}
