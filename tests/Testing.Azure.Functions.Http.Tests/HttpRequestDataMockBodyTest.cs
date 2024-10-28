//-----------------------------------------------------------------------
// <copyright file="HttpRequestDataMockBodyTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http.Tests
{
    using System.Text;

    public class HttpRequestDataMockBodyTest
    {
        [Fact]
        public void Set_WithStream()
        {
            var stream = Mock.Of<Stream>(MockBehavior.Strict);

            var body = new HttpRequestDataMockBody();

            body.Set(stream);

            body.Stream.Should().BeSameAs(stream);
        }

        [Fact]
        public void Set_WithBytes()
        {
            var body = new HttpRequestDataMockBody();

            body.Set([1, 2]);

            body.Stream.Position.Should().Be(0);
            body.Stream.As<MemoryStream>().ToArray().Should().Equal([1, 2]);
        }

        [Fact]
        public void Set_WithJson()
        {
            var body = new HttpRequestDataMockBody();

            body.SetJson(new
            {
                Name = "The name",
            });

            body.Stream.Position.Should().Be(0);

            var jsonString = Encoding.UTF8.GetString(body.Stream.As<MemoryStream>().ToArray());

            jsonString.Should().Be("""{"Name":"The name"}""");
        }
    }
}