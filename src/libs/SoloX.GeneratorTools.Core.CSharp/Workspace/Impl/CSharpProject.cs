// ----------------------------------------------------------------------
// <copyright file="CSharpProject.cs" company="Xavier Solau">
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
using System.Text.Json;
using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// CSharpProject implementation.
    /// </summary>
    public class CSharpProject : ICSharpProject, ICSharpWorkspaceItemLoader<ICSharpProject>
    {
        internal const string DotNet = "dotnet";

        private const string ProjectData = "ProjectData";

        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpProject"/> class.
        /// </summary>
        /// <param name="file">The CSharp file.</param>
        public CSharpProject(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException(file);
            }

            this.ProjectFile = Path.GetFileName(file);
            this.ProjectPath = Path.GetDirectoryName(file);
        }

        /// <inheritdoc/>
        public string ProjectFile { get; }

        /// <inheritdoc/>
        public string ProjectPath { get; }

        /// <inheritdoc/>
        public string RootNameSpace { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpProject> ProjectReferences { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpFile> Files { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpMetadataAssembly> Assemblies { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclaration<SyntaxNode>> Declarations => this.Files.SelectMany(f => f.Declarations).ToArray();

        /// <inheritdoc/>
        public ICSharpProject WorkspaceItem => this;

        /// <inheritdoc/>
        public void Load(ICSharpWorkspace workspace)
        {
            if (workspace == null)
            {
                throw new ArgumentNullException(nameof(workspace), $"The argument {nameof(workspace)} was null.");
            }

            if (this.isLoaded)
            {
                return;
            }

            this.isLoaded = true;

            this.DotNetRestore();

            // Get the project data.
            var projectDataStr = this.DeployAndRunTarget(ProjectData, null);
            projectDataStr = projectDataStr.Replace(@"\", @"/");

            var projectData = JsonSerializer.Deserialize<CSharpProjectData>(projectDataStr);

            if (string.IsNullOrEmpty(projectData.TargetFramework) && !string.IsNullOrEmpty(projectData.TargetFrameworks))
            {
                projectDataStr = this.DeployAndRunTarget(ProjectData, new Dictionary<string, string>()
                {
                    ["TargetFramework"] = projectData.TargetFrameworks.Split(';').First(),
                });

                projectDataStr = projectDataStr.Replace(@"\", @"/");

                projectData = JsonSerializer.Deserialize<CSharpProjectData>(projectDataStr);
            }

            this.RootNameSpace = projectData.RootNamespace;

            var projectAssetsFilePath = projectData.ProjectAssetsFile;

            var projectAssets = JsonSerializer.Deserialize<ProjectAssets>(File.ReadAllText(projectAssetsFilePath));

            var assemblies = new List<ICSharpMetadataAssembly>();
            var compileItems = projectAssets.Targets.First().Value.GetAllPackageCompileItems(projectAssets);
            foreach (var compileItem in compileItems)
            {
                var assemblyFile = GetAssemblyFile(projectAssets, compileItem);

                var registeredAssembly = workspace.RegisterMetadataAssembly(assemblyFile);
                if (registeredAssembly != null)
                {
                    assemblies.Add(registeredAssembly);
                }
            }

            this.Assemblies = assemblies;

            // Get the project references.
            this.ProjectReferences = (
               from project in projectData.ProjectReferences
               select workspace.RegisterProject(PathHelper.ResolveRelativePath(this.ProjectPath, project)))
               .ToArray();

            // Make sure the project references are loaded.
            foreach (var projectItem in this.ProjectReferences)
            {
                ((CSharpProject)projectItem).Load(workspace);
            }

            // Once project references are loaded, we can get the files to compile.
            this.Files = (
                from file in projectData.CompileList
                select workspace.RegisterFile(PathHelper.ResolveRelativePath(this.ProjectPath, file)))
                .ToArray();
        }

        private static string GetAssemblyFile(ProjectAssets projectAssets, string assemblyFile)
        {
            foreach (var packageFolder in projectAssets.PackageFolder)
            {
                var file = Path.Combine(packageFolder, assemblyFile);
                if (File.Exists(file))
                {
                    return file;
                }
            }

            throw new FileNotFoundException(
                $"Unable to find a file ({assemblyFile}) from [{string.Join(";", projectAssets.PackageFolder)}]",
                assemblyFile);
        }

        private string DeployAndRunTarget(string target, Dictionary<string, string> properties)
        {
            // Generate and inject the target.
            this.DeployTarget(target);

            // Not we can call the target and get the output. (dotnet msbuild -t:target -nologo)
            return this.RunTarget(target, properties);
        }

        private void DotNetRestore()
        {
            var processStartInfo = new ProcessStartInfo(DotNet, $"restore")
            {
                WorkingDirectory = this.ProjectPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    var rawError = process.StandardError.ReadToEnd();
                    throw new FormatException($"Unable to restore project: dotnet exit code is {process.ExitCode} ({rawError})");
                }
            }
        }

        private string RunTarget(string target, Dictionary<string, string> properties)
        {
            var propertiesArgs = string.Empty;

            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    propertiesArgs += $" -p:{property.Key}={property.Value}";
                }
            }

            var processStartInfo = new ProcessStartInfo(DotNet, $"msbuild -t:{target} {propertiesArgs} -nologo")
            {
                WorkingDirectory = this.ProjectPath,
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
                        $"Unable to load project file: dotnet exit code is {process.ExitCode}\n" +
                        $"Standard output:\n" +
                        $"{stdOutput})\n" +
                        $"Standard error:\n" +
                        $"{rawError})");
                }

                return stdOutput;
            }
        }

        private void DeployTarget(string target)
        {
            var projectName = this.ProjectFile;

            var currentAssembly = typeof(CSharpProject).Assembly;
            var resName = $"{currentAssembly.GetName().Name}.Resources.Workspace.{target}.targets";
            var outputFolder = Path.Combine(this.ProjectPath, "obj");
            var outputFileName = Path.Combine(outputFolder, $"{projectName}.{target}.targets");

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            var copy = true;
            if (File.Exists(outputFileName))
            {
                // check if modified.
                using (var targetFile = currentAssembly.GetManifestResourceStream(resName))
                using (var targetReader = new StreamReader(targetFile))
                using (var outputFile = File.OpenRead(outputFileName))
                using (var outputReader = new StreamReader(outputFile))
                {
                    var targetTxt = targetReader.ReadToEnd();
                    var outputTxt = outputReader.ReadToEnd();
                    copy = targetTxt != outputTxt;
                }
            }

            if (copy)
            {
                using (var targetFile = currentAssembly.GetManifestResourceStream(resName))
                using (var outputFile = System.IO.File.OpenWrite(outputFileName))
                {
                    targetFile.CopyTo(outputFile);
                }
            }
        }
    }
}
