//-----------------------------------------------------------------------
// <copyright file="HttpResponseDataImplementation.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http
{
    using System.Net;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;

    internal sealed class HttpResponseDataImplementation : HttpResponseData
    {
        private MemoryStream? memoryStream;

        private Stream body;

        public HttpResponseDataImplementation(FunctionContext functionContext)
            : base(functionContext)
        {
            this.Headers = [];

            this.memoryStream = new MemoryStream();
            this.body = this.memoryStream;
        }

        public override HttpStatusCode StatusCode
        {
            get;
            set;
        }

        public override HttpHeadersCollection Headers
        {
            get;
            set;
        }

        public override Stream Body
        {
            get => this.body;
            set
            {
                if (this.memoryStream != value)
                {
                    this.memoryStream = null;
                }

                this.body = value;
            }
        }

        public override HttpCookies Cookies
        {
            get
            {
                throw new NotSupportedException("The Cookies properties is not currently supported by the HttpRequestDataMock.");
            }
        }

        public Stream GetBodyStream()
        {
            if (this.memoryStream is not null)
            {
                return new MemoryStream(this.memoryStream.ToArray(), false);
            }

            return this.body;
        }
    }
}
