// ----------------------------------------------------------------------
// <copyright file="ReflectionEnumDeclarationLoader.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.Utils;
using System;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    internal class ReflectionEnumDeclarationLoader : AEnumDeclarationLoader
    {
        private readonly IGeneratorLogger<ReflectionEnumDeclarationLoader> logger;

        public ReflectionEnumDeclarationLoader(IGeneratorLogger<ReflectionEnumDeclarationLoader> logger)
        {
            this.logger = logger;
        }

        internal override void Load(EnumDeclaration declaration, IDeclarationResolver resolver)
        {
            var declarationType = declaration.GetData<Type>();

            if (declarationType != typeof(int))
            {
                var use = ReflectionGenericDeclarationLoader<SyntaxNode>.GetDeclarationUseFrom(declarationType, resolver, null);

                declaration.UnderlyingType = use;
            }
        }

        /// <summary>
        /// Setup the given declaration to be loaded by reflection from the given type.
        /// </summary>
        /// <param name="decl">The declaration that will be loaded.</param>
        /// <param name="type">The type to load the declaration from.</param>
        internal static void Setup(EnumDeclaration decl, Type type)
        {
            decl.SetData(type);
        }
    }
}
