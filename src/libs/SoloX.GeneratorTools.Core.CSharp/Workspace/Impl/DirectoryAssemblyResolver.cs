// ----------------------------------------------------------------------
// <copyright file="DirectoryAssemblyResolver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using System.Linq;
using System.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    internal class DirectoryAssemblyResolver : MetadataAssemblyResolver
    {
        private readonly string directory;

        public DirectoryAssemblyResolver(string directory)
        {
            this.directory = directory;
        }

        public override Assembly Resolve(MetadataLoadContext context, AssemblyName assemblyName)
        {
            var matchedAssembly = context.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName.Name);

            if (matchedAssembly != null)
            {
                return matchedAssembly;
            }

            var path = Path.Combine(this.directory, assemblyName.Name + ".dll");
            if (File.Exists(path))
            {
                return context.LoadFromAssemblyPath(path);
            }

            throw new FileLoadException($"Could not resolve {assemblyName}");
        }
    }
}
