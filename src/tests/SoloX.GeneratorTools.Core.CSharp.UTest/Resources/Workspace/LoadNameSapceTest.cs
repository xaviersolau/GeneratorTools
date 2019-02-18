// ----------------------------------------------------------------------
// <copyright file="LoadNameSapceTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1201 // Elements should appear in the correct order

using System;
using System.Collections.Generic;

public class LoadTest2NoNamespace
{
}

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Workspace
{
    using System.Text;

    namespace Test
    {
        using System.IO;

        public class LoadTest2Test
        {
        }
    }

    public class LoadTest2
    {
    }
}

#pragma warning restore SA1201 // Elements should appear in the correct order
#pragma warning restore SA1402 // File may only contain a single type
#pragma warning restore SA1649 // File name should match first type name
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1200 // Using directives should be placed correctly
