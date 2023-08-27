// ----------------------------------------------------------------------
// <copyright file="IAutomatedStrategy.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        /// Try to match if the repeat declaration can apply on the given text expression.
        /// </summary>
        /// <param name="repeatAttributeSyntax">The repeat attribute syntax.</param>
        /// <param name="expression">The text expression to test.</param>
        /// <returns>True if there is a match.</returns>
        bool TryMatchRepeatDeclaration(AttributeSyntax repeatAttributeSyntax, string expression);

        /// <summary>
        /// Process repeat declaration handling.
        /// </summary>
        /// <param name="repeatAttributeSyntax">The repeat attribute syntax.</param>
        /// <param name="callback">Declaration callback.</param>
        void RepeatDeclaration(
            AttributeSyntax repeatAttributeSyntax,
            Action<IAutomatedStrategy> callback);

        /// <summary>
        /// Process repeat statements handling.
        /// </summary>
        /// <param name="repeatStatementsAttributeSyntax">The repeat statements attribute syntax.</param>
        /// <param name="parentStrategy">Parent strategy that can be specified.</param>
        /// <param name="callback">Declaration callback.</param>
        void RepeatStatements(
            AttributeSyntax repeatStatementsAttributeSyntax,
            IAutomatedStrategy parentStrategy,
            Action<IAutomatedStrategy> callback);

        /// <summary>
        /// Tells is PackStatement is enabled.
        /// </summary>
        bool IsPackStatementEnabled { get; }

        /// <summary>
        /// Apply the pattern substitution on the given text.
        /// </summary>
        /// <param name="text">Source text to match the pattern from.</param>
        /// <returns>The output text with the pattern replaced.</returns>
        string ApplyPatternReplace(string text);
    }
}
