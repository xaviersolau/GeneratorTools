// ----------------------------------------------------------------------
// <copyright file="IMyModel.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Examples.Core;

namespace SoloX.GeneratorTools.Core.CSharp.Examples.Sample.Model
{
    /// <summary>
    /// A model definition example with two properties MyFirstProperty and MySecondProperty.
    /// </summary>
    /// <remarks>Setting on of the two properties will set the IsDirty flag to true.</remarks>
    public interface IMyModel : IModelBase
    {
        /// <summary>
        /// Gets or sets MyFirstProperty that is the first property of the model example.
        /// </summary>
        /// <remarks>If the property is set, the IsDirty flag will be true.</remarks>
        string MyFirstProperty { get; set; }

        /// <summary>
        /// Gets or sets MySecondProperty that is the second property of the model example.
        /// </summary>
        double MySecondProperty { get; set; }
    }
}
