// ----------------------------------------------------------------------
// <copyright file="CSharpProject.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SoloX.GeneratorTools.Core.CSharp.Utils;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// CSharpProject implementation.
    /// </summary>
    public class CSharpProject : ICSharpProject
    {
        private const string CompileList = "CompileList";
        private const string ProjectReferenceList = "ProjectReferenceList";
        private const string ProjectRootNameSpace = "ProjectRootNameSpace";
        private const string PackageReferenceList = "PackageReferenceList";
        private const string ProjectAssetsFilePath = "ProjectAssetsFilePath";

        private const string DotNet = "dotnet";

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
        public IReadOnlyCollection<ICSharpAssembly> Assemblies { get; private set; }

        /// <summary>
        /// Load the project.
        /// </summary>
        /// <param name="workspace">The workspace within the project is loaded.</param>
        public void Load(ICSharpWorkspace workspace)
        {
            if (this.isLoaded)
            {
                return;
            }

            this.isLoaded = true;

            this.DotNetRestore();

            // Get the project root name space
            this.RootNameSpace = this.DeployAndRunTarget(ProjectRootNameSpace).Trim();

            // Get the project references.
            var projectReferenceList = this.DeployAndRunTarget(ProjectReferenceList);

            // Get the project references.
            var packageReferenceList = this.DeployAndRunTarget(PackageReferenceList);

            var projectAssetsFilePath = this.DeployAndRunTarget(ProjectAssetsFilePath).Trim();

            var projectAssets = JsonConvert.DeserializeObject<ProjectAssets>(File.ReadAllText(projectAssetsFilePath));

            var assemblies = new List<ICSharpAssembly>();
            foreach (var compileItem in projectAssets.Targets.First().Value.AllPackageCompileItems)
            {
                var assemblyFile = GetAssemblyFile(projectAssets, compileItem);

                assemblies.Add(workspace.RegisterAssembly(assemblyFile));
            }

            this.Assemblies = assemblies;

            this.ProjectReferences = (
                from project in projectReferenceList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                select workspace.RegisterProject(PathHelper.ResolveRelativePath(this.ProjectPath, project.Trim())))
                .ToArray();

            // Make sure the project references are loaded.
            foreach (var projectItem in this.ProjectReferences)
            {
                ((CSharpProject)projectItem).Load(workspace);
            }

            // Once project references are loaded, we can get the files to compile.
            var fileList = this.DeployAndRunTarget(CompileList);

            this.Files = (
                from file in fileList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                select workspace.RegisterFile(PathHelper.ResolveRelativePath(this.ProjectPath, file.Trim())))
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

        private string DeployAndRunTarget(string target)
        {
            // Generate and inject the target.
            this.DeployTarget(target);

            // Not we can call the target and get the output. (dotnet msbuild -t:target -nologo)
            return this.RunTarget(target);
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

        private string RunTarget(string target)
        {
            var processStartInfo = new ProcessStartInfo(DotNet, $"msbuild -t:{target} -nologo")
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
                    throw new FormatException($"Unable to load project file: dotnet exit code is {process.ExitCode} ({rawError})");
                }

                return process.StandardOutput.ReadToEnd().TrimEnd('\n', '\r');
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

            if (!File.Exists(outputFileName))
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
