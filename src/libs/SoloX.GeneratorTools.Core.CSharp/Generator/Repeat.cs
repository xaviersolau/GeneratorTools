// ----------------------------------------------------------------------
// <copyright file="Repeat.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.GeneratorTools.Core.CSharp.Generator
{
    /// <summary>
    /// Repeat class to provide a way to specify how statement code must be generated.
    /// </summary>
    public static class Repeat
    {
        /// <summary>
        /// Repeat statements on the pattern.
        /// </summary>
        /// <param name="patternName"></param>
        /// <param name="statements"></param>
        public static void Statements(string patternName, Action statements)
        {
            if (statements == null)
            {
                throw new ArgumentNullException(nameof(statements));
            }

            statements.Invoke();
        }

        /// <summary>
        /// Repeat var affectation on the pattern.
        /// </summary>
        /// <param name="patternName"></param>
        /// <param name="initExpression"></param>
        public static T Affectation<T>(string patternName, T initExpression)
        {
            return initExpression;
        }

        /// <summary>
        /// Repeat argument on the pattern.
        /// </summary>
        /// <param name="patternName"></param>
        /// <param name="argExpression"></param>
        public static T Argument<T>(string patternName, T argExpression)
        {
            return argExpression;
        }
    }
}
