﻿// ----------------------------------------------------------------------
// <copyright file="AutomatedDeclarationsStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedDeclarationsStrategy : IAutomatedStrategy
    {
        private readonly IEnumerable<IDeclaration<SyntaxNode>> declarations;
        private readonly IDeclaration<SyntaxNode> pattern;
        private readonly IDeclarationResolver resolver;
        private readonly string targetName;
        private readonly string currentNameSpace;
        private readonly IEnumerable<IReplacePatternHandlerFactory> replacePatternHandlerFactories;
        private readonly IEnumerable<string> ignoreUsingList;
        private readonly ISelectorResolver selectorResolver;
        private readonly IReplacePatternResolver replacePatternResolver;

        public AutomatedDeclarationsStrategy(
            IDeclaration<SyntaxNode> pattern,
            string nameSpace,
            string name,
            IEnumerable<IDeclaration<SyntaxNode>> declarations,
            IDeclarationResolver resolver,
            IEnumerable<IReplacePatternHandlerFactory> replacePatternHandlerFactories,
            IEnumerable<string> ignoreUsingList,
            ISelectorResolver selectorResolver,
            IReplacePatternResolver replacePatternResolver)
        {
            this.targetName = name;
            this.currentNameSpace = nameSpace;
            this.declarations = declarations;
            this.pattern = pattern;
            this.resolver = resolver;
            this.replacePatternHandlerFactories = replacePatternHandlerFactories;
            this.ignoreUsingList = ignoreUsingList;
            this.selectorResolver = selectorResolver;
            this.replacePatternResolver = replacePatternResolver;
        }

        public IReplacePatternHandler CreateReplacePatternHandler()
        {
            return new StrategyReplacePatternHandler(ApplyPatternReplace);
        }

        public IReplacePatternHandler CreateReplacePatternHandler(AttributeSyntax replacePatternAttributeSyntax)
        {
            throw new NotImplementedException();
        }

        private string ApplyPatternReplace(string text)
        {
            var result = text.Replace(this.pattern.Name, this.targetName);
            return result;
        }

        public string GetCurrentNameSpace()
        {
            return this.currentNameSpace;
        }

        public void RepeatDeclaration(
            AttributeSyntax repeatAttributeSyntax,
            Action<IAutomatedStrategy> callback)
        {
            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>(this.resolver, null);
            var patternName = constEvaluator.Visit(repeatAttributeSyntax.ArgumentList.Arguments.First().Expression);

            var repeatPattern = this.resolver.Resolve(patternName, this.pattern);

            foreach (var declaration in this.declarations)
            {
                callback(
                    new AutomatedGenericStrategy(
                        (IGenericDeclaration<SyntaxNode>)repeatPattern,
                        (IGenericDeclaration<SyntaxNode>)declaration,
                        this.resolver,
                        this.replacePatternHandlerFactories,
                        this.ignoreUsingList,
                        this.selectorResolver,
                        this.replacePatternResolver));
            }
        }

        public void RepeatNameSpace(Action<string> nsCallback)
        {
            var nsSet = new HashSet<string>(this.declarations.Select(d => d.DeclarationNameSpace));

            foreach (var ns in nsSet)
            {
                nsCallback(ns);
            }
        }

        public bool IgnoreUsingDirective(string ns)
        {
            return this.ignoreUsingList.Contains(ns);
        }

        public string ComputeTargetName()
        {
            return this.targetName;
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
}
