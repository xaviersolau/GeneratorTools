﻿// ----------------------------------------------------------------------
// <copyright file="ClassWithGenericMethods.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic
{
#pragma warning disable CA1822 // Marquer les membres comme étant static
#pragma warning disable CA1801
    public class ClassWithGenericMethods
    {
        /// <summary>
        /// A basic generic method.
        /// </summary>
        public SimpleClass ThisIsABasicMethod<TData>()
        {
            // Nothing to do....
            return default;
        }

        /// <summary>
        /// A method with parameters.
        /// </summary>
        public int ThisIsAMethodWithParameters<TData>(int a, string b, TData c)
        {
            // Nothing to do....
            return 0;
        }
    }
#pragma warning restore CA1801
#pragma warning restore CA1822 // Marquer les membres comme étant static
}
