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
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl
{
    /// <summary>
    /// ICSharpFactory implementation.
    /// </summary>
    public class CSharpFactory : ICSharpFactory
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IDeclarationFactory declarationFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpFactory"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger provider.</param>
        /// <param name="declarationFactory">The declaration factory to use to create declaration instances.</param>
        public CSharpFactory(ILoggerFactory loggerFactory, IDeclarationFactory declarationFactory)
        {
            this.loggerFactory = loggerFactory;
            this.declarationFactory = declarationFactory;
        }

        /// <inheritdoc/>
        public ICSharpAssembly CreateAssembly(Assembly assembly)
        {
            return new CSharpAssembly(
                this.loggerFactory.CreateLogger<CSharpAssembly>(),
                this.declarationFactory,
                assembly);
        }

        /// <inheritdoc/>
        public ICSharpFile CreateFile(string file)
        {
            return new CSharpFile(file, this.declarationFactory);
        }

        /// <inheritdoc/>
        public ICSharpProject CreateProject(string file)
        {
            return new CSharpProject(file);
        }
    }
}
