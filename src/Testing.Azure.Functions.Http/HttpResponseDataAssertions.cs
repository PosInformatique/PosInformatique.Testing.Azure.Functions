//-----------------------------------------------------------------------
// <copyright file="HttpResponseDataAssertions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http
{
    using System.Text;
    using System.Text.Json;
    using global::FluentAssertions;
    using global::FluentAssertions.Common;
    using Microsoft.Azure.Functions.Worker.Http;

    /// <summary>
    /// Allows to assert a <see cref="HttpResponseData"/> created
    /// by a <see cref="HttpRequestDataMock"/>.
    /// </summary>
    public sealed class HttpResponseDataAssertions
    {
        private readonly HttpResponseDataImplementation response;

        internal HttpResponseDataAssertions(HttpResponseDataImplementation response)
        {
            this.response = response;
        }

        /// <summary>
        /// Gets the <see cref="HttpResponseData"/> currently asserted.
        /// </summary>
        public HttpResponseData Response => this.response;

        /// <summary>
        /// Assert the content of the <see cref="HttpResponseData.Body"/>.
        /// </summary>
        /// <param name="body">Content of the <see cref="HttpResponseData.Body"/> to assert.</param>
        /// <returns>The current instance of the <see cref="HttpResponseDataAssertions"/> to continue the assertions.</returns>
        public HttpResponseDataAssertions WithBody(Action<Stream> body)
        {
            var bodyStream = this.response.GetBodyStream();

            body(bodyStream);

            return this;
        }

        /// <summary>
        /// Assert the content of the <see cref="HttpResponseData.Body"/> is empty.
        /// </summary>
        /// <returns>The current instance of the <see cref="HttpResponseDataAssertions"/> to continue the assertions.</returns>
        public HttpResponseDataAssertions WithEmptyBody()
        {
            var bodyStream = this.response.GetBodyStream();

            var byteRead = bodyStream.ReadByte();

            if (byteRead != -1)
            {
                throw new AzureFunctionsAssertionFailedException("The body of the response is not empty.");
            }

            return this;
        }

        /// <summary>
        /// Assert the content of the <see cref="HttpResponseData.Body"/> to check if it represent
        /// a JSON object serializable to the <paramref name="expectedJsonObject"/> instance.
        /// </summary>
        /// <typeparam name="T">Type of the serializable JSON object expected.</typeparam>
        /// <param name="expectedJsonObject">Expected serialized JSON object.</param>
        /// <param name="options"><see cref="JsonSerializerOptions"/> used to check the serializable <paramref name="expectedJsonObject"/>.</param>
        /// <returns>The current instance of the <see cref="HttpResponseDataAssertions"/> to continue the assertions.</returns>
        public HttpResponseDataAssertions WithJsonBody<T>(T expectedJsonObject, JsonSerializerOptions? options = null)
        {
            var bodyStream = this.response.GetBodyStream();

            bodyStream.Should().BeJsonDeserializableInto(expectedJsonObject, options);

            return this;
        }

        /// <summary>
        /// Assert the content of the <see cref="HttpResponseData.Body"/> to check if it is
        /// string encoded with the specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="expectedString">Expected string encoded.</param>
        /// <param name="encoding">Encoding of the string to check. If <see langword="null"/>, the UTF-8 encoding is used.</param>
        /// <returns>The current instance of the <see cref="HttpResponseDataAssertions"/> to continue the assertions.</returns>
        public HttpResponseDataAssertions WithStringBody(string expectedString, Encoding? encoding = null)
        {
            if (encoding is null)
            {
                encoding = Encoding.UTF8;
            }

            var bodyStream = this.response.GetBodyStream();

            using var memoryStream = new MemoryStream();

            bodyStream.CopyTo(memoryStream);

            var actualString = encoding.GetString(memoryStream.ToArray());

            actualString.Should().Be(expectedString);

            return this;
        }
    }
}
