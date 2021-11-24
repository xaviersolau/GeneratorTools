// ----------------------------------------------------------------------
// <copyright file="IImplementationGenerator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.Generator.Writer;

namespace SoloX.GeneratorTools.Core.CSharp.Generator
{
    /// <summary>
    /// Implementation generator Interface.
    /// </summary>
    public interface IImplementationGenerator
    {
        /// <summary>
        /// Generate the implementation of the given interface declaration.
        /// </summary>
        /// <param name="writerSelector">The writer selector to use to actually write the code.</param>
        /// <param name="itfDeclaration">The interface declaration to implement.</param>
        /// <param name="implName">The class implementation name.</param>
        /// <returns>The generated tuple class name space / name.</returns>
        (string nameSpace, string name) Generate(IWriterSelector writerSelector, IInterfaceDeclaration itfDeclaration, string implName);
    }
}
