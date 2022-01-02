// ----------------------------------------------------------------------
// <copyright file="AConverterReadHandler.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Text.Json;

namespace SoloX.GeneratorTools.Core.CSharp.Workspace.Impl.Assets.ReadHandler
{
    /// <summary>
    /// Converter Read Handler interface.
    /// </summary>
    internal abstract class AConverterReadHandler
    {
        internal AConverterReadHandler(JsonSerializerOptions options, AConverterReadHandler parent)
        {
            this.Options = options;
            this.Parent = parent;
        }

        protected AConverterReadHandler Parent { get; }

        protected JsonSerializerOptions Options { get; }

        /// <summary>
        /// Handle Converter Read operation.
        /// </summary>
        /// <returns>The next Converter Read Handler.</returns>
        internal AConverterReadHandler Handle(ref Utf8JsonReader reader)
        {
            var tknType = reader.TokenType;
            var res = this.Handle(ref reader, tknType);
            return reader.Read() ? res : null;
        }

        protected abstract AConverterReadHandler Handle(ref Utf8JsonReader reader, JsonTokenType tknType);
    }
}
