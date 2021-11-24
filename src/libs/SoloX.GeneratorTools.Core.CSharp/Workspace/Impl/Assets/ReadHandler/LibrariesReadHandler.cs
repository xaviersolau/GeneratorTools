// ----------------------------------------------------------------------
// <copyright file="LibrariesReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class LibrariesReadHandler : AConverterReadHandler
    {
        private readonly Action<LibraryAssets> addLibrary;

        public LibrariesReadHandler(
            JsonReader reader,
            JsonSerializer serializer,
            AConverterReadHandler parent,
            Action<LibraryAssets> addLibrary)
            : base(reader, serializer, parent)
        {
            this.addLibrary = addLibrary;
        }

        protected override AConverterReadHandler Handle(JsonToken tknType)
        {
#pragma warning disable IDE0010 // Ajouter les instructions case manquantes
            switch (tknType)
            {
                case JsonToken.StartObject:
                    break;
                case JsonToken.EndObject:
                    return this.Parent;
                case JsonToken.PropertyName:
                    var libraryName = (string)this.Reader.Value;
                    var library = new LibraryAssets(libraryName);
                    this.addLibrary(library);
                    return new LibraryReadHandler(this.Reader, this.Serializer, this, library);
                default:
                    break;
            }
#pragma warning restore IDE0010 // Ajouter les instructions case manquantes

            return this;
        }
    }
}
