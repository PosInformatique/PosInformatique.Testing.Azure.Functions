//-----------------------------------------------------------------------
// <copyright file="HttpResponseDataImplementationTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http.Tests
{
    using System.Net;
    using Microsoft.Azure.Functions.Worker.Http;

    public class HttpResponseDataImplementationTest
    {
        [Fact]
        public void Headers_ValueChanged()
        {
            var functionContext = new FunctionContextImplementation(new HttpRequestDataMock());

            var headers = new HttpHeadersCollection();

            var response = new HttpResponseDataImplementation(functionContext);

            response.Headers = headers;

            response.Headers.Should().BeSameAs(headers);
        }

        [Fact]
        public void StatusCode_ValueChanged()
        {
            var functionContext = new FunctionContextImplementation(new HttpRequestDataMock());

            var response = new HttpResponseDataImplementation(functionContext);

            response.StatusCode = HttpStatusCode.Continue;

            response.StatusCode.Should().Be(HttpStatusCode.Continue);
        }

        [Fact]
        public void Cookies_NotSupported()
        {
            var functionContext = new FunctionContextImplementation(new HttpRequestDataMock());

            var response = new HttpResponseDataImplementation(functionContext);

            response.Invoking(r => r.Cookies)
                .Should().ThrowExactly<NotSupportedException>()
                .WithMessage("The Cookies properties is not currently supported by the HttpRequestDataMock.");
        }

        [Fact]
        public void GetBodyStream_Default()
        {
            var functionContext = new FunctionContextImplementation(new HttpRequestDataMock());

            var response = new HttpResponseDataImplementation(functionContext);
            response.Body.Write([1, 2]);

            var bodyStream = response.GetBodyStream();

            bodyStream.Position.Should().Be(0);
            bodyStream.Should().BeReadOnly();
            bodyStream.As<MemoryStream>().ToArray().Should().Equal([1, 2]);
        }

        [Fact]
        public void GetBodyStream_Change()
        {
            var functionContext = new FunctionContextImplementation(new HttpRequestDataMock());

            var body = Mock.Of<Stream>(MockBehavior.Strict);

            var response = new HttpResponseDataImplementation(functionContext);
            response.Body = body;

            var bodyStream = response.GetBodyStream();

            bodyStream.Should().BeSameAs(body);
        }
    }
}