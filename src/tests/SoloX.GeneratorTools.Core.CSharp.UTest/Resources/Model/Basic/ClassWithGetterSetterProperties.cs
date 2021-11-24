// ----------------------------------------------------------------------
// <copyright file="ClassWithGetterSetterProperties.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Resources.Model.Basic
{
    public class ClassWithGetterSetterProperties
    {
        private int writeOnlyProperty;

        public int ReadWriteProperty { get; set; }

        public int ReadOnlyProperty { get; }

#pragma warning disable CA1044 // Les propriétés ne doivent pas être en écriture seule
        public int WriteOnlyProperty
        {
            set { this.writeOnlyProperty = value; }
        }
#pragma warning restore CA1044 // Les propriétés ne doivent pas être en écriture seule
    }
}
