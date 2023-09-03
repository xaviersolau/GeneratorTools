// ----------------------------------------------------------------------
// <copyright file="ParserException.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.Exceptions
{
    /// <summary>
    /// Exception thrown when Parser failed.
    /// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
    public class ParserException : Exception
    {
        /// <summary>
        /// Setup instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="syntaxNode"></param>
        public ParserException(string message, SyntaxNode syntaxNode) :
            this(message, syntaxNode, null)
        {
        }

        /// <summary>
        /// Setup instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="syntaxNode"></param>
        /// <param name="innerException"></param>
        public ParserException(string message, SyntaxNode syntaxNode, Exception innerException)
            : base(syntaxNode == null ? message : $"{message}{Environment.NewLine}{syntaxNode.ToFullString()}", innerException)
        {
            SyntaxNode = syntaxNode;
        }

        /// <summary>
        /// Syntax Node where the exception was thrown.
        /// </summary>
        public SyntaxNode SyntaxNode { get; }
    }
#pragma warning restore CA1032 // Implement standard exception constructors
}
