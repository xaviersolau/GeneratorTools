// ----------------------------------------------------------------------
// <copyright file="GenericClassWithBase.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes
{
    public class GenericClassWithBase<T> : SimpleClass
    {
        public GenericClassWithBase(int parameter)
            : base(parameter)
        {
        }
    }
}
