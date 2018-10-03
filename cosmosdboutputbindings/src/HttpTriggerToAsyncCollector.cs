using cosmosdboutputbindings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace cosmosdboutputbindings
{
    /// <summary>
    /// This sample binds a custom array of <see cref="MyClass"/> objects from the HTTP Trigger body and uses the Cosmos DB IAsyncCollector to save them.
    /// </summary>
    /// <remarks>Sample payload is:
    /// {
    ///     "id": "SomeId", /* optional */    
    ///     "name": "SomeName",
    ///     "city": "SomeCity"
    /// }
    /// </remarks>
    public static class HttpTriggerToAsyncCollector
    {
        [FunctionName("HttpTriggerToAsyncCollector")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] MyClass[] inputDocuments, 
            [CosmosDB(databaseName: "%CosmosDBDatabase%",
                collectionName: "%CosmosDBCollection%",
                ConnectionStringSetting = "CosmosDBConnectionString")] IAsyncCollector<MyClass> documentsToSave,
            ILogger log)
        {
            if (inputDocuments == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            foreach (MyClass inputDocument in inputDocuments)
            {
                await documentsToSave.AddAsync(inputDocument);
                log.LogInformation(inputDocument.id);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}