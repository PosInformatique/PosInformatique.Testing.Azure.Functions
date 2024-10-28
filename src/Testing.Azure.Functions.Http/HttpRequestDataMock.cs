//-----------------------------------------------------------------------
// <copyright file="HttpRequestDataMock.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http
{
    using System.Collections.ObjectModel;
    using System.Security.Claims;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Allows to mock an instance of <see cref="HttpRequestData"/> which can be used to assert the <see cref="HttpResponseData"/>
    /// created when calling the <see cref="HttpResponseData.CreateResponse(HttpRequestData)"/>
    /// method.
    /// To assert the <see cref="HttpResponseData"/> created use the <see cref="HttpResponseDataAssertionsExtensions.ShouldMatchRequest(HttpResponseData, HttpRequestDataMock)"/>
    /// extension method.
    /// </summary>
    public sealed class HttpRequestDataMock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestDataMock"/> class.
        /// </summary>
        public HttpRequestDataMock()
        {
            this.Body = new HttpRequestDataMockBody();
            this.Cookies = [];
            this.Headers = [];
            this.Identities = [];
            this.Method = "GET";
            this.Services = new ServiceCollection();
            this.Url = new Uri("http://test.local");

            this.Mock = new HttpRequestDataImplementation(this, new FunctionContextImplementation(this));
        }

        /// <summary>
        /// Gets the <see cref="HttpRequestDataMockBody"/> to define in the <see cref="HttpRequestData.Body"/>
        /// of the <see cref="HttpRequestData"/> mocked.
        /// </summary>
        public HttpRequestDataMockBody Body { get; }

        /// <summary>
        /// Gets the collection of the <see cref="IHttpCookie"/> to define in the <see cref="HttpRequestData.Cookies"/>
        /// of the <see cref="HttpRequestData"/> mocked.
        /// </summary>
        public Collection<IHttpCookie> Cookies { get; }

        /// <summary>
        /// Gets the <see cref="HttpHeadersCollection"/> to define in the <see cref="HttpRequestData.Headers"/>
        /// of the <see cref="HttpResponseData"/> mocked.
        /// </summary>
        public HttpHeadersCollection Headers { get; }

        /// <summary>
        /// Gets the collection of the <see cref="ClaimsIdentity"/> to define in the <see cref="HttpRequestData.Identities"/>
        /// of the <see cref="HttpRequestData"/> mocked.
        /// </summary>
        public Collection<ClaimsIdentity> Identities { get; }

        /// <summary>
        /// Gets or sets the HTTP method to define in the <see cref="HttpRequestData.Method"/>
        /// of the <see cref="HttpRequestData"/> mocked.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the HTTP url to define in the <see cref="HttpRequestData.Url"/>
        /// of the <see cref="HttpRequestData"/> mocked.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets the instance of the mock <see cref="HttpRequestData"/>.
        /// </summary>
        public HttpRequestData Object => this.Mock;

        /// <summary>
        /// Gets the <see cref="ServiceCollection"/> which contains the services that are avaialble
        /// in the <see cref="FunctionContext.InstanceServices"/> of the <see cref="HttpRequestData.FunctionContext"/>
        /// property.
        /// </summary>
        public ServiceCollection Services { get; }

        internal HttpRequestDataImplementation Mock { get; }
    }
}
