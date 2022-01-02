// ----------------------------------------------------------------------
// <copyright file="ObjectIgnoreReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Text.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    internal class ObjectIgnoreReadHandler : AConverterReadHandler
    {
        private int nestedLevel;

        public ObjectIgnoreReadHandler(JsonSerializerOptions options, AConverterReadHandler parent)
            : base(options, parent)
        {
        }

        protected override AConverterReadHandler Handle(ref Utf8JsonReader reader, JsonTokenType tknType)
        {
#pragma warning disable IDE0010 // Ajouter les instructions case manquantes
            switch (tknType)
            {
                case JsonTokenType.StartObject:
                    this.nestedLevel++;
                    break;
                case JsonTokenType.EndObject:
                    this.nestedLevel--;
                    break;
                default:
                    break;
            }
#pragma warning restore IDE0010 // Ajouter les instructions case manquantes

            if (this.nestedLevel == 0)
            {
                return this.Parent;
            }

            return this;
        }
    }
}
