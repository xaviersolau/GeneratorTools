// ----------------------------------------------------------------------
// <copyright file="ISimpleMethodSample.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Samples
{
    public interface ISimpleMethodSample
    {
        int Method();

        int Method1(string arg);

        double Method2(int arg1, double arg2);
    }
}
