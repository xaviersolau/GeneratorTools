// ----------------------------------------------------------------------
// <copyright file="TargetsReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Text.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class TargetsReadHandler : AConverterReadHandler
    {
        private readonly Action<TargetAssets> addTarget;

        public TargetsReadHandler(
            JsonSerializerOptions options,
            AConverterReadHandler parent,
            Action<TargetAssets> addTarget)
            : base(options, parent)
        {
            this.addTarget = addTarget;
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
                    var targetName = reader.GetString();

                    var targetAssets = new TargetAssets(targetName);
                    this.addTarget(targetAssets);
                    return new TargetReadHandler(this.Options, this, targetAssets);
                default:
                    break;
            }
#pragma warning restore IDE0010 // Ajouter les instructions case manquantes

            return this;
        }
    }
}
