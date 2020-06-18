// ----------------------------------------------------------------------
// <copyright file="IAutomatedStrategy.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model;

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
        /// Apply the pattern substitution on the given text.
        /// </summary>
        /// <param name="text">Source text to match the pattern from.</param>
        /// <returns>The output text with the pattern replaced.</returns>
        string ApplyPatternReplace(string text);
    }
}
