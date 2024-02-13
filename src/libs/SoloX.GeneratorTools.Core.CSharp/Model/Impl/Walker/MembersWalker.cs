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
using Microsoft.CodeAnalysis.Text;
using SoloX.GeneratorTools.Core.CSharp.Exceptions;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
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

            if (use == null)
            {
                throw new ParserException("Unable to load Declaration use.", node.Type);
            }

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

            var attributeList = new List<IAttributeUse>();
            var attributesWalker = new AttributesWalker(this.resolver, this.genericDeclaration, attributeList);

            attributesWalker.Visit(node);

            var attributes = attributeList.Count > 0 ? attributeList.ToArray() : Array.Empty<IAttributeUse>();

            this.memberList.Add(new PropertyDeclaration(
                identifier,
                use,
                new ParserSyntaxNodeProvider<PropertyDeclarationSyntax>(node),
                attributes,
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

                if (use == null)
                {
                    throw new ParserException("Unable to load Declaration use.", node.ReturnType);
                }

                var genericParameters = LoadGenericParameters(node);
                var parameters = LoadParameters(node, useWalker);

                var attributeList = new List<IAttributeUse>();
                var attributesWalker = new AttributesWalker(this.resolver, this.genericDeclaration, attributeList);
                foreach (var attributeItem in node.AttributeLists)
                {
                    if (attributeItem.Target == null)
                    {
                        attributesWalker.Visit(attributeItem);
                    }
                }

                var attributes = attributeList.Count > 0 ? attributeList.ToArray() : Array.Empty<IAttributeUse>();

                var returnAttributeList = new List<IAttributeUse>();
                var returnAttributesWalker = new AttributesWalker(this.resolver, this.genericDeclaration, returnAttributeList);
                foreach (var attributeItem in node.AttributeLists)
                {
                    if (attributeItem.Target != null)
                    {
                        returnAttributesWalker.Visit(attributeItem);
                    }
                }

                this.memberList.Add(new MethodDeclaration(
                    identifier,
                    use,
                    new ParserSyntaxNodeProvider<MethodDeclarationSyntax>(node),
                    genericParameters,
                    parameters,
                    attributes,
                    returnAttributeList));
            }
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            if (node.Modifiers.Any(m => m.Kind() == SyntaxKind.ConstKeyword))
            {
                var useWalker = new DeclarationUseWalker(this.resolver, this.genericDeclaration);
                var use = useWalker.Visit(node.Declaration.Type);

                if (use == null)
                {
                    throw new ParserException("Unable to load Declaration use.", node.Declaration.Type);
                }

                foreach (var variableItem in node.Declaration.Variables)
                {
                    var identifier = variableItem.Identifier.ToString();

                    var attributeList = new List<IAttributeUse>();
                    var attributesWalker = new AttributesWalker(this.resolver, this.genericDeclaration, attributeList);

                    attributesWalker.Visit(node);

                    var attributes = attributeList.Count > 0 ? attributeList.ToArray() : Array.Empty<IAttributeUse>();

                    this.memberList.Add(new ConstantDeclaration(
                        identifier,
                        use,
                        new ParserSyntaxNodeProvider<VariableDeclaratorSyntax>(variableItem),
                        attributes));
                }
            }
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            this.Visit(node.ParameterList);
        }

        public override void VisitParameterList(ParameterListSyntax node)
        {
            foreach (var parameter in node.Parameters)
            {
                Visit(parameter);
            }
        }

        public override void VisitParameter(ParameterSyntax node)
        {
            var identifier = node.Identifier.ToString();

            var useWalker = new DeclarationUseWalker(this.resolver, this.genericDeclaration);
            var use = useWalker.Visit(node.Type);

            if (use == null)
            {
                throw new ParserException("Unable to load Declaration use.", node.Type);
            }

            var canRead = true;
            var canWrite = false;

            var attributeList = new List<IAttributeUse>();
            var attributesWalker = new AttributesWalker(this.resolver, this.genericDeclaration, attributeList);

            attributesWalker.Visit(node);

            var attributes = attributeList.Count > 0 ? attributeList.ToArray() : Array.Empty<IAttributeUse>();

            this.memberList.Add(new PropertyDeclaration(
                identifier,
                use,
                new ParserSyntaxNodeProvider<PropertyDeclarationSyntax>(GeneratePropertyDeclarationSyntax(node.Type.ToString(), identifier)),
                attributes,
                canRead,
                canWrite));
        }

        private static PropertyDeclarationSyntax GeneratePropertyDeclarationSyntax(string typeText, string identifier)
        {
            var node = GetSyntaxNode($"public {typeText} {identifier} {{ get; }}");
            return (PropertyDeclarationSyntax)((CompilationUnitSyntax)node).Members.Single();
        }

        private static SyntaxNode GetSyntaxNode(string text)
        {
            var src = SourceText.From(text);
            var syntaxTree = CSharpSyntaxTree.ParseText(src);

            return syntaxTree.GetRoot();
        }

        private IReadOnlyCollection<IParameterDeclaration> LoadParameters(MethodDeclarationSyntax node, DeclarationUseWalker useWalker)
        {
            IReadOnlyCollection<IParameterDeclaration> parameters;
            var parameterList = node.ParameterList?.Parameters;
            if (parameterList != null && parameterList.Value.Any())
            {
                var parameterSet = new List<IParameterDeclaration>();
                foreach (var parameter in parameterList.Value)
                {
                    var use = useWalker.Visit(parameter.Type);

                    var parameterDeclaration = new ParameterDeclaration(
                        parameter.Identifier.Text,
                        use,
                        new ParserSyntaxNodeProvider<ParameterSyntax>(parameter));

                    var attributeList = new List<IAttributeUse>();
                    var attributesWalker = new AttributesWalker(this.resolver, this.genericDeclaration, attributeList);

                    attributesWalker.Visit(parameter);

                    var attributes = attributeList.Count > 0 ? attributeList.ToArray() : Array.Empty<IAttributeUse>();

                    parameterDeclaration.Attributes = attributes;

                    parameterSet.Add(parameterDeclaration);
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
