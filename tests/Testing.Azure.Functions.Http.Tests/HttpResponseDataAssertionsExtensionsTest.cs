//-----------------------------------------------------------------------
// <copyright file="HttpResponseDataAssertionsExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http.Tests
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Xunit.Sdk;

    public class HttpResponseDataAssertionsExtensionsTest
    {
        [Fact]
        public void ShouldMatchRequest()
        {
            var request = new HttpRequestDataMock();

            var response = request.Object.CreateResponse();
            var response2 = request.Object.CreateResponse();

            response.ShouldMatchRequest(request);
            response2.ShouldMatchRequest(request);
        }

        [Fact]
        public void ShouldMatchRequest_ResponseDifferentRequest()
        {
            var request = new HttpRequestDataMock();
            var otherRequest = new HttpRequestDataMock();

            var response = request.Object.CreateResponse();

            response.Invoking(r => r.ShouldMatchRequest(otherRequest))
                .Should().ThrowExactly<XunitException>()
                .WithMessage("The response does not originate from the request instance.");
        }

        [Fact]
        public void ShouldMatchRequest_ResponseNotCreatedFromMock()
        {
            var request = new HttpRequestDataMock();

            var response = new Mock<HttpResponseData>(MockBehavior.Strict, Mock.Of<FunctionContext>(MockBehavior.Strict));

            response.Object.Invoking(r => r.ShouldMatchRequest(request))
                .Should().ThrowExactly<XunitException>()
                .WithMessage("The response does not originate from a HttpRequestDataMock object.");

            response.VerifyAll();
        }
    }
}