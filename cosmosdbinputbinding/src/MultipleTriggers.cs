namespace cosmosdbinputbinding
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// This sample shows how to leverage LeaseCollectionPrefix to have independent Triggers sharing the same leases container.
    /// </summary>
    public static class MultipleTriggers
    {
        
        [FunctionName("SendEmails")]
        public static async Task SendEmails([CosmosDBTrigger(
            databaseName: "%CosmosDBDatabase%",
            collectionName: "%CosmosDBCollection%",
            ConnectionStringSetting = "AzureCosmosDBConnectionString",
            LeaseConnectionStringSetting = "AzureCosmosDBConnectionString",
            LeaseCollectionPrefix = "emails",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")] IReadOnlyList<Document> documents, 
            ILogger log)
        {
            // Send emails
        }

        [FunctionName("Materialized")]
        public static async Task SendEmails([CosmosDBTrigger(
            databaseName: "%CosmosDBDatabase%",
            collectionName: "%CosmosDBCollection%",
            ConnectionStringSetting = "AzureCosmosDBConnectionString",
            LeaseConnectionStringSetting = "AzureCosmosDBConnectionString",
            LeaseCollectionPrefix = "materialized",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")] IReadOnlyList<Document> documents, 
            [CosmosDB(
                databaseName: "%CosmosDBDatabase%",
                collectionName: "%CosmosDBMaterializedCollection%",
                ConnectionStringSetting = "AzureCosmosDBConnectionString",
                CreateIfNotExists = true,
                PartitionKey = "/id"
            )] IAsyncCollector<MaterializedDevice> materializedView,
            ILogger log)
        {
            foreach (var document in documents){
                // Do aggregation or transformation
                await materializedView.AddAsync(document);
            }
        }
    }
}