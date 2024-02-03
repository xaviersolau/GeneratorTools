// ----------------------------------------------------------------------
// <copyright file="IAsyncMethodPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Threading.Tasks;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf
{
    public interface IAsyncMethodPattern
    {
        Task<object> PatternMethodAsync(object someArgument);
    }
}
