namespace cosmosdbinputbinding
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;

    /// <summary>
    /// This sample binds the HTTP Trigger body to a dynamic JSON object and uses one of its attributes to lookup by Id.
    /// </summary>
    /// <remarks>Sample payload is:
    /// {
    ///     "Query": {
    ///         "Id":"SomeId"
    ///     }
    /// }
    /// </remarks>
    public static class HttpInputBindingByIdInBody
    {
        [FunctionName("HttpInputBindingByIdInBody")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            [CosmosDB(databaseName: "%CosmosDBDatabase%",
                collectionName: "%CosmosDBCollection%",
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{Query.Id}")] Document document,
            TraceWriter log)
        {
            if (document == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(document), Encoding.UTF8, "application/json")
            };
        }
    }
}