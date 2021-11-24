// ----------------------------------------------------------------------
// <copyright file="NodeWriterHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;
using SoloX.GeneratorTools.Core.Generator.Writer;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Utils
{
    public static class NodeWriterHelper
    {
        /// <summary>
        /// Use the given writer on the given syntax node and assert that the resulting output syntax
        /// node will match the given type parameter.
        /// </summary>
        /// <typeparam name="T">The resulting output syntax node type.</typeparam>
        /// <param name="nodeWriter">The writer to use to generate the output.</param>
        /// <param name="implPatternNode">The implementation pattern used as input to the writer.</param>
        /// <returns>The resulting output syntax node.</returns>
        public static T WriteAndAssertSingleMemberOfType<T>(INodeWriter nodeWriter, SyntaxNode implPatternNode)
            where T : SyntaxNode
        {
            var output = new StringBuilder();
            nodeWriter.Write(implPatternNode, s => output.Append(s));

            return AssertSingleMemberOfType<T>(output);
        }

        /// <summary>
        /// Use the given writer on the given syntax node and assert that the resulting output syntax
        /// node will match a list of item of the given type parameter.
        /// </summary>
        /// <typeparam name="T">The resulting output syntax node type.</typeparam>
        /// <param name="nodeWriter">The writer to use to generate the output.</param>
        /// <param name="implPatternNode">The implementation pattern used as input to the writer.</param>
        /// <returns>The resulting output syntax nodes.</returns>
        public static IReadOnlyList<T> WriteAndAssertMultiMemberOfType<T>(INodeWriter nodeWriter, SyntaxNode implPatternNode)
            where T : SyntaxNode
        {
            var output = new StringBuilder();
            nodeWriter.Write(implPatternNode, s => output.Append(s));

            return AssertMultiMemberOfType<T>(output);
        }

        private static T AssertSingleMemberOfType<T>(StringBuilder output)
            where T : SyntaxNode
        {
            var resultingNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode(output.ToString());
            var cun = Assert.IsType<CompilationUnitSyntax>(resultingNode);
            var member = Assert.Single(cun.Members);
            return Assert.IsType<T>(member);
        }

        private static IReadOnlyList<T> AssertMultiMemberOfType<T>(StringBuilder output)
            where T : SyntaxNode
        {
            var resultingNode = AReflectionSyntaxNodeProvider<SyntaxNode>
                .GetSyntaxNode(output.ToString());
            var cun = Assert.IsType<CompilationUnitSyntax>(resultingNode);
            var results = new List<T>();
            foreach (var member in cun.Members)
            {
                results.Add(Assert.IsType<T>(member));
            }

            return results;
        }
    }
}
