//-----------------------------------------------------------------------
// <copyright file="HttpRequestDataMockTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http.Tests
{
    using System.Net;
    using Microsoft.Extensions.DependencyInjection;

    public class HttpRequestDataMockTest
    {
        private interface IService
        {
        }

        [Fact]
        public void Constructor()
        {
            var mock = new HttpRequestDataMock();

            mock.Body.Stream.Should().BeSameAs(Stream.Null);
            mock.Cookies.Should().BeEmpty();
            mock.Identities.Should().BeEmpty();
            mock.Headers.Should().BeEmpty();
            mock.Method.Should().Be("GET");
            mock.Services.Should().BeEmpty();
            mock.Url.Should().Be("http://test.local");
        }

        [Fact]
        public void Method_ValueChanged()
        {
            var mock = new HttpRequestDataMock();

            mock.Method = "The method";

            mock.Method.Should().Be("The method");
        }

        [Fact]
        public void Url_ValueChanged()
        {
            var mock = new HttpRequestDataMock();

            mock.Url = new Uri("http://other/uri");

            mock.Url.Should().Be("http://other/uri");
        }

        [Fact]
        public void Object()
        {
            var body = Mock.Of<Stream>(MockBehavior.Strict);

            var service = new Service();

            var mock = new HttpRequestDataMock()
            {
                Method = "The method",
            };

            mock.Body.Set(body);
            mock.Services.AddSingleton<IService>(service);

            var request = mock.Object;

            request.Body.Should().BeSameAs(body);
            request.Cookies.Should().BeSameAs(mock.Cookies);
            request.Headers.Should().BeSameAs(mock.Headers);
            request.Identities.Should().BeSameAs(mock.Identities);
            request.Method.Should().Be("The method");

            request.FunctionContext.InstanceServices.GetRequiredService<IService>()
                .Should().BeSameAs(service);
        }

        [Fact]
        public void CreateResponse()
        {
            var mock = new HttpRequestDataMock();

            var response = mock.Object.CreateResponse();

            response.Headers.Should().BeEmpty();
            response.StatusCode.Should().Be((HttpStatusCode)0);
        }

        public class Service : IService
        {
        }
    }
}