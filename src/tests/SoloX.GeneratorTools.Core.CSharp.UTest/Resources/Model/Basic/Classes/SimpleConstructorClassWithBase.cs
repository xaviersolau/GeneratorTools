// ----------------------------------------------------------------------
// <copyright file="SimpleConstructorClassWithBase.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes
{
    public class SimpleConstructorClassWithBase(int parameter) : SimpleConstructorClass(parameter)
    {
    }
}
