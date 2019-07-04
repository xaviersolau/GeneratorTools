// ----------------------------------------------------------------------
// <copyright file="LibrariesReadHandler.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class LibrariesReadHandler : AConverterReadHandler
    {
        private Action<LibraryAssets> addLibrary;

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

            return this;
        }
    }
}
