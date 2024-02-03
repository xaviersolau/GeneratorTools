// ----------------------------------------------------------------------
// <copyright file="AutomatedMethodStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedMethodStrategy : IAutomatedStrategy
    {
        private readonly IMethodDeclaration pattern;
        private readonly IMethodDeclaration declaration;
        private readonly IDeclarationResolver resolver;
        private readonly IGenericDeclaration<SyntaxNode> genericDeclaration;
        private readonly IReplacePatternResolver replacePatternResolver;
        private readonly TextPatternHelper textReplaceHelper;
        private readonly TextPatternHelper typeReplaceHelper;

        public AutomatedMethodStrategy(
            IMethodDeclaration pattern,
            IMethodDeclaration declaration,
            IDeclarationResolver resolver,
            IGenericDeclaration<SyntaxNode> genericDeclaration,
            string patternPrefix, string patternSuffix,
            IReplacePatternResolver replacePatternResolver)
        {
            this.pattern = pattern;
            this.declaration = declaration;
            this.resolver = resolver;
            this.genericDeclaration = genericDeclaration;
            this.replacePatternResolver = replacePatternResolver;
            var patternName = this.pattern.Name;
            var declarationName = this.declaration.Name;

            var patternTypeName = this.pattern.ReturnType.SyntaxNodeProvider.SyntaxNode.ToString();
            var declarationTypeName = this.declaration.ReturnType.SyntaxNodeProvider.SyntaxNode.ToString();

            this.textReplaceHelper = new TextPatternHelper(patternName, declarationName, patternPrefix, patternSuffix);
            this.typeReplaceHelper = new TextPatternHelper(patternTypeName, declarationTypeName, patternPrefix, patternSuffix);
        }

        public IReplacePatternHandler CreateReplacePatternHandler()
        {
            return new StrategyReplacePatternHandler(ApplyPatternReplace);
        }

        public IReplacePatternHandler CreateReplacePatternHandler(AttributeSyntax replacePatternAttributeSyntax)
        {
            var replaceHandlerTypeExpression = replacePatternAttributeSyntax.ArgumentList.Arguments.First().Expression;

            var constEvaluator = new ConstantExpressionSyntaxEvaluator<IDeclarationUse<SyntaxNode>>(this.resolver, this.genericDeclaration);
            var typeUse = constEvaluator.Visit(replaceHandlerTypeExpression);

            var factory = this.replacePatternResolver.GetHandlerFactory(typeUse);

            return factory.Setup(this.pattern, this.declaration);
        }

        private string ApplyPatternReplace(string text)
        {
            var result = this.textReplaceHelper.ReplacePattern(text);
            result = this.typeReplaceHelper.ReplacePattern(result);

            return result;
        }

        public string ComputeTargetName()
        {
            return this.declaration.Name;
        }

        public string GetCurrentNameSpace()
        {
            throw new NotImplementedException();
        }

        public bool IgnoreUsingDirective(string ns)
        {
            throw new NotImplementedException();
        }

        public void RepeatDeclaration(AttributeSyntax repeatAttributeSyntax, Action<IAutomatedStrategy> callback)
        {
            var patternNameExpression = repeatAttributeSyntax.ArgumentList.Arguments.First().Expression;

            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>(this.resolver, this.genericDeclaration);
            var patternName = constEvaluator.Visit(patternNameExpression);

            var patternParameter = this.pattern.Parameters.Single(x => x.Name == patternName);

            var patternPrefixExp = repeatAttributeSyntax.ArgumentList
                .Arguments.FirstOrDefault(a => a.NameEquals.Name.ToString() == nameof(RepeatAttribute.Prefix))?.Expression;
            var patternSuffixExp = repeatAttributeSyntax.ArgumentList
                .Arguments.FirstOrDefault(a => a.NameEquals.Name.ToString() == nameof(RepeatAttribute.Suffix))?.Expression;

            var patternPrefix = patternPrefixExp != null ? constEvaluator.Visit(patternPrefixExp) : null;
            var patternSuffix = patternSuffixExp != null ? constEvaluator.Visit(patternSuffixExp) : null;


            var strategy = new AutomatedParameterStrategy(patternParameter, this.declaration.Parameters, this.resolver, this.genericDeclaration, patternPrefix, patternSuffix);

            strategy.TryMatchAndRepeatStatement(patternNameExpression, patternPrefixExp, patternSuffixExp, callback);
        }

        public void RepeatNameSpace(Action<string> nsCallback)
        {
            throw new NotImplementedException();
        }

        public bool TryMatchAndRepeatStatement(
            SyntaxNode? patternNameExpression,
            SyntaxNode? patternPrefixExpression,
            SyntaxNode? patternSuffixExpression,
            Action<IAutomatedStrategy> callback)
        {
            return false;
        }
    }

#pragma warning disable CA1062 // Validate arguments of public methods
    /// <summary>
    /// Expression visitor.
    /// </summary>
    public class ExpressionVisitor : CSharpSyntaxVisitor<bool>
    {
        private readonly Func<string, bool> match;

        /// <summary>
        /// Setup instance with match handler.
        /// </summary>
        /// <param name="match"></param>
        public ExpressionVisitor(Func<string, bool> match)
        {
            this.match = match;
        }

        /// <inheritdoc/>
        public override bool VisitIdentifierName(IdentifierNameSyntax node)
        {
            return this.match(node.Identifier.Text);
        }
    }
#pragma warning restore CA1062 // Validate arguments of public methods
}
