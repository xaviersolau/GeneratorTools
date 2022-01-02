// ----------------------------------------------------------------------
// <copyright file="JsonProjectAssetsConverter.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets
{
    /// <summary>
    /// Json ProjectAssets Converter.
    /// </summary>
    public class JsonProjectAssetsConverter : JsonConverter<ProjectAssets>
    {
        /// <inheritdoc/>
        public override ProjectAssets Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var root = new ProjectAssetsReadHandler(options);

            AConverterReadHandler handler = root;
            while (handler != null)
            {
                handler = handler.Handle(ref reader);
            }

            return root.ProjectAssets;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ProjectAssets value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
