// ----------------------------------------------------------------------
// <copyright file="ResourcesPathHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;

namespace SoloX.GeneratorTools.Core.CSharp.UTest.Utils
{
    /// <summary>
    /// Resource path method extension helper.
    /// </summary>
    public static class ResourcesPathHelper
    {
        /// <summary>
        /// Convert the given class name to the Resources/Model/Basic/Classes path.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The class file path in Resources/Model/Basic/Classes.</returns>
        public static string ToBasicClassesPath(this string className)
        {
            return className.ToBasicPath("Classes");
        }

        /// <summary>
        /// Convert the given class name to the Resources/Model/Basic/Interfaces path.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The class file path in Resources/Model/Basic/Interfaces.</returns>
        public static string ToBasicInterfacesPath(this string className)
        {
            return className.ToBasicPath("Interfaces");
        }

        /// <summary>
        /// Convert the given class name to the Resources/Model/Basic/Enums path.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The class file path in Resources/Model/Basic/Enums.</returns>
        public static string ToBasicEnumsPath(this string className)
        {
            return className.ToBasicPath("Enums");
        }

        /// <summary>
        /// Convert the given class name to the Resources/Model/Basic/Structs path.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The class file path in Resources/Model/Basic/Structs.</returns>
        public static string ToBasicStructsPath(this string className)
        {
            return className.ToBasicPath("Structs");
        }

        /// <summary>
        /// Convert the given class name to the Resources/Model/Basic/Records path.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The class file path in Resources/Model/Basic/Records.</returns>
        public static string ToBasicRecordsPath(this string className)
        {
            return className.ToBasicPath("Records");
        }

        /// <summary>
        /// Convert the given class name to the Resources/Model/Basic/RecordStructs path.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The class file path in Resources/Model/Basic/RecordStructs.</returns>
        public static string ToBasicRecordStructsPath(this string className)
        {
            return className.ToBasicPath("RecordStructs");
        }

        /// <summary>
        /// Convert the given class name to the Resources/Model/Basic/??? path.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The class file path in Resources/Model/Basic/???.</returns>
        public static string ToBasicPath(this string className, string path)
        {
            return Path.Combine(".", "Resources", "Model", "Basic", path, $"{className}.cs");
        }
    }
}
