// ----------------------------------------------------------------------
// <copyright file="ClassLoadingTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using SoloX.GeneratorTools.Core.CSharp.Model;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Model
{
    public class ClassLoadingTest
    {
        [Theory]
        [InlineData("./Resources/Model/SimpleClass.cs", null, null)]
        [InlineData("./Resources/Model/SimpleClassWithBase.cs", null, "SimpleClass")]
        [InlineData("./Resources/Model/GenericClass.cs", "T", null)]
        public void LoadCSharpClassTest(string file, string typeParameterName, string baseClassName)
        {
            var csFile = new CSharpFile(file);
            csFile.Load();

            Assert.Single(csFile.Declarations);
            var declaration = csFile.Declarations.Single();

            var classDecl = Assert.IsType<ClassDeclaration>(declaration);

            classDecl.Load(Mock.Of<IDeclarationResolver>());

            Assert.NotNull(classDecl.GenericParameters);
            Assert.NotNull(classDecl.Extends);

            if (string.IsNullOrEmpty(typeParameterName))
            {
                Assert.Empty(classDecl.GenericParameters);
            }
            else
            {
                var paramDecl = Assert.Single(classDecl.GenericParameters);
                Assert.Equal(typeParameterName, paramDecl.Name);
            }

            if (string.IsNullOrEmpty(baseClassName))
            {
                Assert.Empty(classDecl.Extends);
            }
            else
            {
                var baseClass = Assert.Single(classDecl.Extends);
                Assert.Equal(baseClassName, baseClass.Declaration.Name);
            }
        }
    }
}
