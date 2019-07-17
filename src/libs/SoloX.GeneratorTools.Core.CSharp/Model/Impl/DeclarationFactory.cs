// ----------------------------------------------------------------------
// <copyright file="DeclarationFactory.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Declaration factory.
    /// </summary>
    internal static class DeclarationFactory
    {
        /// <summary>
        /// Create an interface declaration from a syntax node.
        /// </summary>
        /// <param name="nameSpace">The interface name space.</param>
        /// <param name="usingDirectives">The using directives.</param>
        /// <param name="node">The interface syntax node to load the declaration from.</param>
        /// <param name="location">Location of the declaration.</param>
        /// <returns>The created interface declaration object.</returns>
        public static IInterfaceDeclaration CreateInterfaceDeclaration(
            string nameSpace,
            IReadOnlyList<string> usingDirectives,
            InterfaceDeclarationSyntax node,
            string location)
        {
            return new InterfaceDeclaration(
                nameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<InterfaceDeclarationSyntax>(node),
                usingDirectives,
                location,
                ParserGenericDeclarationLoader<InterfaceDeclarationSyntax>.Shared);
        }

        /// <summary>
        /// Create an interface declaration from a syntax node.
        /// </summary>
        /// <param name="type">The interface type to load the declaration from.</param>
        /// <returns>The created interface declaration object.</returns>
        public static IInterfaceDeclaration CreateInterfaceDeclaration(Type type)
        {
            var decl = new InterfaceDeclaration(
                type.Namespace,
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                new ReflectionTypeSyntaxNodeProvider<InterfaceDeclarationSyntax>(type),
                null,
                type.Assembly.Location,
                ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>.Shared);

            decl.DeclarationType = type;
            return decl;
        }

        /// <summary>
        /// Create a class declaration from a syntax node.
        /// </summary>
        /// <param name="nameSpace">The class name space.</param>
        /// <param name="usingDirectives">The using directives.</param>
        /// <param name="node">The class syntax node to load the declaration from.</param>
        /// <param name="location">Location of the declaration.</param>
        /// <returns>The created class declaration object.</returns>
        public static IClassDeclaration CreateClassDeclaration(
            string nameSpace,
            IReadOnlyList<string> usingDirectives,
            ClassDeclarationSyntax node,
            string location)
        {
            return new ClassDeclaration(
                nameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<ClassDeclarationSyntax>(node),
                usingDirectives,
                location,
                ParserGenericDeclarationLoader<ClassDeclarationSyntax>.Shared);
        }

        /// <summary>
        /// Create a class declaration from a syntax node.
        /// </summary>
        /// <param name="type">The class type to load the declaration from.</param>
        /// <returns>The created class declaration object.</returns>
        public static IClassDeclaration CreateClassDeclaration(Type type)
        {
            var decl = new ClassDeclaration(
                type.Namespace,
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                new ReflectionTypeSyntaxNodeProvider<ClassDeclarationSyntax>(type),
                null,
                type.Assembly.Location,
                ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>.Shared);

            decl.DeclarationType = type;
            return decl;
        }

        /// <summary>
        /// Create a struct declaration from a syntax node.
        /// </summary>
        /// <param name="nameSpace">The struct name space.</param>
        /// <param name="usingDirectives">The using directives.</param>
        /// <param name="node">The struct syntax node to load the declaration from.</param>
        /// <param name="location">Location of the declaration.</param>
        /// <returns>The created struct declaration object.</returns>
        public static IStructDeclaration CreateStructDeclaration(
            string nameSpace,
            IReadOnlyList<string> usingDirectives,
            StructDeclarationSyntax node,
            string location)
        {
            return new StructDeclaration(
                nameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<StructDeclarationSyntax>(node),
                usingDirectives,
                location,
                ParserGenericDeclarationLoader<StructDeclarationSyntax>.Shared);
        }

        /// <summary>
        /// Create an enum declaration from a syntax node.
        /// </summary>
        /// <param name="nameSpace">The enum name space.</param>
        /// <param name="usingDirectives">The using directives.</param>
        /// <param name="node">The enum syntax node to load the declaration from.</param>
        /// <param name="location">Location of the declaration.</param>
        /// <returns>The created enum declaration object.</returns>
        public static IEnumDeclaration CreateEnumDeclaration(
            string nameSpace,
            IReadOnlyList<string> usingDirectives,
            EnumDeclarationSyntax node,
            string location)
        {
            return new EnumDeclaration(
                nameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<EnumDeclarationSyntax>(node),
                usingDirectives,
                location);
        }
    }
}
