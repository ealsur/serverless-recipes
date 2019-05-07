using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(cosmosdbstaticclient.Startup))]

namespace cosmosdbstaticclient
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton((s) => {
                CosmosClientBuilder configurationBuilder = new CosmosClientBuilder(Environment.GetEnvironmentVariable("COSMOSDB_CONNECTIONSTRING"));
                return configurationBuilder.UseConnectionModeDirect()
                        .UseCurrentRegion(Environment.GetEnvironmentVariable("COSMOSDB_REGION"))
                        .Build();
            });
        }
    }
}