// ----------------------------------------------------------------------
// <copyright file="TargetItemReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Text.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class TargetItemReadHandler : AConverterReadHandler
    {
        private readonly TargetItemAssets targetItemAssets;

        public TargetItemReadHandler(
            JsonSerializerOptions options,
            AConverterReadHandler parent,
            TargetItemAssets targetItemAssets)
            : base(options, parent)
        {
            this.targetItemAssets = targetItemAssets;
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
                        case "type":
                            reader.Read();
                            this.targetItemAssets.ItemType = reader.GetString();
                            break;
                        case "framework":
                            reader.Read();
                            this.targetItemAssets.Framework = reader.GetString();
                            break;
                        case "dependencies":
                            reader.Read();
                            var dependencies = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, this.Options);
                            this.targetItemAssets.SetDependencies(dependencies);
                            break;
                        case "build":
                            return new ObjectIgnoreReadHandler(this.Options, this);
                        case "compile":
                            return new KeysReadHandler(this.Options, this, this.targetItemAssets.AddCompile);
                        case "runtime":
                            return new KeysReadHandler(this.Options, this, this.targetItemAssets.AddRuntime);
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
