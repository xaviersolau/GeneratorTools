// ----------------------------------------------------------------------
// <copyright file="ClassWithConstants.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes
{
    public class ClassWithConstants
    {
        public const string ValueString = "Something";

        public const int ValueInt = 123;

#pragma warning disable CA1051 // Do not declare visible instance fields
        public string value2 = "";
#pragma warning restore CA1051 // Do not declare visible instance fields
    }
}
