// ----------------------------------------------------------------------
// <copyright file="LoggerHelper.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.CodeQuality.Test.Helpers.XUnit.V3.Logger;
using SoloX.GeneratorTools.Core.CSharp.Extensions.Utils;
using SoloX.GeneratorTools.Core.Utils;
using Xunit;

namespace SoloX.GeneratorTools.Core.Test.Helpers.XUnit
{
    public static class LoggerHelper
    {
        public static IGeneratorLogger<T> CreateGeneratorLogger<T>(ITestOutputHelper testOutputHelper)
        {
            return new GeneratorLogger<T>(new TestLogger<T>(testOutputHelper));
        }
    }
}
