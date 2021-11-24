// ----------------------------------------------------------------------
// <copyright file="TargetsReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class TargetsReadHandler : AConverterReadHandler
    {
        private readonly Action<TargetAssets> addTarget;

        public TargetsReadHandler(
            JsonReader reader,
            JsonSerializer serializer,
            AConverterReadHandler parent,
            Action<TargetAssets> addTarget)
            : base(reader, serializer, parent)
        {
            this.addTarget = addTarget;
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
                    var targetName = (string)this.Reader.Value;

                    var targetAssets = new TargetAssets(targetName);
                    this.addTarget(targetAssets);
                    return new TargetReadHandler(this.Reader, this.Serializer, this, targetAssets);
                default:
                    break;
            }
#pragma warning restore IDE0010 // Ajouter les instructions case manquantes

            return this;
        }
    }
}
