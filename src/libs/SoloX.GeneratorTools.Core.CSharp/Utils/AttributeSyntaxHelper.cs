// ----------------------------------------------------------------------
// <copyright file="AttributeSyntaxHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;

namespace SoloX.GeneratorTools.Core.CSharp.Utils
{
    /// <summary>
    /// Methods to help Attribute Syntax analysis.
    /// </summary>
    public static class AttributeSyntaxHelper
    {
        /// <summary>
        /// Tell if one of the attribute syntax node name match the given attribute type name.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type to match.</typeparam>
        /// <param name="attributeLists">The attribute syntax lists to test.</param>
        /// <param name="attributeSyntax">The attribute syntax node that match the attribute type.</param>
        /// <returns>True if the attribute type name match one of the attribute syntax node.</returns>
        public static bool TryMatchAttributeName<TAttribute>(
            this SyntaxList<AttributeListSyntax> attributeLists,
            out AttributeSyntax attributeSyntax)
            where TAttribute : Attribute
        {
            attributeSyntax = null;
            foreach (var attributeList in attributeLists)
            {
                if (attributeList.TryMatchAttributeName<TAttribute>(out attributeSyntax))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tell if one of the attribute syntax node name match the given attribute type name.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type to match.</typeparam>
        /// <param name="attributeLists">The attribute syntax lists to test.</param>
        /// <param name="attribute">The attribute syntax node that match the attribute type.</param>
        /// <returns>True if the attribute type name match one of the attribute syntax node.</returns>
        public static bool TryMatchAttributeName<TAttribute>(
            this IEnumerable<IAttributeUse> attributeLists,
            out IAttributeUse attribute)
            where TAttribute : Attribute
        {
            attribute = null;
            if (attributeLists != null)
            {
                foreach (var attributeUse in attributeLists)
                {
                    if (attributeUse.Name == typeof(TAttribute).Name)
                    {
                        attribute = attributeUse;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tell if one of the attribute syntax node name match the given attribute type name.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type to match.</typeparam>
        /// <param name="attributeLists">The attribute syntax lists to test.</param>
        /// <param name="matchingAttributeSyntax">The matching attribute syntax node.</param>
        /// <returns>True if the attribute type name match one of the attribute syntax node.</returns>
        public static bool TryMatchAttributeName<TAttribute>(
            this AttributeListSyntax attributeLists,
            out AttributeSyntax matchingAttributeSyntax)
            where TAttribute : Attribute
        {
            matchingAttributeSyntax = null;
            if (attributeLists != null)
            {
                foreach (var attributeSyntax in attributeLists.Attributes)
                {
                    if (attributeSyntax.IsAttributeName<TAttribute>())
                    {
                        matchingAttributeSyntax = attributeSyntax;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tell if the given attribute type match the syntax attribute name.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type to match.</typeparam>
        /// <param name="attributeSyntax">The attribute syntax node to test.</param>
        /// <returns>True if the attribute syntax name match.</returns>
        public static bool IsAttributeName<TAttribute>(this AttributeSyntax attributeSyntax)
            where TAttribute : Attribute
        {
            string name = null;
            if (attributeSyntax != null)
            {
                if (attributeSyntax.Name.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.GenericName)
                {
                    name = ((GenericNameSyntax)attributeSyntax.Name).Identifier.ToString();
                }
                else
                {
                    name = attributeSyntax.Name.ToString();
                }
            }

            var attributeName = typeof(TAttribute).Name;
            if (attributeName.Equals(name, StringComparison.Ordinal))
            {
                return true;
            }

            var attribute = nameof(Attribute);
            if (attributeName.EndsWith(attribute, StringComparison.InvariantCulture))
            {
                return attributeName
                    .Substring(0, attributeName.Length - attribute.Length)
                    .Equals(name, StringComparison.Ordinal);
            }

            return false;
        }
    }
}
