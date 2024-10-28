//-----------------------------------------------------------------------
// <copyright file="HttpRequestDataImplementation.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http
{
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;

    internal sealed class HttpRequestDataImplementation : HttpRequestData
    {
        private readonly HttpRequestDataMock mock;

        private readonly List<HttpResponseDataImplementation> responses;

        public HttpRequestDataImplementation(HttpRequestDataMock mock, FunctionContext functionContext)
            : base(functionContext)
        {
            this.mock = mock;

            this.responses = [];
        }

        public override Stream Body => this.mock.Body.Stream;

        public override HttpHeadersCollection Headers => this.mock.Headers;

        public override IReadOnlyCollection<IHttpCookie> Cookies => this.mock.Cookies;

        public override Uri Url => this.mock.Url;

        public override IEnumerable<ClaimsIdentity> Identities => this.mock.Identities;

        public override string Method => this.mock.Method;

        internal IReadOnlyCollection<HttpResponseDataImplementation> Responses => this.responses;

        public override HttpResponseData CreateResponse()
        {
            var response = new HttpResponseDataImplementation(this.FunctionContext);

            this.responses.Add(response);

            return response;
        }
    }
}
