//-----------------------------------------------------------------------
// <copyright file="HttpResponseDataAssertionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http.Tests
{
    using System.Text;
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

            act.Should().ThrowExactly<AzureFunctionsAssertionFailedException>()
                .WithMessage("The body of the response is not empty.");

            response.Body.Position.Should().Be(position);
        }

        [Fact]
        public void WithStringBody()
        {
            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));
            response.Body.Write(Encoding.Unicode.GetBytes("The string"));

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            assertions.WithStringBody("The string", Encoding.Unicode)
                .Should().BeSameAs(assertions);

            response.Body.Position.Should().Be(position);
        }

        [Fact]
        public void WithStringBody_DefaultEncoding()
        {
            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));
            response.Body.Write(Encoding.UTF8.GetBytes("The string"));

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            assertions.WithStringBody("The string")
                .Should().BeSameAs(assertions);

            response.Body.Position.Should().Be(position);
        }

        [Fact]
        public void WithStringBody_DifferentValue_Failed()
        {
            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));
            response.Body.Write(Encoding.Unicode.GetBytes("The other string"));

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            var act = () =>
            {
                assertions.WithStringBody("The string", Encoding.Unicode);
            };

            act.Should().ThrowExactly<XunitException>()
                .WithMessage("Expected actualString to be \"The string\" with a length of 10, but \"The other string\" has a length of 16, differs near \"oth\" (index 4).");

            response.Body.Position.Should().Be(position);
        }

        [Fact]
        public void WithStringBody_DifferentEncoding_Failed()
        {
            var response = new HttpResponseDataImplementation(new FunctionContextImplementation(new HttpRequestDataMock()));
            response.Body.Write(Encoding.UTF8.GetBytes("The other string"));

            var assertions = new HttpResponseDataAssertions(response);

            var position = response.Body.Position;

            var act = () =>
            {
                assertions.WithStringBody("The string", Encoding.Unicode);
            };

            act.Should().ThrowExactly<XunitException>()
                .WithMessage("Expected actualString to be \"The string\" with a length of 10, but \"桔\u2065瑯敨\u2072瑳楲杮\" has a length of 8, differs near \"桔\u2065瑯\" (index 0).");

            response.Body.Position.Should().Be(position);
        }
    }
}