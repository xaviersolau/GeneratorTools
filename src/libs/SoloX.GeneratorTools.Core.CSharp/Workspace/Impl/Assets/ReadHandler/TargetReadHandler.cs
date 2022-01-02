// ----------------------------------------------------------------------
// <copyright file="TargetReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Text.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class TargetReadHandler : AConverterReadHandler
    {
        private readonly TargetAssets targetAssets;

        public TargetReadHandler(
            JsonSerializerOptions options,
            AConverterReadHandler parent,
            TargetAssets targetAssets)
            : base(options, parent)
        {
            this.targetAssets = targetAssets;
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
                    var targetItemName = reader.GetString();
                    var targetItemAssets = new TargetItemAssets(targetItemName);
                    this.targetAssets.AddTargetItem(targetItemAssets);
                    return new TargetItemReadHandler(this.Options, this, targetItemAssets);
                default:
                    break;
            }
#pragma warning restore IDE0010 // Ajouter les instructions case manquantes

            return this;
        }
    }
}
