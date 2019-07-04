// ----------------------------------------------------------------------
// <copyright file="TargetsReadHandler.cs" company="SoloX Software">
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
    internal class TargetsReadHandler : AConverterReadHandler
    {
        private Action<TargetAssets> addTarget;

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

            return this;
        }
    }
}
