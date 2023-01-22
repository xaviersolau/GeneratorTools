// ----------------------------------------------------------------------
// <copyright file="MembersWalker.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl.Walker;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Walker
{
    internal class MembersWalker : CSharpSyntaxWalker
    {
        private readonly IDeclarationResolver resolver;
        private readonly List<IMemberDeclaration<SyntaxNode>> memberList;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;

        public MembersWalker(
            IDeclarationResolver resolver,
            IGenericDeclaration<SyntaxNode> genericDeclaration,
            List<IMemberDeclaration<SyntaxNode>> memberList)
        {
            this.resolver = resolver;
            this.memberList = memberList;
            this.genericDeclaration = genericDeclaration;
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var identifier = node.Identifier.ToString();

            var useWalker = new DeclarationUseWalker(this.resolver, this.genericDeclaration);
            var use = useWalker.Visit(node.Type);

            var canRead = false;
            var canWrite = false;

            if (node.AccessorList != null)
            {
                canRead = node.AccessorList.Accessors.FirstOrDefault(a => a.Kind() == SyntaxKind.GetAccessorDeclaration) != null;
                canWrite = node.AccessorList.Accessors.LastOrDefault(a => a.Kind() == SyntaxKind.SetAccessorDeclaration) != null;
            }
            else if (node.ExpressionBody != null)
            {
                canRead = true;
            }
            else
            {
                throw new NotImplementedException();
            }

            this.memberList.Add(new PropertyDeclaration(
                identifier,
                use,
                new ParserSyntaxNodeProvider<PropertyDeclarationSyntax>(node),
                canRead,
                canWrite));
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var identifier = node.Identifier.ToString();

            if (node.Modifiers.All(m => m.Kind() != SyntaxKind.PrivateKeyword))
            {
                var useWalker = new DeclarationUseWalker(this.resolver, this.genericDeclaration);
                var use = useWalker.Visit(node.ReturnType);
                var genericParameters = LoadGenericParameters(node);
                var parameters = LoadParameters(node, useWalker);

                this.memberList.Add(new MethodDeclaration(
                    identifier,
                    use,
                    new ParserSyntaxNodeProvider<MethodDeclarationSyntax>(node),
                    genericParameters,
                    parameters));
            }
        }

        private static IReadOnlyCollection<IParameterDeclaration> LoadParameters(MethodDeclarationSyntax node, DeclarationUseWalker useWalker)
        {
            IReadOnlyCollection<IParameterDeclaration> parameters;
            var parameterList = node.ParameterList?.Parameters;
            if (parameterList != null && parameterList.Value.Any())
            {
                var parameterSet = new List<IParameterDeclaration>();
                foreach (var parameter in parameterList.Value)
                {
                    var use = useWalker.Visit(parameter.Type);

                    parameterSet.Add(new ParameterDeclaration(
                        parameter.Identifier.Text,
                        use,
                        new ParserSyntaxNodeProvider<ParameterSyntax>(parameter)));
                }

                parameters = parameterSet;
            }
            else
            {
                parameters = Array.Empty<IParameterDeclaration>();
            }

            return parameters;
        }

        private static IReadOnlyCollection<IGenericParameterDeclaration> LoadGenericParameters(MethodDeclarationSyntax node)
        {
            IReadOnlyCollection<IGenericParameterDeclaration> genericParameters;
            var parameterList = node.TypeParameterList;
            if (parameterList != null)
            {
                var parameterSet = new List<IGenericParameterDeclaration>();
                foreach (var parameter in parameterList.Parameters)
                {
                    parameterSet.Add(new GenericParameterDeclaration(
                        parameter.Identifier.Text,
                        new ParserSyntaxNodeProvider<TypeParameterSyntax>(parameter)));
                }

                genericParameters = parameterSet;
            }
            else
            {
                genericParameters = Array.Empty<IGenericParameterDeclaration>();
            }

            return genericParameters;
        }
    }
}
