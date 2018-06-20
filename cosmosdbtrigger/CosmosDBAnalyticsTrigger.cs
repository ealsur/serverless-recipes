using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Rest;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using cosmosdbtrigger.Models;

namespace cosmosdbtrigger
{
    public static class CosmosDBAnalyticsTrigger
    {
        private const string languageToAnalyze = "en";
        private static ITextAnalyticsAPI textAnalyticsClient = InitializeTextAnalyticsClient();

        private static ITextAnalyticsAPI InitializeTextAnalyticsClient()
        {
            ITextAnalyticsAPI textAnalyticsClient = new TextAnalyticsAPI(new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("CognitiveServicesKey")));
            textAnalyticsClient.AzureRegion = AzureRegions.Westus; // Change if it's in another region
            return textAnalyticsClient;
        }

        private static AzureSignalR signalR = new AzureSignalR(Environment.GetEnvironmentVariable("AzureSignalRConnectionString"));

        /// <summary>
        /// This Cosmos DB Triggered Function calls Cognitive Services to analyze each new user content and then notify the result connected clients through Azure SignalR.
        /// </summary>
        /// <param name=""chat""></param>
        /// <param name=""lines""></param>
        /// <param name=""AzureCosmosDBConnectionString""></param>
        /// <param name=""AzureCosmosDBConnectionString""></param>
        /// <param name="true"></param>
        /// <param name=""leases""></param>
        /// <returns></returns>
        [FunctionName("CosmosDBAnalyticsTrigger")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "chat",
            collectionName: "lines",
            ConnectionStringSetting = "AzureCosmosDBConnectionString",
            LeaseConnectionStringSetting = "AzureCosmosDBConnectionString",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")] IReadOnlyList<Document> documents, TraceWriter log)
        {
            List<Result> analyticsResults = new List<Result>(documents.Count);

            foreach (var doc in documents)
            {
                try
                {
                    SentimentBatchResult sentiment = await textAnalyticsClient.SentimentAsync(
                        new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                                new MultiLanguageInput(languageToAnalyze, "0" /* id */ , doc.GetPropertyValue<string>("message"))
                            }));

                    var result = new Result()
                    {
                        id = doc.Id,
                        score = sentiment.Documents[0].Score
                    };

                    analyticsResults.Add(result);
                }
                catch (System.Exception ex)
                {
                    // Don't throw to avoid breaking the batch
                    log.Error($"Error processing document {doc.Id}", ex);
                }
            }

            // Notify to all SignalR clients
            await signalR.SendAsync("cosmicServerlessHub", "NewMessages", JsonConvert.SerializeObject(analyticsResults));
        }

        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            private string serviceKey;
            public ApiKeyServiceClientCredentials(string key)
            {
                this.serviceKey = key;
            }

            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Ocp-Apim-Subscription-Key", this.serviceKey);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }
    }
}