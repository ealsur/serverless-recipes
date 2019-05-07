# Serverless recipes with a Cosmos DB custom client
[<- Back to the root](../README.md)

The following project contains a sample on maintaining and reusing a [Azure Cosmos DB V3 client](https://github.com/Azure/azure-cosmos-dotnet-v3). The concept is valid for any HTTP-based client library/SDK.

It contains a single sample:

1. [Startup](./src/Startup.cs): Create and maintain a custom `CosmosClient` instance to be reused in all the Function's executions through [ASP.NET Core Dependency Injection](https://docs.microsoft.com/azure/azure-functions/functions-dotnet-dependency-injection).
2. [HttpTriggerToStaticClient](./src/HttpTriggerToStaticClient.cs): Pulls in the singleton `CosmosClient` and when the Function receives a POST with a particular payload, it uses it to store it as a new document.

## How to run this sample

You can use Visual Studio Code, Visual Studio or even the [Azure Functions' CLI](https://github.com/Azure/azure-functions-core-tools). You only need to customize the [local.settings.json](./src/local.settings.json) file to match the Cosmos DB *Connection String*, *Preferred Region*, *Database name*, and *Container name* on which you want the client to be created and documents to be saved.

## Useful links

* [Azure Cosmos DB + Functions Cookbook — static client](https://medium.com/@Ealsur/azure-cosmos-db-functions-cookbook-static-client-874072aef28e)
* [Managing connections in Azure Functions](https://github.com/Azure/azure-functions-host/wiki/Managing-Connections)