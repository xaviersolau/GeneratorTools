// ----------------------------------------------------------------------
// <copyright file="KeysReadHandler.cs" company="SoloX Software">
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
    internal class KeysReadHandler : AConverterReadHandler
    {
        private int nestedLevel;
        private Action<string> addKey;

        public KeysReadHandler(JsonReader reader, JsonSerializer serializer, AConverterReadHandler parent, Action<string> addKey)
            : base(reader, serializer, parent)
        {
            this.addKey = addKey;
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
                case JsonToken.PropertyName:
                    if (this.nestedLevel == 1)
                    {
                        this.addKey((string)this.Reader.Value);
                    }

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
