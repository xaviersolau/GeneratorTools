// ----------------------------------------------------------------------
// <copyright file="ImplementationGenerator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Generator.Walker;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator;

namespace SoloX.GeneratorTools.Core.CSharp.Generator
{
    /// <summary>
    /// Entity implementation generator example.
    /// </summary>
    public class ImplementationGenerator
    {
        private readonly IGenerator fileGenerator;
        private readonly IInterfaceDeclaration itfPattern;
        private readonly IGenericDeclaration implPattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementationGenerator"/> class.
        /// </summary>
        /// <param name="itfPattern">The interface pattern to use for the generator.</param>
        /// <param name="implPattern">The implementation pattern to use for the generator.</param>
        /// <param name="generationRootFolder">The root folder where to generate the implementation entities.</param>
        public ImplementationGenerator(IInterfaceDeclaration itfPattern, IGenericDeclaration implPattern, string generationRootFolder)
        {
            this.fileGenerator = new FileGenerator(generationRootFolder);
            this.itfPattern = itfPattern;
            this.implPattern = implPattern;
        }

        /// <summary>
        /// Generate the implementation of the given entity.
        /// </summary>
        /// <param name="declaration">The entity interface.</param>
        public void Generate(IInterfaceDeclaration declaration)
        {
            var entityName = GetEntityName(declaration.Name);
            this.fileGenerator.Generate(@"Model/Impl", entityName, writer =>
            {
                var generatorWalker = new ImplementationGeneratorWalker(writer, this.itfPattern, this.implPattern, declaration, entityName);
                generatorWalker.Visit(this.implPattern.SyntaxNode.SyntaxTree.GetRoot());
            });
        }

        private static string GetEntityName(string name)
        {
            if (name.Length > 1 && name[0] == 'I' && char.IsUpper(name[1]))
            {
                return name.Substring(1);
            }
            else
            {
                return $"{name}Entity";
            }
        }
    }
}
