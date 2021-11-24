﻿// ----------------------------------------------------------------------
// <copyright file="LibraryReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Newtonsoft.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class LibraryReadHandler : AConverterReadHandler
    {
        private readonly LibraryAssets library;

        public LibraryReadHandler(
            JsonReader reader,
            JsonSerializer serializer,
            AConverterReadHandler parent,
            LibraryAssets library)
            : base(reader, serializer, parent)
        {
            this.library = library;
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
                    var propertyName = (string)this.Reader.Value;
                    switch (propertyName)
                    {
                        case "path":
                            this.library.Path = this.Reader.ReadAsString();
                            break;
                        default:
                            return new ObjectIgnoreReadHandler(this.Reader, this.Serializer, this);
                    }
                    break;
                default:
                    break;
            }
#pragma warning restore IDE0010 // Ajouter les instructions case manquantes

            return this;
        }
    }
}
