// ----------------------------------------------------------------------
// <copyright file="ParserEnumDeclarationLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Exceptions;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser
{
    internal class ParserEnumDeclarationLoader : AEnumDeclarationLoader
    {
        private readonly IGeneratorLogger<ParserEnumDeclarationLoader> logger;

        public ParserEnumDeclarationLoader(IGeneratorLogger<ParserEnumDeclarationLoader> logger)
        {
            this.logger = logger;
        }

        internal override void Load(EnumDeclaration declaration, IDeclarationResolver resolver)
        {
            var baseListSyntax = declaration.SyntaxNodeProvider.SyntaxNode.BaseList;

            if (baseListSyntax != null)
            {
                var baseWalker = new DeclarationUseWalker(resolver, null);
                var uses = new List<IDeclarationUse<SyntaxNode>>();

                foreach (var node in baseListSyntax.ChildNodes())
                {
                    var use = baseWalker.Visit(node);

                    if (use == null)
                    {
                        throw new ParserException("Unable to load Declaration use.", node);
                    }

                    uses.Add(use);
                }

                if (uses.Count != 1)
                {
                    this.logger.LogError($"Expected only one type as extension for {declaration.FullName}.");
                }

                declaration.UnderlyingType = uses.Single();
            }
        }
    }
}
