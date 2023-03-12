// ----------------------------------------------------------------------
// <copyright file="ReflectionDeclarationFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Loader.Reflection
{
    /// <summary>
    /// Declaration factory.
    /// </summary>
    internal class ReflectionDeclarationFactory : IReflectionDeclarationFactory
    {
        private readonly ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax> reflectionLoaderInterfaceDeclarationSyntax;
        private readonly ReflectionGenericDeclarationLoader<ClassDeclarationSyntax> reflectionLoaderClassDeclarationSyntax;
        private readonly ReflectionGenericDeclarationLoader<StructDeclarationSyntax> reflectionLoaderStructDeclarationSyntax;
        private readonly ReflectionGenericDeclarationLoader<RecordDeclarationSyntax> reflectionLoaderRecordDeclarationSyntax;

        public ReflectionDeclarationFactory(
            ReflectionGenericDeclarationLoader<InterfaceDeclarationSyntax> reflectionLoaderInterfaceDeclarationSyntax,
            ReflectionGenericDeclarationLoader<ClassDeclarationSyntax> reflectionLoaderClassDeclarationSyntax,
            ReflectionGenericDeclarationLoader<StructDeclarationSyntax> reflectionLoaderStructDeclarationSyntax,
            ReflectionGenericDeclarationLoader<RecordDeclarationSyntax> reflectionLoaderRecordDeclarationSyntax)
        {
            this.reflectionLoaderInterfaceDeclarationSyntax = reflectionLoaderInterfaceDeclarationSyntax;
            this.reflectionLoaderClassDeclarationSyntax = reflectionLoaderClassDeclarationSyntax;
            this.reflectionLoaderStructDeclarationSyntax = reflectionLoaderStructDeclarationSyntax;
            this.reflectionLoaderRecordDeclarationSyntax = reflectionLoaderRecordDeclarationSyntax;
        }

        /// <inheritdoc/>
        public IDeclaration<SyntaxNode> CreateDeclaration(Type type)
        {
            if (type.IsInterface)
            {
                return CreateInterfaceDeclaration(type);
            }
            else if (type.IsEnum)
            {
                return CreateEnumDeclaration(type);
            }
            else if (type.IsValueType)
            {
                if (ReflectionGenericDeclarationLoader<SyntaxNode>.ProbeRecordStructType(type))
                {
                    return CreateRecordStructDeclaration(type);
                }

                return CreateStructDeclaration(type);
            }
            else
            {
                if (ReflectionGenericDeclarationLoader<SyntaxNode>.ProbeRecordType(type))
                {
                    return CreateRecordDeclaration(type);
                }

                return CreateClassDeclaration(type);
            }

            return null;
        }

        private IInterfaceDeclaration CreateInterfaceDeclaration(Type type)
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

        private IClassDeclaration CreateClassDeclaration(Type type)
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

        private IRecordDeclaration CreateRecordDeclaration(Type type)
        {
            var decl = new RecordDeclaration(
                type.Namespace,
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                new ReflectionTypeSyntaxNodeProvider<RecordDeclarationSyntax>(type),
                null,
                type.Assembly.Location,
                this.reflectionLoaderRecordDeclarationSyntax);

            ReflectionGenericDeclarationLoader<RecordDeclarationSyntax>.Setup(decl, type);

            return decl;
        }

        private IRecordStructDeclaration CreateRecordStructDeclaration(Type type)
        {
            var decl = new RecordStructDeclaration(
                type.Namespace,
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                new ReflectionTypeSyntaxNodeProvider<RecordDeclarationSyntax>(type),
                null,
                type.Assembly.Location,
                this.reflectionLoaderRecordDeclarationSyntax);

            ReflectionGenericDeclarationLoader<RecordDeclarationSyntax>.Setup(decl, type);

            return decl;
        }

        private IStructDeclaration CreateStructDeclaration(Type type)
        {
            var decl = new StructDeclaration(
                type.Namespace,
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                new ReflectionTypeSyntaxNodeProvider<StructDeclarationSyntax>(type),
                null,
                type.Assembly.Location,
                this.reflectionLoaderStructDeclarationSyntax);

            ReflectionGenericDeclarationLoader<StructDeclarationSyntax>.Setup(decl, type);

            return decl;
        }

        private static IEnumDeclaration CreateEnumDeclaration(Type type)
        {
            var decl = new EnumDeclaration(
                type.Namespace,
                ReflectionGenericDeclarationLoader<SyntaxNode>.GetNameWithoutGeneric(type.Name),
                new ReflectionTypeSyntaxNodeProvider<EnumDeclarationSyntax>(type),
                null,
                type.Assembly.Location);

            return decl;
        }
    }
}
