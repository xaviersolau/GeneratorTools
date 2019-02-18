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
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Utils;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// CSharpProject implementation.
    /// </summary>
    public class CSharpProject : ICSharpProject
    {
        private const string CompileList = "CompileList";
        private const string ProjectReferenceList = "ProjectReferenceList";

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
                throw new FileNotFoundException();
            }

            this.ProjectFile = Path.GetFileName(file);
            this.ProjectPath = Path.GetDirectoryName(file);
        }

        /// <inheritdoc/>
        public string ProjectFile { get; }

        /// <inheritdoc/>
        public string ProjectPath { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpProject> ProjectReferences { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICSharpFile> Files { get; private set; }

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

            // Get the project references.
            var projectReferenceList = this.DeployAndRunTarget(ProjectReferenceList);

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

        private string DeployAndRunTarget(string target)
        {
            // Generate and inject the target.
            this.DeployTarget(target);

            // Not we can call the target and get the output. (dotnet msbuild -t:target -nologo)
            return this.RunTarget(target);
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
            var outputFileName = Path.Combine(this.ProjectPath, "obj", $"{projectName}.{target}.targets");

            using (var targetFile = currentAssembly.GetManifestResourceStream(resName))
            using (var outputFile = System.IO.File.OpenWrite(outputFileName))
            {
                targetFile.CopyTo(outputFile);
            }
        }
    }
}
