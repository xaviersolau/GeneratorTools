// ----------------------------------------------------------------------
// <copyright file="ClassWithPropertyAttributes.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.ComponentModel;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes
{
    public class ClassWithPropertyAttributes
    {
        [Description("Some description")]
        public int PropertyWithAttribute { get; set; }

        public const string Test = "use of const";

        [Description($"Some description with {Test}")]
        public int PropertyWithAttributeComplexArg { get; set; }
    }
}
