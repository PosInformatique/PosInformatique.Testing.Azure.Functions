//-----------------------------------------------------------------------
// <copyright file="HttpFunctions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Samples
{
    using System.Net;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;

    public class HttpFunctions
    {
        public HttpFunctions()
        {
        }

        [Function("GetJsonData")]
        public async Task<HttpResponseData> GetJsonData([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData request)
        {
            var jsonRequest = await request.ReadFromJsonAsync<JsonRequest>();

            var response = request.CreateResponse();

            await response.WriteAsJsonAsync(new { Name = $"The request name is: {jsonRequest!.RequestName}" });

            response.StatusCode = HttpStatusCode.Created;

            return response;
        }

        [Function("GetRawData")]
        public async Task<HttpResponseData> GetRawData([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData request)
        {
            var response = request.CreateResponse();

            await response.WriteBytesAsync([1, 2]);

            response.StatusCode = HttpStatusCode.NotFound;

            return response;
        }

        private class JsonRequest
        {
            public string? RequestName { get; set; }
        }
    }
}
