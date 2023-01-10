// ----------------------------------------------------------------------
// <copyright file="SimplePattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Implementation.Patterns.Itf;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Implementation.Patterns.Impl
{
    public class SimplePattern : ISimplePattern
    {
        private object patternProperty;

        public object PatternProperty
        {
            get { return this.patternProperty; }
            set { this.patternProperty = value; }
        }
    }
}
