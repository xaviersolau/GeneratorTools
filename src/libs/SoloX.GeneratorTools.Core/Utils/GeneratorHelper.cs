// ----------------------------------------------------------------------
// <copyright file="GeneratorHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.GeneratorTools.Core.Utils
{
    /// <summary>
    /// Generator helper used to compute class implementation name for example.
    /// </summary>
    public static class GeneratorHelper
    {
        /// <summary>
        /// Compute the implementation class name from the given interface name.
        /// for example if the interface name is like IInterfaceName, the implementation class name will be the
        /// same without the first 'I' character.
        /// In the case where the interface name is suffixed by 'Interface' or 'Itf', the suffix will just be removed.
        /// If the interface name is something else, it will be suffixed by 'Impl'.
        /// </summary>
        /// <param name="interfaceName">The interface name from witch we will compute the implementation name.</param>
        /// <returns>The implementation class name.</returns>
        public static string ComputeClassName(string interfaceName)
        {
            if (interfaceName == null)
            {
                throw new ArgumentNullException(nameof(interfaceName), $"The argument {nameof(interfaceName)} was null.");
            }

            var len = interfaceName.Length;
            if (len > 1 && interfaceName[0] == 'I' && char.IsUpper(interfaceName[1]))
            {
                return interfaceName.Substring(1);
            }
            else if (len > 3 && interfaceName.EndsWith("Itf", StringComparison.InvariantCulture))
            {
                return interfaceName.Substring(0, len - 3);
            }
            else if (len > 9 && interfaceName.EndsWith("Interface", StringComparison.InvariantCulture))
            {
                return interfaceName.Substring(0, len - 9);
            }
            else
            {
                return $"{interfaceName}Impl";
            }
        }
    }
}
