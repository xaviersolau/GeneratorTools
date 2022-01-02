// ----------------------------------------------------------------------
// <copyright file="LibrariesReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Text.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class LibrariesReadHandler : AConverterReadHandler
    {
        private readonly Action<LibraryAssets> addLibrary;

        public LibrariesReadHandler(
            JsonSerializerOptions options,
            AConverterReadHandler parent,
            Action<LibraryAssets> addLibrary)
            : base(options, parent)
        {
            this.addLibrary = addLibrary;
        }

        protected override AConverterReadHandler Handle(ref Utf8JsonReader reader, JsonTokenType tknType)
        {
#pragma warning disable IDE0010 // Ajouter les instructions case manquantes
            switch (tknType)
            {
                case JsonTokenType.StartObject:
                    break;
                case JsonTokenType.EndObject:
                    return this.Parent;
                case JsonTokenType.PropertyName:
                    var libraryName = reader.GetString();
                    var library = new LibraryAssets(libraryName);
                    this.addLibrary(library);
                    return new LibraryReadHandler(this.Options, this, library);
                default:
                    break;
            }
#pragma warning restore IDE0010 // Ajouter les instructions case manquantes

            return this;
        }
    }
}
