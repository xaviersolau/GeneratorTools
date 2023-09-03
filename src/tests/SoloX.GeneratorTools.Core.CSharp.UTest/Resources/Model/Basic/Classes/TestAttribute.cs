// ----------------------------------------------------------------------
// <copyright file="TestAttribute.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic.Classes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TestAttribute : Attribute
    {
        public TestAttribute(Type typeValue)
        {
            TypeValue = typeValue;
        }

        public Type TypeValue { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Test2Attribute : Attribute
    {
        public Type TypeValue { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Test3Attribute<T> : Attribute
    {
    }
}
