//-----------------------------------------------------------------------
// <copyright file="HttpResponseDataAssertionsExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http
{
    using global::FluentAssertions.Common;
    using Microsoft.Azure.Functions.Worker.Http;

    /// <summary>
    /// Contains extensions method to check the <see cref="HttpResponseData"/> created using a <see cref="HttpRequestDataMock"/>.
    /// </summary>
    public static class HttpResponseDataAssertionsExtensions
    {
        /// <summary>
        /// Check if the specified <paramref name="response"/> has been created from the specified <paramref name="request"/>.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseData"/> to check.</param>
        /// <param name="request">The <see cref="HttpRequestDataMock"/> which should create the <paramref name="response"/> instance.</param>
        /// <returns>An instance of the <see cref="HttpResponseDataAssertions"/> which allows to assert the content of the response.</returns>
        public static HttpResponseDataAssertions ShouldMatchRequest(this HttpResponseData response, HttpRequestDataMock request)
        {
            if (response is not HttpResponseDataImplementation responseMock)
            {
                Services.ThrowException("The response does not originate from a HttpRequestDataMock object.");
                return default!;
            }

            if (!request.Mock.Responses.Contains(responseMock))
            {
                Services.ThrowException("The response does not originate from the request instance.");
            }

            return new HttpResponseDataAssertions(responseMock);
        }
    }
}
