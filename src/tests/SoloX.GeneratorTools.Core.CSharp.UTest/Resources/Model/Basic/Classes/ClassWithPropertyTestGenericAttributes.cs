// ----------------------------------------------------------------------
// <copyright file="ClassWithPropertyTestGenericAttributes.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes
{
    public class ClassWithPropertyTestGenericAttributes
    {
        [Test3<int>]
        public int Property1WithGenAttribute { get; set; }

        [SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes.Test3<int>]
        public int Property2WithGenAttribute { get; set; }
    }
}
