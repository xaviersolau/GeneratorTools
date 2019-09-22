// ----------------------------------------------------------------------
// <copyright file="CSharpFactory.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// ICSharpFactory implementation.
    /// </summary>
    public class CSharpFactory : ICSharpFactory
    {
        private ILoggerFactory loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpFactory"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger provider.</param>
        public CSharpFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        public ICSharpAssembly CreateAssembly(Assembly assembly)
        {
            return new CSharpAssembly(this.loggerFactory.CreateLogger<CSharpAssembly>(), assembly);
        }

        /// <inheritdoc/>
        public ICSharpFile CreateFile(string file)
        {
            return new CSharpFile(file);
        }

        /// <inheritdoc/>
        public ICSharpProject CreateProject(string file)
        {
            return new CSharpProject(file);
        }
    }
}
