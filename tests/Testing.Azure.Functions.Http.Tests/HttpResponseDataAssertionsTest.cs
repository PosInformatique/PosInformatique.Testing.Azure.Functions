//-----------------------------------------------------------------------
// <copyright file="HttpResponseDataAssertionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http.Tests
{
    using System.Text.Json;
    using Xunit.Sdk;

    public class HttpResponseDataAssertionsTest
    {
        [Fact]
        public void Constructor()
        {
            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));

            var assertions = new HttpResponseDataAssertions(response);

            assertions.Response.Should().BeSameAs(response);
        }

        [Fact]
        public void WithBody()
        {
            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));
            response.Body.Write([1, 2]);

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            assertions.WithBody(b =>
            {
                var buffer = new byte[5];

                b.ReadAtLeast(buffer, 5, false).Should().Be(2);

                buffer.Should().Equal([1, 2, 0, 0, 0]);
            }).Should().BeSameAs(assertions);

            response.Body.Position.Should().Be(position);
        }

        [Fact]
        public void WithJsonBody_WithoutOptions()
        {
            var json = new
            {
                name = "Gilles",
            };

            using var jsonBytes = new MemoryStream();

            JsonSerializer.Serialize(jsonBytes, json);

            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));
            response.Body.Write(jsonBytes.ToArray());

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            assertions.WithJsonBody(
                new
                {
                    name = "Gilles",
                })
                .Should().BeSameAs(assertions);

            response.Body.Position.Should().Be(position);
        }

        [Fact]
        public void WithJsonBody_WithOptions()
        {
            var json = new
            {
                THE_NAME = "Gilles",
            };

            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper,
            };

            using var jsonBytes = new MemoryStream();

            JsonSerializer.Serialize(jsonBytes, json);

            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));
            response.Body.Write(jsonBytes.ToArray());

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            assertions.WithJsonBody(
                new
                {
                    TheName = "Gilles",
                },
                jsonSerializerOptions)
                .Should().BeSameAs(assertions);

            response.Body.Position.Should().Be(position);
        }

        [Fact]
        public void WithEmptyBody()
        {
            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            assertions.WithEmptyBody().Should().BeSameAs(assertions);

            response.Body.Position.Should().Be(position);
        }

        [Fact]
        public void WithEmptyBody_Failed()
        {
            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));
            response.Body.Write([1, 2]);

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            var act = () =>
            {
                assertions.WithEmptyBody();
            };

            act.Should().ThrowExactly<XunitException>()
                .WithMessage("The body of the response is not empty.");

            response.Body.Position.Should().Be(position);
        }
    }
}