using cosmosdbstaticclient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace cosmosdbstaticclient
{
    /// <summary>
    /// This sample binds a custom array of <see cref="MyClass"/> objects from the HTTP Trigger body and uses a custom static <see cref="DocumentClient"/> to save the documents.
    /// </summary>
    /// <remarks>Sample payload is:
    /// {
    ///     "id": "SomeId", /* optional */    
    ///     "name": "SomeName",
    ///     "city": "SomeCity"
    /// }
    /// </remarks>
    public static class HttpTriggerWithStaticClient
    {
        private static Uri CollectionUri = UriFactory.CreateDocumentCollectionUri(
            Environment.GetEnvironmentVariable("CosmosDBDatabase"),
            Environment.GetEnvironmentVariable("CosmosDBCollection"));
        private static DocumentClient Client = GetCustomClient();
        private static DocumentClient GetCustomClient()
        {
            DocumentClient customClient = new DocumentClient(
                new Uri(Environment.GetEnvironmentVariable("CosmosDBAccountEndpoint")), 
                Environment.GetEnvironmentVariable("CosmosDBAccountKey"),
                new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp,
                    // Customize retry options for Throttled requests
                    RetryOptions = new RetryOptions()
                    {
                        MaxRetryAttemptsOnThrottledRequests = 10,
                        MaxRetryWaitTimeInSeconds = 30
                    }
                });

            // Customize PreferredLocations
            customClient.ConnectionPolicy.PreferredLocations.Add(LocationNames.SouthCentralUS);
            customClient.ConnectionPolicy.PreferredLocations.Add(LocationNames.EastUS);

            return customClient;
        }
        
        [FunctionName("HttpTriggerWithStaticClient")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] MyClass[] inputDocuments,
            ILogger log)
        {
            if (inputDocuments == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            foreach (MyClass inputDocument in inputDocuments)
            {
                await Client.CreateDocumentAsync(CollectionUri, inputDocument);
                log.LogInformation(inputDocument.id);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}