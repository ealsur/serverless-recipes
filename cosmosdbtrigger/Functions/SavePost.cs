using System;
using System.Net;
using System.Net.Http;
using cosmosdbtrigger.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace cosmosdbtrigger
{
    public static class SavePost
    {
        /// <summary>
        /// This HttpTriggered function is called from the web client to save a post into Azure Cosmos DB
        /// </summary>
        [FunctionName("SavePost")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] MessagePayload post, 
            [DocumentDB("%CosmosDBDatabase%", "%CosmosDBCollection%", Id = "id", ConnectionStringSetting = "AzureCosmosDBConnectionString")] out dynamic document)
        {
            document = new { id = Guid.NewGuid(), message = post.message };

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}