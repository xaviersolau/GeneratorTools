// ----------------------------------------------------------------------
// <copyright file="AConverterReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Newtonsoft.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    /// <summary>
    /// Converter Read Handler interface.
    /// </summary>
    internal abstract class AConverterReadHandler
    {
        internal AConverterReadHandler(JsonReader reader, JsonSerializer serializer, AConverterReadHandler parent)
        {
            this.Reader = reader;
            this.Serializer = serializer;
            this.Parent = parent;
        }

        protected JsonReader Reader { get; }

        protected JsonSerializer Serializer { get; }

        protected AConverterReadHandler Parent { get; }

        /// <summary>
        /// Handle Converter Read operation.
        /// </summary>
        /// <returns>The next Converter Read Handler.</returns>
        internal AConverterReadHandler Handle()
        {
            var tknType = this.Reader.TokenType;
            var res = this.Handle(tknType);
            return this.Reader.Read() ? res : null;
        }

        protected abstract AConverterReadHandler Handle(JsonToken tknType);
    }
}
