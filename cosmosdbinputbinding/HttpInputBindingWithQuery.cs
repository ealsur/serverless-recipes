namespace cosmosdbinputbinding
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using cosmosdbinputbinding.Models;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;

    /// <summary>
    /// This sample binds a custom <see cref="Query"/> class to the HTTP Trigger and uses its attributes in the Cosmos DB query.
    /// </summary>
    /// <remarks>Sample payload is:
    /// {
    ///     "Name": "SomeName",
    ///     "City": "SomeCity"
    /// }
    /// </remarks>
    public static class HttpInputBindingWithQuery
    {
        [FunctionName("HttpInputBindingWithQuery")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] Query httpQuery,
            [CosmosDB(databaseName: "%CosmosDBDatabase%",
                collectionName: "%CosmosDBCollection%",
                ConnectionStringSetting = "CosmosDBConnectionString",
                SqlQuery = "SELECT * FROM d WHERE d.name = {Name} and d.city = {City}")] IEnumerable<dynamic> documents,
            TraceWriter log)
        {
            int totalDocuments = documents.Count();
            log.Info($"Found {totalDocuments} documents");
            if (totalDocuments == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(documents), Encoding.UTF8, "application/json")
            };
        }
    }
}