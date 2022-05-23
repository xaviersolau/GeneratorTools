// ----------------------------------------------------------------------
// <copyright file="CSharpWorkspace.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver.Impl;
using SoloX.GeneratorTools.Core.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// Implements ICSharpWorkspace.
    /// </summary>
    public class CSharpWorkspace : ICSharpWorkspace
    {
        private readonly ICSharpWorkspaceItemFactory factory;
        private readonly IGeneratorLogger<CSharpWorkspace> logger;

        private readonly Dictionary<string, ICSharpProject> projects = new Dictionary<string, ICSharpProject>();
        private readonly Dictionary<string, ICSharpFile> files = new Dictionary<string, ICSharpFile>();
        private readonly Dictionary<string, ICSharpAssembly> assemblies = new Dictionary<string, ICSharpAssembly>();
        private readonly Dictionary<string, ICSharpMetadataAssembly> metadataAssemblies = new Dictionary<string, ICSharpMetadataAssembly>();
        private readonly Dictionary<string, ICSharpSyntaxTree> syntaxTrees = new Dictionary<string, ICSharpSyntaxTree>();
        private readonly bool disableRuntimeProb;

        private string runTimePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpWorkspace"/> class.
        /// </summary>
        /// <param name="logger">The logger to log errors.</param>
        /// <param name="factory">The factory to create File and Project object.</param>
        /// <param name="disableRuntimeProb">Disable installed runtime assembly prob.</param>
        public CSharpWorkspace(IGeneratorLogger<CSharpWorkspace> logger, ICSharpWorkspaceItemFactory factory, bool disableRuntimeProb = false)
        {
            this.logger = logger;
            this.factory = factory;

            this.disableRuntimeProb = disableRuntimeProb;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpProject> Projects => this.projects.Values;

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpFile> Files => this.files.Values;

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpAssembly> Assemblies => this.assemblies.Values;

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpMetadataAssembly> MetadataAssemblies => this.metadataAssemblies.Values;

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpSyntaxTree> SyntaxTrees => this.syntaxTrees.Values;

        /// <inheritdoc/>
        public void RegisterCompilation(Compilation compilation)
        {
            if (compilation == null)
            {
                throw new ArgumentNullException(nameof(compilation));
            }

            foreach (var externalReference in compilation.References)
            {
                if (externalReference.Properties.Kind == MetadataImageKind.Assembly)
                {
                    var assemblyFile = externalReference.Display;

                    var assemblyFileName = Path.GetFileName(assemblyFile);

                    if (!this.metadataAssemblies.TryGetValue(assemblyFileName, out var csMetadataAssembly))
                    {
                        var csMetadataAssemblyLoader = this.CreateMetadataAssembly(assemblyFile);

                        this.metadataAssemblies.Add(assemblyFileName, csMetadataAssemblyLoader.WorkspaceItem);
                    }
                }
            }

            foreach (var csMetadataAssembly in this.metadataAssemblies.Values)
            {
                this.LoadMetadataAssembly(csMetadataAssembly);
            }

            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                var syntaxTreeName = syntaxTree.FilePath;

                if (!this.syntaxTrees.ContainsKey(syntaxTreeName))
                {
                    var csSyntaxTree = this.factory.CreateSyntaxTree(syntaxTree);
                    this.syntaxTrees.Add(syntaxTreeName, csSyntaxTree.WorkspaceItem);

                    csSyntaxTree.Load(this);
                }
            }
        }

        /// <inheritdoc/>
        public ICSharpFile RegisterFile(string file)
        {
            // Resolve the full path
            file = Path.GetFullPath(file);
            if (!this.files.TryGetValue(file, out var csFile))
            {
                var csFileLoader = this.factory.CreateFile(file);
                csFile = csFileLoader.WorkspaceItem;

                this.files.Add(file, csFile);

                csFileLoader.Load(this);
            }

            return csFile;
        }

        /// <inheritdoc/>
        public ICSharpProject RegisterProject(string projectFile)
        {
            // Resolve the full path
            projectFile = Path.GetFullPath(projectFile);
            if (!this.projects.TryGetValue(projectFile, out var csProject))
            {
                var csProjectLoader = this.factory.CreateProject(projectFile);
                csProject = csProjectLoader.WorkspaceItem;

                this.projects.Add(projectFile, csProject);

                csProjectLoader.Load(this);
            }

            return csProject;
        }

        /// <inheritdoc/>
        public ICSharpAssembly RegisterAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                return null;
            }

            var assemblyFileName = Path.GetFileName(assembly.FullName);

            if (!this.assemblies.TryGetValue(assemblyFileName, out var csAssembly))
            {
                var csAssemblyLoader = this.factory.CreateAssembly(assembly);
                csAssembly = csAssemblyLoader.WorkspaceItem;

                this.assemblies.Add(assemblyFileName, csAssembly);

                csAssemblyLoader.Load(this);
            }

            return csAssembly;
        }

        /// <inheritdoc/>
        public ICSharpMetadataAssembly RegisterMetadataAssembly(string assemblyFile)
        {
            if (assemblyFile == null)
            {
                return null;
            }

            var assemblyFileName = Path.GetFileName(assemblyFile);

            if (!this.metadataAssemblies.TryGetValue(assemblyFileName, out var csMetadataAssembly))
            {
                var csMetadataAssemblyLoader = this.CreateMetadataAssembly(assemblyFile);

                if (csMetadataAssemblyLoader != null)
                {
                    csMetadataAssembly = csMetadataAssemblyLoader.WorkspaceItem;

                    this.metadataAssemblies.Add(assemblyFileName, csMetadataAssembly);

                    this.LoadMetadataAssembly(csMetadataAssembly);
                }
            }

            return csMetadataAssembly;
        }

        /// <inheritdoc/>
        public IDeclarationResolver DeepLoad()
        {
            var declarations = this.Assemblies.SelectMany(a => a.Declarations)
                .Concat(this.MetadataAssemblies.SelectMany(f => f.Declarations))
                .Concat(this.Files.SelectMany(f => f.Declarations))
                .Concat(this.SyntaxTrees.SelectMany(s => s.Declarations));
            var resolver = new DeclarationResolver(declarations);

            resolver.Load();

            return resolver;
        }

        private static IReadOnlyDictionary<Version, string> GetDotnetRunTimes()
        {
            var processStartInfo = new ProcessStartInfo(CSharpProject.DotNet, $"--list-runtimes")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
                var stdOutput = process.StandardOutput.ReadToEnd();
                if (process.ExitCode != 0)
                {
                    var rawError = process.StandardError.ReadToEnd();
                    throw new FormatException(
                        $"Unable to get .Net Core RunTimes: dotnet exit code is {process.ExitCode}\n" +
                        $"Standard output:\n" +
                        $"{stdOutput})\n" +
                        $"Standard error:\n" +
                        $"{rawError})");
                }

                var lines = stdOutput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                var pathMap = lines
                    .Select(line => line.Split(' '))
                    .Where(words =>
                    {
                        var versionString = words[1];
                        return Version.TryParse(versionString, out _);
                    })
                    .Select(words =>
                    {
                        var versionString = words[1];
                        var version = new Version(versionString);

                        var path = string.Join(" ", words.Skip(2));
                        path = path.Substring(1, path.Length - 2);

                        // path = Path.Combine(path, versionString, "mscorlib.dll");
                        path = Path.Combine(path, versionString);
                        return new
                        {
                            Name = words.First(),
                            Version = version,
                            Path = path,
                        };
                    })
                    .Where(sdk => sdk.Name == "Microsoft.NETCore.App")
                    .ToDictionary(sdk => sdk.Version, sdk => sdk.Path);

                return pathMap;
            }
        }

        private string LoadRunTimeAssembly()
        {
            if (this.runTimePath == null)
            {
                var currentVersion = Environment.Version;

                var runtimes = GetDotnetRunTimes();
                this.runTimePath = runtimes[runtimes.Keys.Where(v => v <= currentVersion).Max()];
            }
            return this.runTimePath;
        }

        private ICSharpWorkspaceItemLoader<ICSharpMetadataAssembly> CreateMetadataAssembly(string assemblyFile)
        {
            if (!File.Exists(assemblyFile))
            {
                if (this.disableRuntimeProb)
                {
                    return null;
                }

                var runtimePath = LoadRunTimeAssembly();

                assemblyFile = Path.Combine(runtimePath, assemblyFile);

                if (!File.Exists(assemblyFile))
                {
                    throw new FileNotFoundException(assemblyFile);
                }
            }

            var csMetadataAssembly = this.factory.CreateMetadataAssembly(assemblyFile);

            return csMetadataAssembly;
        }

        private void LoadMetadataAssembly(ICSharpMetadataAssembly csMetadataAssembly)
        {
            ((ICSharpWorkspaceItemLoader<ICSharpMetadataAssembly>)csMetadataAssembly).Load(this);
        }
    }
}
