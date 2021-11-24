﻿// ----------------------------------------------------------------------
// <copyright file="ClassWithMethods.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic
{
#pragma warning disable CA1822 // Marquer les membres comme étant static
#pragma warning disable CA1801
    public class ClassWithMethods
    {
        /// <summary>
        /// A basic method.
        /// </summary>
        public SimpleClass ThisIsABasicMethod()
        {
            // Nothing to do....
            return default;
        }

        /// <summary>
        /// A generic method with parameters.
        /// </summary>
        public int ThisIsAMethodWithParameters(int a, string b, object c)
        {
            // Nothing to do....
            return 0;
        }
    }
#pragma warning restore CA1801
#pragma warning restore CA1822 // Marquer les membres comme étant static
}
