namespace cosmosdbconnectionmode
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
    /// This sample binds the value of a certain route parameter to a Cosmos DB lookup by id.
    /// </summary>
    public static class CustomConnectionMode
    {
        [FunctionName("CustomConnectionMode")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CustomConnectionMode/{id}")] HttpRequestMessage req,
            [CosmosDB(databaseName: "%CosmosDBDatabase%",
                collectionName: "%CosmosDBCollection%",
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{id}")] Document document,
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
