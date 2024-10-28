//-----------------------------------------------------------------------
// <copyright file="HttpRequestDataMockBody.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http
{
    using System.Text.Json;
    using Microsoft.Azure.Functions.Worker.Http;

    /// <summary>
    /// Body of the <see cref="HttpRequestData"/> to set in the <see cref="HttpRequestData.Body"/>.
    /// </summary>
    public class HttpRequestDataMockBody
    {
        internal HttpRequestDataMockBody()
        {
            this.Stream = Stream.Null;
        }

        internal Stream Stream { get; set; }

        /// <summary>
        /// Sets the <see cref="HttpRequestData.Body"/> with the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> which will be used in the <see cref="HttpRequestData.Body"/>.</param>
        public void Set(Stream stream)
        {
            this.Stream = stream;
        }

        /// <summary>
        /// Sets the <see cref="HttpRequestData.Body"/> with the specified <paramref name="bytes"/> bytes.
        /// </summary>
        /// <param name="bytes">Bytes which will be used in the <see cref="HttpRequestData.Body"/>.</param>
        public void Set(ReadOnlySpan<byte> bytes)
        {
            this.Stream = new MemoryStream(bytes.ToArray(), false);
        }

        /// <summary>
        /// Sets the <see cref="HttpRequestData.Body"/> with a serialized <paramref name="json"/> content.
        /// </summary>
        /// <param name="json">Object to serialize into JSON.</param>
        /// <param name="options">Options of the serialization.</param>
        public void SetJson(object? json, JsonSerializerOptions? options = null)
        {
            this.Stream = new MemoryStream();

            JsonSerializer.Serialize(this.Stream, json, options);

            this.Stream.Position = 0;
        }
    }
}
