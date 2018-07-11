using System;
using System.Net;
using System.Net.Http;
using System.Text;
using cosmosdbtrigger.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

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
            string newId = Guid.NewGuid().ToString();
            document = new { id = newId, message = post.message };
            string result = JsonConvert.SerializeObject(new { id = newId });
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(result, Encoding.UTF8, "application/json")
            };
        }
    }
}