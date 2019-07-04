// ----------------------------------------------------------------------
// <copyright file="JsonProjectAssetsConverter.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets
{
    /// <summary>
    /// Json ProjectAssets Converter.
    /// </summary>
    public class JsonProjectAssetsConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var root = new ProjectAssetsReadHandler(reader, serializer);

            AConverterReadHandler handler = root;
            while (handler != null)
            {
                handler = handler.Handle();
            }

            return root.ProjectAssets;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
