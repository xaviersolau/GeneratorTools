// ----------------------------------------------------------------------
// <copyright file="TargetReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Newtonsoft.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class TargetReadHandler : AConverterReadHandler
    {
        private readonly TargetAssets targetAssets;

        public TargetReadHandler(
            JsonReader reader,
            JsonSerializer serializer,
            AConverterReadHandler parent,
            TargetAssets targetAssets)
            : base(reader, serializer, parent)
        {
            this.targetAssets = targetAssets;
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
                    var targetItemName = (string)this.Reader.Value;
                    var targetItemAssets = new TargetItemAssets(targetItemName);
                    this.targetAssets.AddTargetItem(targetItemAssets);
                    return new TargetItemReadHandler(this.Reader, this.Serializer, this, targetItemAssets);
                default:
                    break;
            }
#pragma warning restore IDE0010 // Ajouter les instructions case manquantes

            return this;
        }
    }
}
