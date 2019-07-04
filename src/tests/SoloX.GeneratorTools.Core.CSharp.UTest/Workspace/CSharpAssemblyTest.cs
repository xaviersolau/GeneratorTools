// ----------------------------------------------------------------------
// <copyright file="CSharpAssemblyTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Reflection;
using SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Workspace
{
    public class CSharpAssemblyTest
    {
        [Fact]
        public void LoadCSharpAssemblyTest()
        {
            var assembly = typeof(CSharpAssemblyTest).Assembly;
            var csAssembly = new CSharpAssembly(assembly);

            csAssembly.Load();

            Assert.Same(assembly, csAssembly.Assembly);
            var decl = Assert.Single(csAssembly.Declarations.Where(d => d.Name == nameof(BasicInterface)));

            var typeItfDecl = Assert.IsType<TypeInterfaceDeclaration>(decl);

            Assert.Same(typeof(BasicInterface), typeItfDecl.DeclarationType);
        }
    }
}
