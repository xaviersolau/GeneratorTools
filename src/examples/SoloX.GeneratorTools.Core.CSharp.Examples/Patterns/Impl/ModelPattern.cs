// ----------------------------------------------------------------------
// <copyright file="ModelPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Examples.Core;
using SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Itf;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Patterns.Impl
{
    /// <summary>
    /// Model pattern implementation.
    /// </summary>
    [Pattern(typeof(InterfaceBasedOnSelector<IModelBase>))]
    [Repeat(Pattern = nameof(IModelPattern), Prefix = "I")]
    public class ModelPattern : IModelPattern
    {
        [Repeat(Pattern = nameof(IModelPattern.PropertyPattern))]
        private object propertyPattern;

        /// <inheritdoc/>
        public bool IsDirty
        { get; private set; }

        /// <inheritdoc/>
        [Repeat(Pattern = nameof(IModelPattern.PropertyPattern))]
        public object PropertyPattern
        {
            get
            {
                return this.propertyPattern;
            }

            set
            {
                this.propertyPattern = value;
                this.IsDirty = true;
            }
        }
    }
}
