// ----------------------------------------------------------------------
// <copyright file="CompilationAssemblyResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    internal class CompilationAssemblyResolver : MetadataAssemblyResolver
    {
        private readonly Compilation compilation;

        public CompilationAssemblyResolver(Compilation compilation)
        {
            this.compilation = compilation;
        }

        public override Assembly Resolve(MetadataLoadContext context, AssemblyName assemblyName)
        {
            var matchedAssembly = context.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName.Name);

            if (matchedAssembly != null)
            {
                return matchedAssembly;
            }

            var assemblyRefName = this.compilation.ReferencedAssemblyNames.FirstOrDefault(x => x.Name == assemblyName.Name);

            var assemblyRef = this.compilation.References.FirstOrDefault(x => x.Display.EndsWith(assemblyName.Name + ".dll", System.StringComparison.OrdinalIgnoreCase));

            var path = assemblyRef.Display;
            if (File.Exists(path))
            {
                return context.LoadFromAssemblyPath(path);
            }

            throw new FileLoadException($"Could not resolve {assemblyName}");
        }
    }
}
