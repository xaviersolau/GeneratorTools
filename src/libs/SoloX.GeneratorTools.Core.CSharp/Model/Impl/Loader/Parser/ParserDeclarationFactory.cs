// ----------------------------------------------------------------------
// <copyright file="ParserDeclarationFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Parser
{
    /// <summary>
    /// Declaration factory.
    /// </summary>
    internal class ParserDeclarationFactory : IParserDeclarationFactory
    {
        private readonly ParserGenericDeclarationLoader<InterfaceDeclarationSyntax> parserLoaderInterfaceDeclarationSyntax;
        private readonly ParserGenericDeclarationLoader<ClassDeclarationSyntax> parserLoaderClassDeclarationSyntax;
        private readonly ParserGenericDeclarationLoader<StructDeclarationSyntax> parserLoaderStructDeclarationSyntax;
        private readonly ParserGenericDeclarationLoader<RecordDeclarationSyntax> parserLoaderRecordDeclarationSyntax;

        public ParserDeclarationFactory(
            ParserGenericDeclarationLoader<InterfaceDeclarationSyntax> parserLoaderInterfaceDeclarationSyntax,
            ParserGenericDeclarationLoader<ClassDeclarationSyntax> parserLoaderClassDeclarationSyntax,
            ParserGenericDeclarationLoader<StructDeclarationSyntax> parserLoaderStructDeclarationSyntax,
            ParserGenericDeclarationLoader<RecordDeclarationSyntax> parserLoaderRecordDeclarationSyntax)
        {
            this.parserLoaderInterfaceDeclarationSyntax = parserLoaderInterfaceDeclarationSyntax;
            this.parserLoaderClassDeclarationSyntax = parserLoaderClassDeclarationSyntax;
            this.parserLoaderStructDeclarationSyntax = parserLoaderStructDeclarationSyntax;
            this.parserLoaderRecordDeclarationSyntax = parserLoaderRecordDeclarationSyntax;
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

        /// <inheritdoc/>
        public IRecordDeclaration CreateRecordDeclaration(
            string nameSpace,
            IReadOnlyList<string> usingDirectives,
            RecordDeclarationSyntax node,
            string location)
        {
            return new RecordDeclaration(
                nameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<RecordDeclarationSyntax>(node),
                usingDirectives,
                location,
                this.parserLoaderRecordDeclarationSyntax);
        }

        /// <inheritdoc/>
        public IRecordStructDeclaration CreateRecordStructDeclaration(
            string nameSpace,
            IReadOnlyList<string> usingDirectives,
            RecordDeclarationSyntax node,
            string location)
        {
            return new RecordStructDeclaration(
                nameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<RecordDeclarationSyntax>(node),
                usingDirectives,
                location,
                this.parserLoaderRecordDeclarationSyntax);
        }
    }
}
