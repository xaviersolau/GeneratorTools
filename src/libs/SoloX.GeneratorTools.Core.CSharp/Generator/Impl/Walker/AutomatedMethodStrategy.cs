﻿// ----------------------------------------------------------------------
// <copyright file="AutomatedMethodStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    internal class AutomatedMethodStrategy : IAutomatedStrategy
    {
        private readonly IMethodDeclaration pattern;
        private readonly IMethodDeclaration declaration;
        private readonly IEnumerable<IReplacePatternHandler> replacePatternHandlers;

        public AutomatedMethodStrategy(
            IMethodDeclaration pattern,
            IMethodDeclaration declaration,
            IEnumerable<IReplacePatternHandler> replacePatternHandlers)
        {
            this.pattern = pattern;
            this.declaration = declaration;
            this.replacePatternHandlers = replacePatternHandlers;
        }

        public bool IsPackStatementEnabled => false;

        public string ApplyPatternReplace(string text)
        {
            var result = text
                .Replace(this.pattern.Name, this.declaration.Name);

            foreach (var replacePatternHandler in this.replacePatternHandlers)
            {
                result = replacePatternHandler.ApplyOn(result);
            }

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
            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>();
            var patternName = constEvaluator.Visit(repeatAttributeSyntax.ArgumentList.Arguments.First().Expression);

            var patternParameter = this.pattern.Parameters.Single(x => x.Name == patternName);

            var patternPrefixExp = repeatAttributeSyntax.ArgumentList
                .Arguments.FirstOrDefault(a => a.NameEquals.Name.ToString() == nameof(RepeatAttribute.Prefix))?.Expression;
            var patternSuffixExp = repeatAttributeSyntax.ArgumentList
                .Arguments.FirstOrDefault(a => a.NameEquals.Name.ToString() == nameof(RepeatAttribute.Suffix))?.Expression;

            var patternPrefix = patternPrefixExp != null ? constEvaluator.Visit(patternPrefixExp) : null;
            var patternSuffix = patternSuffixExp != null ? constEvaluator.Visit(patternSuffixExp) : null;


            foreach (var parameter in this.declaration.Parameters)
            {
                var strategy = new AutomatedParameterStrategy(patternParameter, parameter, this.replacePatternHandlers, patternPrefix, patternSuffix);
                callback(strategy);
            }
        }

        public void RepeatNameSpace(Action<string> nsCallback)
        {
            throw new NotImplementedException();
        }

        public bool TryMatchRepeatDeclaration(AttributeSyntax repeatAttributeSyntax, string expression)
        {
            var constEvaluator = new ConstantExpressionSyntaxEvaluator<string>();
            var patternName = constEvaluator.Visit(repeatAttributeSyntax.ArgumentList.Arguments.First().Expression);

            // get the property from the current pattern generic definition.
            var repeatParameter = this.pattern.Parameters.First(p => p.Name == patternName);

            return AutomatedParameterStrategy.Match(repeatParameter, expression);
        }

        public void RepeatStatements(AttributeSyntax repeatStatementsAttributeSyntax, Action<IAutomatedStrategy> callback)
        {
            throw new NotImplementedException();
        }
    }
}
