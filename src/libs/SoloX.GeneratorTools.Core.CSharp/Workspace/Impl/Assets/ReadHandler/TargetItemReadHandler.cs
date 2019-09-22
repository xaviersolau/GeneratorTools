// ----------------------------------------------------------------------
// <copyright file="TargetItemReadHandler.cs" company="SoloX Software">
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
    internal class TargetItemReadHandler : AConverterReadHandler
    {
        private TargetItemAssets targetItemAssets;

        public TargetItemReadHandler(
            JsonReader reader,
            JsonSerializer serializer,
            AConverterReadHandler parent,
            TargetItemAssets targetItemAssets)
            : base(reader, serializer, parent)
        {
            this.targetItemAssets = targetItemAssets;
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
                    var propertyName = (string)this.Reader.Value;
                    switch (propertyName)
                    {
                        case "type":
                            this.targetItemAssets.ItemType = this.Reader.ReadAsString();
                            break;
                        case "framework":
                            this.targetItemAssets.Framework = this.Reader.ReadAsString();
                            break;
                        case "dependencies":
                            this.Reader.Read();
                            var dependencies = this.Serializer.Deserialize<Dictionary<string, string>>(this.Reader);
                            this.targetItemAssets.SetDependencies(dependencies);
                            break;
                        case "build":
                            return new ObjectIgnoreReadHandler(this.Reader, this.Serializer, this);
                        case "compile":
                            return new KeysReadHandler(this.Reader, this.Serializer, this, this.targetItemAssets.AddCompile);
                        case "runtime":
                            return new KeysReadHandler(this.Reader, this.Serializer, this, this.targetItemAssets.AddRuntime);
                        default:
                            return new ObjectIgnoreReadHandler(this.Reader, this.Serializer, this);
                    }

                    break;
                default:
                    break;
            }

            return this;
        }
    }
}
