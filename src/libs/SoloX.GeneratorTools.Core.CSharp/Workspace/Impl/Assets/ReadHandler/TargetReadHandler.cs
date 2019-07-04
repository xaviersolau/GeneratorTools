// ----------------------------------------------------------------------
// <copyright file="TargetReadHandler.cs" company="SoloX Software">
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
    internal class TargetReadHandler : AConverterReadHandler
    {
        private TargetAssets targetAssets;

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

            return this;
        }
    }
}
