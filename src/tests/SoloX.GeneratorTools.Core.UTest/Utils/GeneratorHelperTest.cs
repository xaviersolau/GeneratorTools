// ----------------------------------------------------------------------
// <copyright file="GeneratorHelperTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.UTest.Utils
{
    public class GeneratorHelperTest
    {
        [Theory]
        [InlineData("IClass", "Class")]
        [InlineData("Class", "ClassImpl")]
        [InlineData("ClassItf", "Class")]
        [InlineData("ClassInterface", "Class")]
        public void GeneratorHelperComputeClassNameTest(string interfaceName, string expectedClassName)
        {
            var className = GeneratorHelper.ComputeClassName(interfaceName);
            Assert.Equal(expectedClassName, className);
        }
    }
}
