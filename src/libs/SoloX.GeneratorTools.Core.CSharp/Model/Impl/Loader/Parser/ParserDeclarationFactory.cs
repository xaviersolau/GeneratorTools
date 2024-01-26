// ----------------------------------------------------------------------
// <copyright file="ParserDeclarationFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

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
        private readonly ParserEnumDeclarationLoader parserLoaderEnumDeclarationSyntax;

        public ParserDeclarationFactory(
            ParserGenericDeclarationLoader<InterfaceDeclarationSyntax> parserLoaderInterfaceDeclarationSyntax,
            ParserGenericDeclarationLoader<ClassDeclarationSyntax> parserLoaderClassDeclarationSyntax,
            ParserGenericDeclarationLoader<StructDeclarationSyntax> parserLoaderStructDeclarationSyntax,
            ParserGenericDeclarationLoader<RecordDeclarationSyntax> parserLoaderRecordDeclarationSyntax,
            ParserEnumDeclarationLoader parserLoaderEnumDeclarationSyntax)
        {
            this.parserLoaderInterfaceDeclarationSyntax = parserLoaderInterfaceDeclarationSyntax;
            this.parserLoaderClassDeclarationSyntax = parserLoaderClassDeclarationSyntax;
            this.parserLoaderStructDeclarationSyntax = parserLoaderStructDeclarationSyntax;
            this.parserLoaderRecordDeclarationSyntax = parserLoaderRecordDeclarationSyntax;
            this.parserLoaderEnumDeclarationSyntax = parserLoaderEnumDeclarationSyntax;
        }

        /// <inheritdoc/>
        public IInterfaceDeclaration CreateInterfaceDeclaration(
            string nameSpace,
            IUsingDirectives usingDirectives,
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
            IUsingDirectives usingDirectives,
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
            IUsingDirectives usingDirectives,
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
            IUsingDirectives usingDirectives,
            EnumDeclarationSyntax node,
            string location)
        {
            return new EnumDeclaration(
                nameSpace,
                node.Identifier.Text,
                new ParserSyntaxNodeProvider<EnumDeclarationSyntax>(node),
                usingDirectives,
                location,
                this.parserLoaderEnumDeclarationSyntax);
        }

        /// <inheritdoc/>
        public IRecordDeclaration CreateRecordDeclaration(
            string nameSpace,
            IUsingDirectives usingDirectives,
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
            IUsingDirectives usingDirectives,
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
