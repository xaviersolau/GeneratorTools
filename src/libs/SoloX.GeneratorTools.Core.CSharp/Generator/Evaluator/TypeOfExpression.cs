// ----------------------------------------------------------------------
// <copyright file="TypeOfExpression.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeOfExpression
    {
        /// <summary>
        /// Setup instance.
        /// </summary>
        /// <param name="typeExpression">Type expression text.</param>
        public TypeOfExpression(string typeExpression)
        {
            TypeExpression = typeExpression;
        }

        /// <summary>
        /// Type expression text.
        /// </summary>
        public string TypeExpression { get; }
    }
}
