// ----------------------------------------------------------------------
// <copyright file="TaskValueTypeReplaceHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern
{
    /// <summary>
    /// Task Value type replace handler.
    /// </summary>
    public class TaskValueTypeReplaceHandler : IReplacePatternHandlerFactory, IReplacePatternHandler
    {
        private string patternName;
        private string declarationName;

        /// <inheritdoc/>
        public string ApplyOn(string patternText)
        {
            if (patternText == null)
            {
                throw new ArgumentNullException(nameof(patternText));
            }

            return patternText.Replace(this.patternName, this.declarationName);
        }

        /// <inheritdoc/>
        public IReplacePatternHandler Setup(IGenericDeclaration<SyntaxNode> pattern, IGenericDeclaration<SyntaxNode> declaration)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IReplacePatternHandler Setup(IMethodDeclaration pattern, IMethodDeclaration declaration)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (declaration == null)
            {
                throw new ArgumentNullException(nameof(declaration));
            }

            if (pattern.ReturnType is IGenericDeclarationUse patternUse
                && (patternUse.Declaration.FullName == typeof(Task).FullName || patternUse.Declaration.FullName == typeof(ValueTask).FullName
                    || patternUse.Declaration.Name == nameof(Task) || patternUse.Declaration.Name == nameof(ValueTask)))
            {
                var patternAsyncReturnType = patternUse.GenericParameters.First();

                this.patternName = patternAsyncReturnType.SyntaxNodeProvider.SyntaxNode.ToString();

                if (declaration.ReturnType is IGenericDeclarationUse declarationUse
                    && (declarationUse.Declaration.FullName == typeof(Task).FullName || declarationUse.Declaration.FullName == typeof(ValueTask).FullName
                    || declarationUse.Declaration.Name == nameof(Task) || declarationUse.Declaration.Name == nameof(ValueTask)))
                {
                    var declarationAsyncReturnType = declarationUse.GenericParameters.First();

                    this.declarationName = declarationAsyncReturnType.SyntaxNodeProvider.SyntaxNode.ToString();

                    return this;
                }

            }
            return null;
        }
    }
}
