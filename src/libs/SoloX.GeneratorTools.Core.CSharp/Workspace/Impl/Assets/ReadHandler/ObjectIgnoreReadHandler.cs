// ----------------------------------------------------------------------
// <copyright file="ObjectIgnoreReadHandler.cs" company="SoloX Software">
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
    internal class ObjectIgnoreReadHandler : AConverterReadHandler
    {
        private int nestedLevel;

        public ObjectIgnoreReadHandler(JsonReader reader, JsonSerializer serializer, AConverterReadHandler parent)
            : base(reader, serializer, parent)
        {
        }

        protected override AConverterReadHandler Handle(JsonToken tknType)
        {
            switch (tknType)
            {
                case JsonToken.StartObject:
                    this.nestedLevel++;
                    break;
                case JsonToken.EndObject:
                    this.nestedLevel--;
                    break;
                default:
                    break;
            }

            if (this.nestedLevel == 0)
            {
                return this.Parent;
            }

            return this;
        }
    }
}
