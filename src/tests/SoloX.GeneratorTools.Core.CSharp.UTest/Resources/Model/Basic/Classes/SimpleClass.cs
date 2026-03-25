// ----------------------------------------------------------------------
// <copyright file="SimpleClass.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes
{
    public class SimpleClass
    {
        private readonly int parameter;

        public SimpleClass(int parameter)
        {
            this.parameter = parameter;
        }
        public int SomeValue()
        {
            return this.parameter;
        }
    }
}
