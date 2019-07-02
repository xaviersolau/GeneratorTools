// ----------------------------------------------------------------------
// <copyright file="ProjectAssetsReadHandler.cs" company="SoloX Software">
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
    internal class ProjectAssetsReadHandler : AConverterReadHandler
    {
        public ProjectAssetsReadHandler(JsonReader reader, JsonSerializer serializer)
            : base(reader, serializer, null)
        {
            this.ProjectAssets = new ProjectAssets();
        }

        internal ProjectAssets ProjectAssets { get; }

        protected override AConverterReadHandler Handle(JsonToken tknType)
        {
            switch (tknType)
            {
                case JsonToken.StartObject:
                    break;
                case JsonToken.EndObject:
                    break;
                case JsonToken.PropertyName:
                    var propertyName = (string)this.Reader.Value;
                    switch (propertyName)
                    {
                        case "version":
                            this.ProjectAssets.Version = this.Reader.ReadAsInt32().Value;
                            break;
                        case "targets":
                            return new TargetsReadHandler(this.Reader, this.Serializer, this, this.ProjectAssets.AddTarget);
                        case "packageFolders":
                            return new KeysReadHandler(this.Reader, this.Serializer, this, this.ProjectAssets.AddPackageFolder);
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
