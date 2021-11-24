// ----------------------------------------------------------------------
// <copyright file="LocatorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using SoloX.GeneratorTools.Core.Generator.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.UTest.Generator
{
    public class LocatorTest
    {
        [Theory]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "NameSpace.Base.A.B", "Folder/Root.Path/A/B", "NameSpace.Base.A.B")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "NameSpace.Base.A", "Folder/Root.Path/A", "NameSpace.Base.A")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "NameSpace.Base", "Folder/Root.Path", "NameSpace.Base")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Other.NameSpace.Base", "Folder/Root.Path", "NameSpace.Base")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "NameSpace.BaseOther", "Folder/Root.Path", "NameSpace.Base")]
        public void BaicTargetLocationTest(
            string baseFolder,
            string baseNameSpace,
            string inputNameSpace,
            string expectedLocation,
            string expectedNameSpace)
        {
            var locator = new RelativeLocator(baseFolder, baseNameSpace);
            var (targetLocation, targetNameSpace) = locator.ComputeTargetLocation(inputNameSpace);
            Assert.Equal(Normalize(targetLocation), Normalize(expectedLocation));
            Assert.Equal(targetNameSpace, expectedNameSpace);
        }

        [Theory]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Impl", "NameSpace.Base.A.B", "Folder/Root.Path/A/B/Impl", "NameSpace.Base.A.B.Impl")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Impl", "NameSpace.Base.A", "Folder/Root.Path/A/Impl", "NameSpace.Base.A.Impl")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Impl", "NameSpace.Base", "Folder/Root.Path/Impl", "NameSpace.Base.Impl")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Impl", "Other.NameSpace.Base", "Folder/Root.Path/Impl", "NameSpace.Base.Impl")]
        public void SuffixedTargetLocationTest(
            string baseFolder,
            string baseNameSpace,
            string suffix,
            string inputNameSpace,
            string expectedLocation,
            string expectedNameSpace)
        {
            var locator = new RelativeLocator(baseFolder, baseNameSpace, suffix);
            var (targetLocation, targetNameSpace) = locator.ComputeTargetLocation(inputNameSpace);
            Assert.Equal(Normalize(targetLocation), Normalize(expectedLocation));
            Assert.Equal(targetNameSpace, expectedNameSpace);
        }

        [Theory]
        [InlineData("Folder/Root.Path", "NameSpace.Base", null, "Fall.Back.NameSpace", "NameSpace.Base.A.B", "Folder/Root.Path/A/B", "NameSpace.Base.A.B")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", null, "Fall.Back.NameSpace", "NameSpace.Base.A", "Folder/Root.Path/A", "NameSpace.Base.A")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", null, "Fall.Back.NameSpace", "NameSpace.Base", "Folder/Root.Path", "NameSpace.Base")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", null, "Fall.Back.NameSpace", "Other.NameSpace.Base", "Folder/Root.Path/Fall/Back/NameSpace", "NameSpace.Base.Fall.Back.NameSpace")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Impl", "Fall.Back.NameSpace", "NameSpace.Base.A.B", "Folder/Root.Path/A/B/Impl", "NameSpace.Base.A.B.Impl")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Impl", "Fall.Back.NameSpace", "NameSpace.Base.A", "Folder/Root.Path/A/Impl", "NameSpace.Base.A.Impl")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Impl", "Fall.Back.NameSpace", "NameSpace.Base", "Folder/Root.Path/Impl", "NameSpace.Base.Impl")]
        [InlineData("Folder/Root.Path", "NameSpace.Base", "Impl", "Fall.Back.NameSpace", "Other.NameSpace.Base", "Folder/Root.Path/Fall/Back/NameSpace/Impl", "NameSpace.Base.Fall.Back.NameSpace.Impl")]
        public void FallBackTargetLocationTest(
            string baseFolder,
            string baseNameSpace,
            string suffix,
            string fallBack,
            string inputNameSpace,
            string expectedLocation,
            string expectedNameSpace)
        {
            var locator = new RelativeLocator(baseFolder, baseNameSpace, suffix, fallBack);
            var (targetLocation, targetNameSpace) = locator.ComputeTargetLocation(inputNameSpace);
            Assert.Equal(Normalize(targetLocation), Normalize(expectedLocation));
            Assert.Equal(targetNameSpace, expectedNameSpace);
        }

        private static string Normalize(string path)
        {
            return path
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
