using cosmosdbstaticclient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace cosmosdbstaticclient
{
    /// <summary>
    /// This sample binds a custom array of <see cref="MyClass"/> objects from the HTTP Trigger body and uses a custom static <see cref="CosmosClient"/> to save the documents.
    /// </summary>
    /// <remarks>Sample payload is:
    /// [{
    ///     "id": "SomeId", /* optional */    
    ///     "name": "SomeName",
    ///     "city": "SomeCity"
    /// },
    /// {
    ///     "id": "SomeOtherId", /* optional */    
    ///     "name": "SomeOtherName",
    ///     "city": "SomeOtherCity"
    /// }]
    /// </remarks>
    public class HttpTriggerWithStaticClient
    {
        private readonly CosmosClient _cosmosClient;

        public HttpTriggerWithStaticClient(CosmosClient cosmosClient)
        {
            this._cosmosClient = cosmosClient;
        }

        [FunctionName("HttpTriggerWithStaticClient")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            MyClass[] inputDocuments = JsonConvert.DeserializeObject<MyClass[]>(requestBody);

            if (inputDocuments == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            foreach (MyClass inputDocument in inputDocuments)
            {
                // Assumes a container partitioned by /id
                await this._cosmosClient
                    .Databases[Environment.GetEnvironmentVariable("COSMOSDB_DATABASE")]
                    .Containers[Environment.GetEnvironmentVariable("COSMOSDB_CONTAINER")]
                    .Items.CreateItemAsync(inputDocument.id, inputDocument);
                log.LogInformation(inputDocument.id);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}