//-----------------------------------------------------------------------
// <copyright file="HttpFunctionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Samples.Tests
{
    using System.Net;
    using global::FluentAssertions;
    using PosInformatique.Testing.Azure.Functions.Http;

    public class HttpFunctionsTest
    {
        [Fact]
        public async Task GetJsonData()
        {
            var request = new HttpRequestDataMock();
            request.Body.SetJson(new
            {
                RequestName = "Gilles TOURREAU",
            });

            var functions = new HttpFunctions();

            var response = await functions.GetJsonData(request.Object);

            response.ShouldMatchRequest(request)
                .WithJsonBody(new
                {
                    Name = "The request name is: Gilles TOURREAU",
                });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task GetRawData()
        {
            var request = new HttpRequestDataMock();

            var functions = new HttpFunctions();

            var response = await functions.GetRawData(request.Object);

            response.ShouldMatchRequest(request)
                .WithBody(body =>
                {
                    var content = new MemoryStream();

                    body.CopyTo(content);

                    content.ToArray().Should().Equal([1, 2]);
                });

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}