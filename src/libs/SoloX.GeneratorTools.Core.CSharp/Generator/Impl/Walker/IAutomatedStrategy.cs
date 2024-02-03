// ----------------------------------------------------------------------
// <copyright file="IAutomatedStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Impl.Walker
{
    /// <summary>
    /// Automated generator walker strategy interface.
    /// </summary>
    public interface IAutomatedStrategy
    {
        /// <summary>
        /// Compute the target name.
        /// </summary>
        /// <returns>The computed target name.</returns>
        string ComputeTargetName();

        /// <summary>
        /// Gets the current name space.
        /// </summary>
        /// <returns>The current name space.</returns>
        string GetCurrentNameSpace();

        /// <summary>
        /// Process repeat NameSpace handling.
        /// </summary>
        /// <param name="nsCallback">NameSpace callback.</param>
        void RepeatNameSpace(Action<string> nsCallback);

        /// <summary>
        /// Tells if a name space must be ignored.
        /// </summary>
        /// <param name="ns">The name space to test.</param>
        /// <returns>True if the name space must be ignored.</returns>
        bool IgnoreUsingDirective(string ns);

        /// <summary>
        /// Process repeat declaration handling.
        /// </summary>
        /// <param name="repeatAttributeSyntax">The repeat attribute syntax.</param>
        /// <param name="callback">Declaration callback.</param>
        void RepeatDeclaration(
            AttributeSyntax repeatAttributeSyntax,
            Action<IAutomatedStrategy> callback);

        /// <summary>
        /// Create the IReplacePatternHandler from the current strategy.
        /// </summary>
        /// <param name="replacePatternAttributeSyntax">ReplacePatternAttribute node</param>
        /// <returns></returns>
        IReplacePatternHandler CreateReplacePatternHandler(AttributeSyntax replacePatternAttributeSyntax);

        /// <summary>
        /// Create the IReplacePatternHandler from the current strategy.
        /// </summary>
        /// <returns></returns>
        IReplacePatternHandler CreateReplacePatternHandler();

        /// <summary>
        /// Try to match the given pattern name expression and Repeat statement.
        /// </summary>
        /// <param name="patternNameExpression"></param>
        /// <param name="patternPrefixExpression"></param>
        /// <param name="patternSuffixExpression"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        bool TryMatchAndRepeatStatement(
            SyntaxNode? patternNameExpression,
            SyntaxNode? patternPrefixExpression,
            SyntaxNode? patternSuffixExpression,
            Action<IAutomatedStrategy> callback);
    }

    /// <summary>
    /// Strategy ReplacePatternHandler implementation.
    /// </summary>
    public class StrategyReplacePatternHandler : IReplacePatternHandler
    {
        private readonly Func<string, string> handler;

        /// <summary>
        /// Setup instance with the given handler.
        /// </summary>
        /// <param name="handler"></param>
        public StrategyReplacePatternHandler(Func<string, string> handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public string ApplyOn(string patternText)
        {
            return this.handler(patternText);
        }
    }
}
