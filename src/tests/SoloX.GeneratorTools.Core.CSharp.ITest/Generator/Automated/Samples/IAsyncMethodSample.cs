// ----------------------------------------------------------------------
// <copyright file="IAsyncMethodSample.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Samples
{
    public interface IAsyncMethodSample
    {
        Task<int> Method();

        Task<char> Method1(string arg);

        Task<double> Method2(int arg1, string arg2);

        Task<IEnumerable<double>> Method3();
    }
}
