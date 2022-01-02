// ----------------------------------------------------------------------
// <copyright file="ProjectAssetsReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Text.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class ProjectAssetsReadHandler : AConverterReadHandler
    {
        public ProjectAssetsReadHandler(JsonSerializerOptions options)
            : base(options, null)
        {
            this.ProjectAssets = new ProjectAssets();
        }

        internal ProjectAssets ProjectAssets { get; }

        protected override AConverterReadHandler Handle(ref Utf8JsonReader reader, JsonTokenType tknType)
        {
#pragma warning disable IDE0010 // Ajouter les instructions case manquantes
            switch (tknType)
            {
                case JsonTokenType.StartObject:
                    break;
                case JsonTokenType.EndObject:
                    break;
                case JsonTokenType.PropertyName:
                    var propertyName = reader.GetString();
                    switch (propertyName)
                    {
                        case "version":
                            reader.Read();
                            this.ProjectAssets.Version = reader.GetInt32();
                            break;
                        case "targets":
                            return new TargetsReadHandler(this.Options, this, this.ProjectAssets.AddTarget);
                        case "libraries":
                            return new LibrariesReadHandler(this.Options, this, this.ProjectAssets.AddLibrary);
                        case "packageFolders":
                            return new KeysReadHandler(this.Options, this, this.ProjectAssets.AddPackageFolder);
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
