using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using cosmosdboutputbindings.Models;
using System.Net;
using System.Net.Http;

namespace cosmosdboutputbindings
{
    /// <summary>
    /// This sample binds a custom <see cref="MyClass"/> class from the HTTP Trigger body and uses the Cosmos DB output binding to save it.
    /// </summary>
    /// <remarks>Sample payload is:
    /// {
    ///     "id": "SomeId", /* optional */    
    ///     "name": "SomeName",
    ///     "city": "SomeCity"
    /// }
    /// </remarks>
    public static class HttpTriggerWithSingleDocument
    {
        [FunctionName("HttpTriggerWithSingleDocument")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] MyClass singleDocument,
            [CosmosDB(databaseName: "%CosmosDBDatabase%",
                collectionName: "%CosmosDBCollection%",
                ConnectionStringSetting = "CosmosDBConnectionString")] out MyClass documentToSave,
            ILogger log)
        {
            if (singleDocument == null)
            {
                documentToSave = null;
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            documentToSave = singleDocument;

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}