// ----------------------------------------------------------------------
// <copyright file="RelativeLocator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoloX.GeneratorTools.Core.Generator.Impl
{
    /// <summary>
    /// Relative locator implementation.
    /// </summary>
    public class RelativeLocator : ILocator
    {
        private readonly string baseFolder;
        private readonly string baseNameSpace;
        private readonly string suffix;
        private readonly string suffixPath;
        private readonly string fallBackSubNameSpace;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeLocator"/> class.
        /// </summary>
        /// <param name="baseFolder">The base folder of the locator.</param>
        /// <param name="baseNameSpace">The base name space.</param>
        /// <param name="suffix">NameSpace suffix.</param>
        /// <param name="fallBackSubNameSpace">The fall back name space.</param>
        public RelativeLocator(string baseFolder, string baseNameSpace, string suffix = null, string fallBackSubNameSpace = null)
        {
            this.baseFolder = baseFolder;
            this.baseNameSpace = baseNameSpace;
            this.suffix = suffix;
            this.suffixPath = this.suffix?.Replace('.', Path.DirectorySeparatorChar);
            this.fallBackSubNameSpace = fallBackSubNameSpace;
        }

        /// <inheritdoc/>
        public (string location, string nameSpace) ComputeTargetLocation(string declarationNameSpace)
        {
            if (declarationNameSpace == null)
            {
                throw new ArgumentNullException($"The argument {nameof(declarationNameSpace)} was null.");
            }

            var baseNameSpaceLength = this.baseNameSpace.Length;

            if (declarationNameSpace == this.baseNameSpace)
            {
                if (string.IsNullOrEmpty(this.suffix))
                {
                    return (this.baseFolder, this.baseNameSpace);
                }

                return (Path.Combine(this.baseFolder, this.suffixPath), $"{this.baseNameSpace}.{this.suffix}");
            }
            else if (declarationNameSpace.Length > baseNameSpaceLength + 1 &&
                declarationNameSpace[baseNameSpaceLength] == '.' &&
                declarationNameSpace.StartsWith(this.baseNameSpace, StringComparison.InvariantCulture))
            {
                var subNameSpace = declarationNameSpace.Substring(baseNameSpaceLength + 1);
                var subPath = subNameSpace.Replace('.', Path.DirectorySeparatorChar);

                if (string.IsNullOrEmpty(this.suffix))
                {
                    return (Path.Combine(this.baseFolder, subPath), declarationNameSpace);
                }

                return (Path.Combine(this.baseFolder, subPath, this.suffixPath), $"{declarationNameSpace}.{this.suffix}");
            }
            else
            {
                if (string.IsNullOrEmpty(this.fallBackSubNameSpace))
                {
                    if (string.IsNullOrEmpty(this.suffix))
                    {
                        return (this.baseFolder, this.baseNameSpace);
                    }

                    return (Path.Combine(this.baseFolder, this.suffixPath), $"{this.baseNameSpace}.{this.suffix}");
                }

                var subPath = this.fallBackSubNameSpace.Replace('.', Path.DirectorySeparatorChar);

                if (string.IsNullOrEmpty(this.suffix))
                {
                    return (Path.Combine(this.baseFolder, subPath), $"{this.baseNameSpace}.{this.fallBackSubNameSpace}");
                }

                return (Path.Combine(this.baseFolder, subPath, this.suffixPath), $"{this.baseNameSpace}.{this.fallBackSubNameSpace}.{this.suffix}");
            }
        }
    }
}
