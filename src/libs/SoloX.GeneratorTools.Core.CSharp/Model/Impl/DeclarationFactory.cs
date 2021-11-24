// ----------------------------------------------------------------------
// <copyright file="DeclarationFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser;
using SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl
{
    /// <summary>
    /// Declaration factory.
    /// </summary>
    internal class DeclarationFactory : IDeclarationFactory
    {
        private readonly ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax> reflectionLoaderInterfaceDeclarationSyntax;
        private readonly ReflectionGenericDeclarationLoader<ClassDeclarationSyntax> reflectionLoaderClassDeclarationSyntax;
        private readonly ParserGenericDeclarationLoader<InterfaceDeclarationSyntax> parserLoaderInterfaceDeclarationSyntax;
        private readonly ParserGenericDeclarationLoader<ClassDeclarationSyntax> parserLoaderClassDeclarationSyntax;
        private readonly ParserGenericDeclarationLoader<StructDeclarationSyntax> parserLoaderStructDeclarationSyntax;

        public DeclarationFactory(
            ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax> reflectionLoaderInterfaceDeclarationSyntax,
            ReflectionGenericDeclarationLoader<ClassDeclarationSyntax> reflectionLoaderClassDeclarationSyntax,
            ParserGenericDeclarationLoader<InterfaceDeclarationSyntax> parserLoaderInterfaceDeclarationSyntax,
            ParserGenericDeclarationLoader<ClassDeclarationSyntax> parserLoaderClassDeclarationSyntax,
            ParserGenericDeclarationLoader<StructDeclarationSyntax> parserLoaderStructDeclarationSyntax)
        {
            this.reflectionLoaderInterfaceDeclarationSyntax = reflectionLoaderInterfaceDeclarationSyntax;
            this.reflectionLoaderClassDeclarationSyntax = reflectionLoaderClassDeclarationSyntax;

            this.parserLoaderInterfaceDeclarationSyntax = parserLoaderInterfaceDeclarationSyntax;
            this.parserLoaderClassDeclarationSyntax = parserLoaderClassDeclarationSyntax;
            this.parserLoaderStructDeclarationSyntax = parserLoaderStructDeclarationSyntax;
        }

        /// <inheritdoc/>
        public IInterfaceDeclaration CreateInterfaceDeclaration(
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
                this.parserLoaderInterfaceDeclarationSyntax);
        }

        /// <inheritdoc/>
        public IInterfaceDeclaration CreateInterfaceDeclaration(Type type)
        {
            var decl = new InterfaceDeclaration(
                type.Namespace,
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                new ReflectionTypeSyntaxNodeProvider<InterfaceDeclarationSyntax>(type),
                null,
                type.Assembly.Location,
                this.reflectionLoaderInterfaceDeclarationSyntax);

            ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax>.Setup(decl, type);

            return decl;
        }

        /// <inheritdoc/>
        public IClassDeclaration CreateClassDeclaration(
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
                this.parserLoaderClassDeclarationSyntax);
        }

        /// <inheritdoc/>
        public IClassDeclaration CreateClassDeclaration(Type type)
        {
            var decl = new ClassDeclaration(
                type.Namespace,
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                new ReflectionTypeSyntaxNodeProvider<ClassDeclarationSyntax>(type),
                null,
                type.Assembly.Location,
                this.reflectionLoaderClassDeclarationSyntax);

            ReflectionGenericDeclarationLoader<ClassDeclarationSyntax>.Setup(decl, type);

            return decl;
        }

        /// <inheritdoc/>
        public IStructDeclaration CreateStructDeclaration(
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
                this.parserLoaderStructDeclarationSyntax);
        }

        /// <inheritdoc/>
        public IEnumDeclaration CreateEnumDeclaration(
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
