using cosmosdboutputbindings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace cosmosdboutputbindings
{
    public static class HttpTriggerToAsyncCollector
    {
        [FunctionName("HttpTriggerToAsyncCollector")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] MyClass[] inputDocuments, [CosmosDB(databaseName: "%CosmosDBDatabase%",
                collectionName: "%CosmosDBCollection%",
                ConnectionStringSetting = "CosmosDBConnectionString")] IAsyncCollector<MyClass> documentsToSave, TraceWriter log)
        {
            if (inputDocuments == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            foreach (MyClass inputDocument in inputDocuments)
            {
                await documentsToSave.AddAsync(inputDocument);
                log.Info(inputDocument.id);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}