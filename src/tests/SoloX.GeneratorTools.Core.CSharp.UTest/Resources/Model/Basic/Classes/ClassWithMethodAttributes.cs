// ----------------------------------------------------------------------
// <copyright file="ClassWithMethodAttributes.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.ComponentModel;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes
{
    public class ClassWithMethodAttributes
    {
        [global::System.ComponentModel.Description("Some description")]
#pragma warning disable CA1822 // Mark members as static
        public int MethodWithAttribute1([Description("Some arg description")] int arg)
        {
            return 0;
        }
#pragma warning restore CA1822 // Mark members as static

        [return: Description("Some return description")]
        [Description("Some description")]
#pragma warning disable CA1822 // Mark members as static
        public int MethodWithAttribute2([Description("Some arg description")] int arg)
        {
            return 0;
        }
#pragma warning restore CA1822 // Mark members as static

#pragma warning disable CA1822 // Mark members as static
        public int MethodWithAttribute3(int arg1, [Description("Some arg2 description")] int arg2)
        {
            return 0;
        }
#pragma warning restore CA1822 // Mark members as static
    }
}
