namespace cosmosdbtriggers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

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
            LeaseCollectionName = "leases")] IReadOnlyList<Document> events, 
            [CosmosDB(
                databaseName: "%CosmosDBDatabase%",
                collectionName: "%CosmosDBMaterializedCollection%",
                ConnectionStringSetting = "AzureCosmosDBConnectionString",
                CreateIfNotExists = true,
                PartitionKey = "/deviceId"
            )] IAsyncCollector<MaterializedView> materializedView,
            ILogger log)
        {
            foreach (var group in events.GroupBy(singleEvent => singleEvent.GetPropertyValue<string>("deviceId")))
            {
                log.LogInformation($"Generating materialized view for device {group.Key}...");
                await materializedView.AddAsync(new MaterializedView()
                {
                    deviceId = group.Key,
                    maxValue = group.Max(item => item.GetPropertyValue<int>("value"))
                });
            }
        }

        public class MaterializedView
        {
            public string id { get; set; }

            public string deviceId { get; set; }

            public int maxValue { get; set; }
        }
    }
}