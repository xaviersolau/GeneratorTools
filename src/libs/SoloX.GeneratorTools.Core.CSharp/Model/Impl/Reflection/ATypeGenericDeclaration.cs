// ----------------------------------------------------------------------
// <copyright file="ATypeGenericDeclaration.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model.Use.Impl;

namespace SoloX.GeneratorTools.Core.CSharp.Model.Impl.Reflection
{
    /// <summary>
    /// Base abstract generic declaration from Type reflection implementation.
    /// </summary>
    public abstract class ATypeGenericDeclaration : ADeclaration, IGenericDeclaration
    {
        private readonly List<IGenericDeclaration> extendedBy = new List<IGenericDeclaration>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ATypeGenericDeclaration"/> class.
        /// </summary>
        /// <param name="type">Type to load the declaration from.</param>
        protected ATypeGenericDeclaration(Type type)
            : base(type.Namespace, GetNameWithoutGeneric(type.Name), null, null, type.Assembly.Location)
        {
            this.DeclarationType = type;
            this.TypeParameterListSyntax = null;
        }

        /// <inheritdoc/>
        public TypeParameterListSyntax TypeParameterListSyntax { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IGenericParameterDeclaration> GenericParameters { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IDeclarationUse> Extends { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IGenericDeclaration> ExtendedBy => this.extendedBy;

        /// <inheritdoc/>
        public IReadOnlyCollection<IMemberDeclaration> Members { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IPropertyDeclaration> Properties { get; private set; }

        /// <summary>
        /// Gets the declaration type.
        /// </summary>
        public Type DeclarationType { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.Extends?.Any() ?? false)
            {
                return $"{base.ToString()}: {string.Join(", ", this.Extends?.Select(e => e.ToString()))}";
            }

            return base.ToString();
        }

        internal static string GetNameWithoutGeneric(string name)
        {
            var genericIdx = name.IndexOf('`');
            if (genericIdx >= 0)
            {
                return name.Substring(0, genericIdx);
            }

            return name;
        }

        internal static IDeclarationUse GetDeclarationUseFrom(Type type, IDeclarationResolver resolver)
        {
            if (type.Namespace == "System" && (type.IsPrimitive || type == typeof(string) || type == typeof(object)))
            {
                return new PredefinedDeclarationUse(null, type.Name);
            }

            var interfaceDeclaration = resolver.Resolve(type);

            if (interfaceDeclaration == null)
            {
                return new UnknownDeclarationUse(null, new UnknownDeclaration(type.Name));
            }

            IReadOnlyCollection<IDeclarationUse> genericParameters;

            if (type.IsGenericType)
            {
                var uses = new List<IDeclarationUse>();
                foreach (var typeArg in type.GenericTypeArguments)
                {
                    uses.Add(GetDeclarationUseFrom(typeArg, resolver));
                }

                genericParameters = uses;
            }
            else
            {
                genericParameters = Array.Empty<IDeclarationUse>();
            }

            return new GenericDeclarationUse(null, interfaceDeclaration, genericParameters);
        }

        /// <summary>
        /// Load the generic parameters from the type parameter list node.
        /// </summary>
        protected void LoadGenericParameters()
        {
            if (this.DeclarationType.IsGenericTypeDefinition)
            {
                var parameterSet = new List<IGenericParameterDeclaration>();

                foreach (var parameter in this.DeclarationType.GetTypeInfo().GenericTypeParameters)
                {
                    parameterSet.Add(new GenericParameterDeclaration(parameter.Name, null));
                }

                this.GenericParameters = parameterSet;
            }
            else
            {
                this.GenericParameters = Array.Empty<IGenericParameterDeclaration>();
            }
        }

        /// <summary>
        /// Load extends statement list.
        /// </summary>
        /// <param name="resolver">The resolver to resolve dependencies.</param>
        protected void LoadExtends(IDeclarationResolver resolver)
        {
            var extendedInterfaces = this.DeclarationType.GetInterfaces();
            if (extendedInterfaces != null && extendedInterfaces.Any())
            {
                var uses = new List<IDeclarationUse>();

                foreach (var extendedInterface in extendedInterfaces)
                {
                    uses.Add(GetDeclarationUseFrom(extendedInterface, resolver));
                }

                this.Extends = uses;
            }
            else
            {
                this.Extends = Array.Empty<IDeclarationUse>();
            }
        }

        /// <summary>
        /// Load member list.
        /// </summary>
        /// <param name="resolver">The resolver to resolve dependencies.</param>
        protected void LoadMembers(IDeclarationResolver resolver)
        {
            var memberList = new List<IMemberDeclaration>();

            foreach (var property in this.DeclarationType.GetProperties())
            {
                memberList.Add(
                    new PropertyDeclaration(
                        property.Name,
                        GetDeclarationUseFrom(property.PropertyType, resolver),
                        null));
            }

            this.Members = memberList.Any() ? memberList.ToArray() : Array.Empty<IMemberDeclaration>();
            this.Properties = this.Members.OfType<IPropertyDeclaration>().ToArray();
        }

        private void AddExtendedBy(AGenericDeclaration declaration)
        {
            this.extendedBy.Add(declaration);
        }
    }
}
