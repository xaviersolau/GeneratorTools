// ----------------------------------------------------------------------
// <copyright file="LibraryReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Text.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class LibraryReadHandler : AConverterReadHandler
    {
        private readonly LibraryAssets library;

        public LibraryReadHandler(
            JsonSerializerOptions options,
            AConverterReadHandler parent,
            LibraryAssets library)
            : base(options, parent)
        {
            this.library = library;
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
                    var propertyName = reader.GetString();
                    switch (propertyName)
                    {
                        case "path":
                            reader.Read();
                            this.library.Path = reader.GetString();
                            break;
                        default:
                            return new ObjectIgnoreReadHandler(this.Options, this);
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
