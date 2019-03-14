// ----------------------------------------------------------------------
// <copyright file="EntityImplementationGenerator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator;

namespace SoloX.GeneratorTools.Core.CSharp.Examples
{
    /// <summary>
    /// Entity implementation generator example.
    /// </summary>
    public class EntityImplementationGenerator
    {
        private readonly IGenerator fileGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityImplementationGenerator"/> class.
        /// </summary>
        /// <param name="generationRootFolder">The root folder where to generate the implementation entities.</param>
        public EntityImplementationGenerator(string generationRootFolder)
        {
            this.fileGenerator = new FileGenerator(generationRootFolder);
        }

        /// <summary>
        /// Generate the implementation of the given entity.
        /// </summary>
        /// <param name="declaration">The entity interface.</param>
        public void Generate(IInterfaceDeclaration declaration)
        {
            var entityName = GetEntityName(declaration.Name);
            this.fileGenerator.Generate(@"Model", entityName, writer =>
            {
                writer.WriteLine($"// Hello {entityName}!");
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
