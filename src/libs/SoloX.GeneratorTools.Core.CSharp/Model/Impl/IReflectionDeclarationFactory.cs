// ----------------------------------------------------------------------
// <copyright file="IReflectionDeclarationFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Declaration factory used to create declaration instances depending the reflection load.
    /// </summary>
    public interface IReflectionDeclarationFactory
    {
        /// <summary>
        /// Create an interface declaration from the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IDeclaration<SyntaxNode> CreateDeclaration(Type type);
    }
}
